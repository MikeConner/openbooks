using System.Configuration;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Data;

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
        // if args are empty, try to do both sides of the ETLprovider
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
                        Logger.Instance.SetLogFile(mSettings.Get("LogFile"));

                        if (readFromOnbase)
                        {
                            // Default to SQLServer (in case missing); parse and override if given
                            DatabaseEnum readDBType = DatabaseEnum.SQLServer;
                            DatabaseEnum writeDBType = DatabaseEnum.SQLServer;

                            Enum.TryParse<DatabaseEnum>(mSettings.Get("OnBaseDBtype"), true, out readDBType);
                            Enum.TryParse<DatabaseEnum>(mSettings.Get("PMSDBtype"), true, out writeDBType);

                            OnBaseReader reader = new OnBaseReader(readDBType, writeDBType);

                            reader.DownloadTo(SQLFilename);
                        }

                        if (writeToPMS)
                        {
                            DatabaseEnum dbType = DatabaseEnum.SQLServer;
                            Enum.TryParse<DatabaseEnum>(mSettings.Get("PMSDBtype"), true, out dbType);
                            string batchSize = mSettings.Get("PMSBatchSize");

                            PMSWriter writer = null == batchSize ? new PMSWriter(dbType, DEFAULT_BATCH_SIZE) : new PMSWriter(dbType, Int32.Parse(batchSize));

                            writer.UploadFrom(SQLFilename);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Logger.Instance.LogToFile(ex.ToString());
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
            public OnBaseReader(DatabaseEnum dbReadType, DatabaseEnum dbWriteType)
            {
                mReadDBManager = DBManagerFactory.Instance.CreateDBManager(dbReadType);
                mWriteDBManager = DBManagerFactory.Instance.CreateDBManager(dbWriteType);
            }

            // Write two files: a .sql file that can be run directly (e.g., from CLI), and a raw .txt file with just the values.
            // The raw file is basically a CSV, and can be read and used to update faster (e.g., batched), instead of one line at a time
            public void DownloadTo(string filename)
            {
                Logger.Instance.LogToFile("Downloading to " + filename);

                using (StreamWriter rawWriter = new StreamWriter(filename + RAW_FILE_EXTENSION), sqlWriter = new StreamWriter(filename + SQL_FILE_EXTENSION))
                {
                    string strConn = mReadDBManager.GenerateConnectionString(mSettings.Get("onbaseDBuser"), mSettings.Get("onbaseDBpassword"), 
                                                                             mSettings.Get("onbaseDBserver"), mSettings.Get("onbaseDBname"), 
                                                                             Boolean.Parse(mSettings.Get("ONBintegratedSecurity")));
                    Logger.Instance.LogToFile("Attempting connection to Onbase using: '" + strConn + "'");

                    if (mReadDBManager.EstablishConnection(strConn))
                    {
                        try
                        {
                            Logger.Instance.LogToFile("Opened database connection to OnBase");

                            if ("true" == mSettings.Get("SQLDebug"))
                            {
                                Logger.Instance.LogToFile(mReadDBManager.OnBaseFetch(mSettings));
                            }

                            using (IDataReader reader = mReadDBManager.ExecuteQuery(mReadDBManager.OnBaseFetch(mSettings)))
                            {
                                Logger.Instance.LogToFile("Fetched OnBase data; writing to file");
                                int lines = 0;
                                bool smallInt = "32" == mSettings.Get("OnbaseIntSize");

                                while (reader.Read())
                                {
                                    string values = smallInt ? mWriteDBManager.GenerateRawInsertData(reader.GetString(0).Trim(), reader.GetStringOrNull(1), reader.GetShortDateOrNull(2), reader.GetInt32(3).ToString()) :
                                                               mWriteDBManager.GenerateRawInsertData(reader.GetString(0).Trim(), reader.GetStringOrNull(1), reader.GetShortDateOrNull(2), reader.GetInt64(3).ToString());
                                    string stmt = smallInt ? mWriteDBManager.GenerateInsertStatement(mSettings.Get("PMSDestTable") , reader.GetString(0).Trim(), reader.GetStringOrNull(1), reader.GetShortDateOrNull(2, mWriteDBManager.GetDatabaseType()), reader.GetInt32(3).ToString()) :
                                                             mWriteDBManager.GenerateInsertStatement(mSettings.Get("PMSDestTable"), reader.GetString(0).Trim(), reader.GetStringOrNull(1), reader.GetShortDateOrNull(2, mWriteDBManager.GetDatabaseType()), reader.GetInt64(3).ToString());

                                    rawWriter.WriteLine(values);
                                    sqlWriter.WriteLine(stmt);
                                    lines++;
                                }

                                Logger.Instance.LogToFile("Wrote " + lines + " lines to PMS upload files");
                            }
                        }
                        finally
                        {
                            mReadDBManager.CloseConnection();
                        }
                    }
                }
            }

            private DBManager mReadDBManager = null;
            private DBManager mWriteDBManager = null;
        }

        class PMSWriter
        {
            public PMSWriter(DatabaseEnum dbType, int batchSize = DEFAULT_BATCH_SIZE)
            {
                mDbType = dbType;
                mBatchSize = batchSize;
                mDBManager = DBManagerFactory.Instance.CreateDBManager(dbType);
            }

            public virtual string PMSConnectionString
            {
                get
                {
                    string strConn = "User ID=" + mSettings.Get("PMSDBuser") + ";Password=" + mSettings.Get("PMSDBpassword") + ";Data Source=" + mSettings.Get("PMSDBserver");
                    if (DatabaseEnum.SQLServer == mDbType)
                    {
                        strConn += ";Initial Catalog=" + mSettings.Get("PMSDBname");
                    }

                    return strConn;
                }
            }

            /* Prepared Statement (hard to make generic; defer until needd. Don't pre-optimize)

                                    using (OracleCommand command = new OracleCommand(PMSInsertCommand, conn))
                                    {
                                        OracleParameter docketParam = new OracleParameter(":docket_num", OracleDbType.Varchar2, 30);  // VarChar OracleDbType / SqlDbType
                                        OracleParameter doctypeParam = new OracleParameter(":doc_type", OracleDbType.Char, 66);
                                        OracleParameter dateFiledParam = new OracleParameter(":date_filed", OracleDbType.TimeStamp, 8);// DateTime
                                        OracleParameter docidParam = new OracleParameter(":docid", OracleDbType.Int32, 0);  // Int

                                        command.Parameters.Add(docketParam);
                                        command.Parameters.Add(doctypeParam);
                                        command.Parameters.Add(dateFiledParam);
                                        command.Parameters.Add(docidParam);

                                        command.Prepare();

                                        Logger.Instance.LogToFile("Preparations for batch insertion complete; batch size = " + mBatchSize);

                                        using (StreamReader reader = new StreamReader(filename + RAW_FILE_EXTENSION))
                                        {
                                            string line;
                                            //IDbTransaction dbtx = conn.BeginTransaction();

                                            OracleTransaction tx = conn.BeginTransaction();

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
                                                            Logger.Instance.LogToFile("Batch committed to PMS");
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string message = "Write failed on index " + cnt + ". " + ex.ToString();

                                                            Console.WriteLine(message);
                                                            Logger.Instance.LogToFile(message);
                                                            tx.Rollback();
                                                        }

                                                        tx = conn.BeginTransaction();
                                                        command.Transaction = tx;
                                                        cnt = mBatchSize;
                                                    }
                                                }
                                                else
                                                {
                                                    string message = "Invalid line: " + line;
                                                    Console.WriteLine(message);
                                                    Logger.Instance.LogToFile(message);
                                                }
                                            }

                                            try
                                            {
                                                tx.Commit();
                                            }
                                            catch (Exception ex)
                                            {
                                                string message = "Write failed on final commit: " + ex.ToString();
                                                Console.WriteLine(message);
                                                Logger.Instance.LogToFile(message);
                                                tx.Rollback();
                                            }
                                        }
                                    }*/

            public virtual void UploadFrom(string filename)
            {
                Logger.Instance.LogToFile("Uploading from " + filename);

                if (mDBManager.EstablishConnection(PMSConnectionString))
                {
                    try
                    {
                        Logger.Instance.LogToFile("SQL connection to PMS established.");

                        mDBManager.ExecuteCommand("DELETE FROM " + mSettings.Get("PMSDestTable"));

                        Logger.Instance.LogToFile("Cleared old table");
                        int cnt = 0;
                        int numWrote = 0;
                        string message = null;

                        using (StreamReader reader = new StreamReader(filename + RAW_FILE_EXTENSION))
                        {
                            string line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                cnt++;

                                string[] fields = line.Split(',');
                                if (4 == fields.Length)
                                {
                                    try
                                    {
                                        string cmd = mDBManager.GenerateInsertStatement(mSettings.Get("PMSDestTable"), fields[0], fields[1], fields[2], fields[3]);

                                        mDBManager.ExecuteCommand(cmd);
                                        numWrote++;
                                    }
                                    catch (Exception ex)
                                    {
                                        message = "Error on line " + cnt + ": " + ex.Message;

                                        Console.WriteLine(message);
                                        Logger.Instance.LogToFile(message);
                                    }
                                }
                                else
                                {
                                    message = "Invalid line: " + line + " (" + cnt + ")";
                                    Console.WriteLine(message);
                                    Logger.Instance.LogToFile(message);
                                }
                            }
                        }

                        message = "Read " + cnt + " lines from OnBase; wrote " + numWrote + " lines to PMS.";

                        Console.WriteLine(message);
                        Logger.Instance.LogToFile(message);
                    }
                    finally
                    {
                        mDBManager.CloseConnection();
                    }
                }
            }

            private DBManager mDBManager = null;

            private DatabaseEnum mDbType = DatabaseEnum.SQLServer;
            private int mBatchSize = -1;
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
