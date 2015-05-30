using System.Configuration;
using System;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Globalization;

/// <summary>
/// Periodically transfer data from an OnBase installation to a PMS installation (attorney general database).
/// </summary>
/// <remarks>
/// CHARTER: Download from OnBase, store in a temporary file on the file system, upload to PMS.
/// 
/// USAGE: Run periodically (e.g., daily). Process is stateless. Can run the download and upload operations separately.
///        
/// NOTES AND WARNINGS: 
/// 
/// Configuration on their side: create DB user on ONBase with db_datareader (and public) access to onbase.  SQL authentication.  
/// schema db_datareader / database role also db_datareader / server role: public

/// We are doing the full set so the process is stateless - no support for diffs at this time. We anticipate sub-minute running times.

/// They will have Oracle -> version 9-12

/// Oracle to oracle (westmoreland)
/// Washington, Allegheny counties own Public Access; not Carbon county
/// doc IDs unique - no grouping!!!! WOOOOO!!
/// link to each physical document

/// PDF viewer is default. If they have Public Access, use DocPop for PDF on the fly.
/// Otherwise might have to serve binary and let system open with Adobe?
/// For the moment, assume they have (or can acquire) Public Access. First two targets have it anyway.
///
/// instructions:     Find ONBdocketNumberOwner (K325 for test system) - This is the docket number keyword
///                  Find ONBkeyValueDatefieldTable (keyitem167 for test system) - this is the date filed keyword
///                  Find ONBDocketNumberTable (hsi.keyitem325 for test system.  hsi.keyitem + number for DocketNumber) - Docket number
/// </remarks>               

namespace OnBasePMS
{
    public enum DatabaseEnum
    {
        SQLServer,
        Oracle
    }

    class Program
    {
        // if args are empty, try to do both sides of the ETL
        // if args has value 'OnbaseOnly' - only perform onbase portion
        // if args has value 'PMSonly' - only do the data insert
        // if args has value 'help' - show the previous options
        // args can be used to do diff - we will need a way of persisting a time stamp in that case (not used now)

        public const string USAGE = "[help,OnbaseOnly,PMSonly]";
        public const int DEFAULT_BATCH_SIZE = 1000;

        public static void Main(string[] args)
        {
            bool readFromOnbase = true;
            bool writeToPMS = true;

            if (args.Length > 0)
            {
                if ("help" == args[0].ToLower())
                {
                    Console.WriteLine(USAGE);
                    Environment.Exit(1);
                }
                else if ("onbaseonly" == args[0].ToLower())
                {
                    writeToPMS = false;
                }
                else if ("pmsonly" == args[0].ToLower())
                {
                    readFromOnbase = false;
                }
            }

            try
            {
                // Delete any extension. We will write a .sql file with Insert statements, and another file with just the raw data
                string SQLFilename = Path.GetFileNameWithoutExtension(mSettings.Get("IntermediateSQLFile"));
                if (null == SQLFilename)
                {
                    Console.WriteLine("Please supply an IntermediateSQLFile setting.");
                    Environment.Exit(1);
                }
                else
                {
                    try
                    {
                        if (readFromOnbase)
                        {
                            DatabaseEnum dbType = (DatabaseEnum)Enum.Parse(typeof(DatabaseEnum), mSettings.Get("PMSDBtype"), true);
                            OnBaseReader reader = new OnBaseReader(dbType);

                            reader.DownloadTo(SQLFilename);
                        }

                        if (writeToPMS)
                        {
                            string batchSize = mSettings.Get("PMSBatchSize");

                            PMSWriter writer = null == batchSize ? new PMSWriter(DEFAULT_BATCH_SIZE) : new PMSWriter(Int32.Parse(batchSize));

                            writer.UploadFrom(SQLFilename);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        class OnBaseReader
        {
            public OnBaseReader(DatabaseEnum dbType)
            {
                mDbType = dbType;
            }

            public string OnBaseConnectionString
            {
                get
                {
                    return "User ID=" + mSettings.Get("onbaseDBuser") + ";Password=" + mSettings.Get("onbaseDBpassword") + ";Initial Catalog=" + mSettings.Get("onbaseDBname") + ";Data Source=" + mSettings.Get("onbaseDBserver");
                }
            }

            public string OnBaseFetch
            {
                get
                {
                    string SQLselect = "";
                    string SQLFrom = "";
                    string SQLWhere = "";

                    SQLselect = "SELECT " + mSettings.Get("ONBdocketNumberOwner") + "." +
                        mSettings.Get("ONBdocketNumberField")
                        + " AS 'docketNum', " +

                        "(select " + mSettings.Get("ONBitemtypenameField") +
                        " from " + mSettings.Get("ONBitemTypeTable") + "  where " +
                        mSettings.Get("ONBitemtypenumField") + " = i." +
                        mSettings.Get("ONBitemtypenumField") + ") as doc_type, " +

                        "(select " + mSettings.Get("ONBkeyvaluedateField") +
                        " from " + mSettings.Get("ONBkeyValueDatefieldTable") +
                        " where " + mSettings.Get("ONBitemnumField") + " = i." +
                        mSettings.Get("ONBitemnumField") + ") as date_filed" +

                        ", i." + mSettings.Get("ONBitemnumField") + " as docid ";

                    SQLFrom = " FROM " + mSettings.Get("ONBhsiItemDataTable") + "  i " +
                        "LEFT JOIN " + mSettings.Get("ONBDocketNumberTable") + " " +
                        mSettings.Get("ONBdocketNumberOwner") + " ON " +
                        mSettings.Get("ONBdocketNumberOwner") +
                        "." + mSettings.Get("ONBitemnumField") + " = i." +
                        mSettings.Get("ONBitemnumField");


                    SQLWhere = " WHERE " + mSettings.Get("ONBdocketNumberOwner") + "." +
                        mSettings.Get("ONBitemnumField") + " IS NOT NULL ";

                   return SQLselect + SQLFrom + SQLWhere;
                }
            }

            // Write two files: a .sql file that can be run directly (e.g., from CLI), and a raw .txt file with just the values.
            // The raw file is basically a CSV, and can be read and used to update faster (e.g., batched), instead of one line at a time
            public void DownloadTo(string filename)
            {
                using (StreamWriter rawWriter = new StreamWriter(filename + RAW_FILE_EXTENSION), sqlWriter = new StreamWriter(filename + SQL_FILE_EXTENSION))
                {
                    using (SqlConnection conn = new SqlConnection(OnBaseConnectionString))
                    {
                        conn.Open();

                        using (SqlCommand command = new SqlCommand(OnBaseFetch, conn))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {   
                                    // NOTE: The Nullables will return quotes around a valid string, or quoteless "null"                                
                                    string values = reader.GetString(0).Trim() + "," +
                                                    reader.GetStringOrNull(1) + "," +
                                                    reader.GetShortDateOrNull(2) + "," +
                                                    reader.GetInt32(3);
                                    string sqlValues = "'" + reader.GetString(0).Trim() + "'," +
                                                       reader.GetStringOrNull(1) + "," +
                                                       reader.GetShortDateOrNull(2, mDbType) + "," +
                                                       reader.GetInt32(3);
                                    string stmt = "INSERT INTO " + mSettings.Get("PMSDestTable") + " (docket_num,doc_type,date_filed,docid)" +
                                                  " VALUES (" + sqlValues + ");";

                                    rawWriter.WriteLine(values);
                                    sqlWriter.WriteLine(stmt);
                                }
                            }
                        }
                    }
                }
            }

            private DatabaseEnum mDbType = DatabaseEnum.SQLServer;
        }

        class PMSWriter
        {
            public PMSWriter(int batchSize = DEFAULT_BATCH_SIZE)
            {
                mBatchSize = batchSize;
            }

            public virtual string PMSConnectionString
            {
                get
                {
                    return "User ID=" + mSettings.Get("PMSDBuser") + ";Password=" + mSettings.Get("PMSDBpassword") + ";Initial Catalog=" + mSettings.Get("PMSDBname") + ";Data Source=" + mSettings.Get("PMSDBserver");
                }
            }

            public virtual string PMSInsertCommand
            {
                get
                {
                    return "INSERT INTO " + mSettings.Get("PMSDestTable") + " VALUES(@docket_num,@doc_type,@date_filed,@docid)";
                }
            }

            public virtual void UploadFrom(string filename)
            {
                using (SqlConnection conn = new SqlConnection(PMSConnectionString))
                {
                    conn.Open();

                    // Blow away old temp table
                    using (SqlCommand command = new SqlCommand("DELETE FROM " + mSettings.Get("PMSDestTable"), conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(PMSInsertCommand, conn))
                    {
                        SqlParameter docketParam = new SqlParameter("@docket_num", SqlDbType.VarChar, 30);
                        SqlParameter doctypeParam = new SqlParameter("@doc_type", SqlDbType.Char, 66);
                        SqlParameter dateFiledParam = new SqlParameter("@date_filed", SqlDbType.DateTime, 8);
                        SqlParameter docidParam = new SqlParameter("@docid", SqlDbType.Int, 0);

                        command.Parameters.Add(docketParam);
                        command.Parameters.Add(doctypeParam);
                        command.Parameters.Add(dateFiledParam);
                        command.Parameters.Add(docidParam);

                        command.Prepare();

                        using (StreamReader reader = new StreamReader(filename + RAW_FILE_EXTENSION))
                        {
                            string line;
                            SqlTransaction tx = conn.BeginTransaction();

                            int cnt = mBatchSize;
                            command.Transaction = tx;

                            while ((line = reader.ReadLine()) != null)
                            {
                                string[] fields = line.Split(',');
                                if (4 == fields.Length)
                                {
                                    command.Parameters[0].Value = fields[0];
                                    command.Parameters[1].Value = "null" == fields[1] ? (object)DBNull.Value : fields[1];
                                    command.Parameters[2].Value = "null" == fields[2] ? (object)DBNull.Value : DateTime.ParseExact(fields[2].Replace("'", ""), "yyyyMMdd", CultureInfo.InvariantCulture);
                                    command.Parameters[3].Value = Int32.Parse(fields[3]);

                                    command.ExecuteNonQuery();
                                    cnt--;

                                    if (0 == cnt)
                                    {
                                        try
                                        {
                                            tx.Commit();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Write failed on index " + cnt + ". " + ex.ToString());
                                            tx.Rollback();
                                        }

                                        tx = conn.BeginTransaction();
                                        command.Transaction = tx;
                                        cnt = mBatchSize;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid line: " + line);
                                }
                            }

                            try
                            {
                                tx.Commit();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Write failed on final commit: " + ex.ToString());
                                tx.Rollback();
                            }
                        }
                    }
                }
            }

            private int mBatchSize;
        }

        private const string RAW_FILE_EXTENSION = ".txt";
        private const string SQL_FILE_EXTENSION = ".sql";

        private static NameValueCollection mSettings = ConfigurationManager.AppSettings;
    }

    /// <summary>
    /// Tolerate null values when reading from OnBase (helper function)
    /// </summary>
    /// <remarks>
    /// CHARTER: Some data elements (namely date_filed and doc_type) can be null. 
    /// 
    /// USAGE: Call these on DataReaders instead of GetString/GetDateTime
    ///
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Get a string value from a DataReader that might be null 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns>single-quoted string, or unquoted null</returns>
        public static string GetStringOrNull(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? "null" : "'" + reader.GetString(ordinal).Trim() + "'";
        }

        /// <summary>
        /// Get a string value from a DataReader that might be null, given a column name
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns>single-quoted string, or unquoted null</returns>
        public static string GetStringOrNull(this IDataReader reader, string columnName)
        {
            return reader.GetStringOrNull(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get an integer value from a DataReader that might be null
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns>Integer value, or null</returns>
        public static Nullable<int> GetIntOrNull(this IDataReader reader, int ordinal)
        {
            Nullable<int> result = null;

            if (!reader.IsDBNull(ordinal))
            {
                result = reader.GetInt32(ordinal);
            }

            return result;
        }

        /// <summary>
        /// Get an integer value from a DataReader that might be null, given a column name
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns>Integer value, or null</returns>
        public static Nullable<int> GetIntOrNull(this IDataReader reader, string columnName)
        {
            return reader.GetIntOrNull(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a date value from a DataReader that might be null; format it for SQL insertion
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ordinal"></param>
        /// <returns>date format, or null</returns>
        public static string GetShortDateOrNull(this IDataReader reader, int ordinal, DatabaseEnum dbType = DatabaseEnum.SQLServer)
        {
            switch (dbType)
            {
                case DatabaseEnum.Oracle:
                    return reader.IsDBNull(ordinal) ? "null" : "to_date('" + String.Format("{0:yyyyMMdd}", reader.GetDateTime(ordinal)) + "', 'YYYYMMDD')";
                default:
                    return reader.IsDBNull(ordinal) ? "null" : "'" + String.Format("{0:yyyyMMdd}", reader.GetDateTime(ordinal)) + "'"; 
            }
        }

        public static string GetShortDateOrNull(this IDataReader reader, string columnName, DatabaseEnum dbType = DatabaseEnum.SQLServer)
        {
            return reader.GetShortDateOrNull(reader.GetOrdinal(columnName), dbType);
        }
    }
}
