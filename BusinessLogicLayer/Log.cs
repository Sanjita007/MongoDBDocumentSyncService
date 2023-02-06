using System;
using System.IO;

namespace BusinessLogicLayer
{
    public class Log
    {
        private static string GetFolderName()
        {
            string logFolderName = AppDomain.CurrentDomain.BaseDirectory + "\\LOG";
            // If directory does not exist, create
            if (!Directory.Exists(logFolderName))
            {
                Directory.CreateDirectory(logFolderName);
            }

            return logFolderName;
        }

        public static void WriteLog(string message)
        {
           
            using (StreamWriter sw = new StreamWriter(GetFolderName() + $"\\Sync LogFile-{DateTime.Now:yyyy-MM-dd}.txt", true))
            {
                sw.WriteLine(message);
                sw.WriteLine(DateTime.Now.ToString());
            }
        }

        public static void WriteBackupLog(string message)
        {
            using (StreamWriter sw = new StreamWriter(GetFolderName() + $"\\Backup LogFile-{DateTime.Now:yyyy-MM-dd}.txt", true))
            {
                sw.WriteLine(message);
                sw.WriteLine(DateTime.Now.ToString());
            }
        }
    }
}
