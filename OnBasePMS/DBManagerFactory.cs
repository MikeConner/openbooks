using System;

namespace OnBasePMS
{
    public sealed class DBManagerFactory
    {
        private static volatile DBManagerFactory instance;
        private static object syncRoot = new Object();

        private DBManagerFactory() { }

        public DBManager CreateDBManager(DatabaseEnum dbType)
        {
            switch (dbType)
            {
                case DatabaseEnum.SQLServer: return new SQLServerDBManager();
                case DatabaseEnum.Oracle: return new OracleDBManager();
                default: throw new ArgumentException("Unknown database type");
            }
        }

        public static DBManagerFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DBManagerFactory();
                    }
                }

                return instance;
            }
        }
    }
}
