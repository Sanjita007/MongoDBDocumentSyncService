using MongoDocumentSyncService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusinessLogicLayer.BusinessLogic
{
    public class BLLDocumentBackup : DAL
    {
        string _cStr = "";

        public BLLDocumentBackup(string ConnStr) : base(connectionString: ConnStr)
        {
            _cStr = ConnStr;
        }

        public bool DumpDocumentBackup(string backupPath)
        {
            backupPath = CreateProperPathForBackup(backupPath);

            bool res = true;
            // if we want to backup
            List<ClientMast> clientMasts = new BLLClientMast(_cStr).GetClientList();
            foreach (var client in clientMasts.Where(r => r.DocServerDataSource != "" && r.DocServerUserID != "" && r.DocServerPassword != ""))
            {
                try
                {
                    string[] serverInfo = client.DocServerDataSource.Split(':');

                    Log.WriteLog($"server info: {serverInfo}");
                    Log.WriteBackupLog($"Backup started in client :- {client.ClientName}, DB {client.DbName}...");

                    StreamWriter SW = new StreamWriter("backup.bat");

                    string str = $@"mongodump --username=""{client.DocServerUserID}"" --password=""{client.DocServerPassword}"" --db=""{client.DbName}"" --authenticationDatabase=admin --out=""{backupPath}"" --host=""{serverInfo[0]}"" --port=""{serverInfo[1]}""";

                    Log.WriteBackupLog(str);
                    SW.WriteLine(str);

                    SW.Flush();
                    SW.Close();
                    SW.Dispose();

                    System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        FileName = "backup.bat",
                        Arguments = str,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    };

                    System.Diagnostics.Process process = new System.Diagnostics.Process
                    {
                        StartInfo = processStartInfo,

                    };
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    Log.WriteBackupLog(output);
                    Log.WriteBackupLog(error);

                    if (Directory.Exists($"{backupPath}{client.DbName}"))
                        Log.WriteBackupLog($"Successful database backup in client {client.ClientName}...");
                    else
                        Log.WriteBackupLog($"Cannot backup database in client {client.ClientName}...");

                }
                catch (Exception ex)
                {
                    Log.WriteLog("Exception occured...");
                    Log.WriteLog(ex.ToString());
                }
            }

            return res;
        }

        public string CreateProperPathForBackup(string backup)
        {
            string path = backup + $"DB Backup {DateTime.Now:yyyy-MM-dd}/";
            Directory.CreateDirectory(path);

            Log.WriteLog($"Backup Path: {path}");
            return path;
        }
    }
}
