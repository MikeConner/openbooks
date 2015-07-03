using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace OnBasePMS
{
    public abstract class DBManager
    {
        public abstract bool EstablishConnection(string strConnection);
        public abstract void CloseConnection();
        public abstract DatabaseEnum GetDatabaseType();
        public abstract string GetIntegratedSecurityFlag(bool integratedSecurity);

        public virtual string GenerateConnectionString(string user, string password, string server, string database, bool integratedSecurity)
        {
            return "User ID=" + user + ";Password=" + password + ";Data Source=" + server + GetIntegratedSecurityFlag(integratedSecurity);
        }

        public virtual string GenerateRawInsertData(string docket, string item_type, string date_filed, string document_id)
        {
            return docket + "," + item_type + "," + date_filed + "," + document_id;
        }

        public abstract string GenerateInsertStatement(string table_name, string docket, string item_type, string date_filed, string document_id);

        public virtual string OnBaseFetch(NameValueCollection settings)
        {
            string SQLselect = "";
            string SQLFrom = "";
            string SQLWhere = "";

            bool dualTable = Boolean.Parse(settings.Get("ONBDocketDualTable"));
            string owner = dualTable ? "dualOwner" : "docketTableOwner";

            SQLselect = "SELECT " + owner + "." + settings.Get("ONBdocketNumberField")
                + " AS 'docketNum', " +

                "(select " + settings.Get("ONBitemtypenameField") +
                " from " + settings.Get("ONBitemTypeTable") + "  where " +
                settings.Get("ONBitemtypenumField") + " = i." +
                settings.Get("ONBitemtypenumField") + ") as doc_type, " +

                "(select " + settings.Get("ONBkeyvaluedateField") +
                " from " + settings.Get("ONBkeyValueDatefieldTable") +
                " where " + settings.Get("ONBitemnumField") + " = i." +
                settings.Get("ONBitemnumField") + ") as date_filed" +

                ", i." + settings.Get("ONBitemnumField") + " as docid ";

            SQLFrom = " FROM " + settings.Get("ONBhsiItemDataTable") + "  i " +
                "LEFT JOIN " + settings.Get("ONBDocketNumberTable") + " docketTableOwner " +
                " ON docketTableOwner." +
                settings.Get("ONBitemnumField") + " = i." +
                settings.Get("ONBitemnumField");

            if (dualTable)
            {
                SQLFrom += " LEFT JOIN " + settings.Get("ONBDocketDualTableName") + " dualOwner ON dualOwner.keywordnum = " + "docketTableOwner.keywordnum";
            }

            SQLWhere = " WHERE docketTableOwner." +
                settings.Get("ONBitemnumField") + " IS NOT NULL ";

            return SQLselect + SQLFrom + SQLWhere;
        }

        public abstract IDataReader ExecuteQuery(string query);
        public abstract void ExecuteCommand(string command);
    }

    public class SQLServerDBManager : DBManager
    {
        public override DatabaseEnum GetDatabaseType()
        {
            return DatabaseEnum.SQLServer;
        }

        public override string GetIntegratedSecurityFlag(bool integratedSecurity)
        {
            string strFlag = ";Integrated Security=";
            strFlag += integratedSecurity ? "true" : "false";

            return strFlag;
        }

        public override bool EstablishConnection(string strConnection)
        {
            try
            {
                mConnection = new SqlConnection(strConnection);
                mConnection.Open();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Instance.LogToFile(ex.Message);
            }

            return false;
        }

        public override void CloseConnection()
        {
            if (null != mConnection)
            {
                mConnection.Close();
            }
        }

        public override string GenerateConnectionString(string user, string password, string server, string database, bool integratedSecurity)
        {
            return base.GenerateConnectionString(user, password, server, database, integratedSecurity) + ";Initial Catalog=" + database;
        }

        public override string GenerateInsertStatement(string table_name, string docket, string item_type, string date_filed, string document_id)
        {
            string cmd = "INSERT INTO " + table_name + "(docket,document_id";
            string values = ") VALUES ('" + docket + "'," + document_id;

            if (null != item_type)
            {
                cmd += ",item_type";
                values += "," + item_type;
            }
            if (null != date_filed)
            {
                cmd += ",date_filed";
                values += "," + date_filed;
            }

            values += ")";

            return cmd + values;

        }

        public override IDataReader ExecuteQuery(string command)
        {
            using (SqlCommand cmd = new SqlCommand(command, mConnection))
            {
                return cmd.ExecuteReader();
            }
        }

        public override void ExecuteCommand(string command)
        {
            using (SqlCommand cmd = new SqlCommand(command, mConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private SqlConnection mConnection = null;
    }

    public class OracleDBManager : DBManager
    {
        public override DatabaseEnum GetDatabaseType()
        {
            return DatabaseEnum.Oracle;
        }

        public override string GetIntegratedSecurityFlag(bool integratedSecurity)
        {
            string strFlag = ";Integrated Security=";
            strFlag += integratedSecurity ? "yes" : "no";

            return strFlag;
        }

        public override bool EstablishConnection(string strConnection)
        {
            try
            {
                mConnection = new OracleConnection(strConnection);
                mConnection.Open();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Instance.LogToFile(ex.Message);
            }

            return false;
        }

        public override void CloseConnection()
        {
            if (null != mConnection)
            {
                mConnection.Close();
            }
        }

        public override IDataReader ExecuteQuery(string query)
        {
            using (OracleCommand cmd = new OracleCommand(query, mConnection))
            {
                return cmd.ExecuteReader();
            }
        }

        public override void ExecuteCommand(string command)
        {
            using (OracleCommand cmd = new OracleCommand(command, mConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public override string GenerateInsertStatement(string table_name, string docket, string item_type, string date_filed, string document_id)
        {
            string cmd = "INSERT INTO " + table_name + "(docket,document_id";
            string values = ") VALUES ('" + docket + "'," + document_id;

            if ("null" != item_type)
            {
                cmd += ",item_type";
                values += "," + item_type;
            }
            if ("null" != date_filed)
            {
                cmd += ",date_filed";

                // Depending on the source, this might or not have had to_date already applied
                if (-1 == date_filed.ToLower().IndexOf("to_date"))
                {
                    date_filed = "to_date(" + date_filed + ", 'yyyymmdd')";
                }
                values += "," + date_filed;
            }

            values += ")";

            return cmd + values;
        }

        private OracleConnection mConnection = null;
    }
}
