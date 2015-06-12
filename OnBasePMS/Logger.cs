using System;
using System.IO;

namespace OnBasePMS
{
    public sealed class Logger
    {
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        private Logger() { }

        public void LogToFile(string message)
        {
            if (null != mLogFilename)
            {
                File.AppendAllText(mLogFilename, message + Environment.NewLine);
            }
        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }

        public void SetLogFile(string logFilename)
        {
            try
            {
                using (FileStream fs = File.Create(logFilename, 1, FileOptions.DeleteOnClose))
                {
                    mLogFilename = logFilename;
                }
            }
            catch
            {
                Console.WriteLine("Cannot write to Log File: " + logFilename);
            }

        }

        private string mLogFilename = null;
    }
}
