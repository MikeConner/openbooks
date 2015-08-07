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
                File.AppendAllText(mLogFilename, DateTime.Now.ToString() + " " + message + Environment.NewLine);
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

        public void SetLogFile(string logFilename, bool append = true)
        {
            try
            {
                if (append)
                {
                    using (FileStream fs = new FileStream(logFilename, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write)) // File.Create(logFilename))
                    {
                        mLogFilename = logFilename;
                    }
                }
                else
                {
                    using (FileStream fs = File.Create(logFilename, 1, FileOptions.DeleteOnClose))
                    {
                        mLogFilename = logFilename;
                    }
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
