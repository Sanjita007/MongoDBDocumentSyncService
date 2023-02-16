using BusinessLogicLayer;
using BusinessLogicLayer.BusinessLogic;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace MongoDocumentSyncService
{
    public partial class MongoDocScheduler : ServiceBase
    {
        private Timer timer = null;
        readonly BLLDocument bllDoc = null;
        readonly BLLDocumentBackup blDocBackup = null;
        public MongoDocScheduler()
        {
            InitializeComponent();
            string cStr = ConfigurationManager.ConnectionStrings["MDSSConnectionString"].ConnectionString;
            bllDoc = new BLLDocument(cStr);
            blDocBackup = new BLLDocumentBackup(cStr);
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer
            {
                Interval = 1000 //every 1 secs
            };
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Enabled = true;

            Log.WriteLog("Mongo service started....");

        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {

                string now = DateTime.Now.ToString("HH:mm:ss");
                string DocSyncTime = ConfigurationManager.AppSettings["DocSyncTime"];
                string BackupDumpTime = ConfigurationManager.AppSettings["BackupDumpTime"];
                string BackupPath = ConfigurationManager.AppSettings["BackupPath"];

                //Log.WriteLog(now);

                // check if current time is scheduled time for mongo data sync
                if (DocSyncTime == now)
                {
                    Log.WriteLog("Mongo DB document sync started...");
                    new Task(() =>
                    {

                        bllDoc.SyncDocuments();
                        Log.WriteLog("Mongo DB document sync completed...");
                    }).Start();
                }

                // check if current time is scheduled time for mongo data backup
                if (BackupDumpTime == now)
                {
                    new Task(() =>
                    {
                        Log.WriteLog("Mongo DB database backup started...");
                        Log.WriteLog($"Backup path: {BackupPath}");
                        blDocBackup.DumpDocumentBackup(BackupPath);
                        Log.WriteLog("Mongo DB database backup completed...");
                    }).Start();

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("Exception occured...");
                Log.WriteLog(ex.ToString());

            }
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            Log.WriteLog("Mongo service stopped.");
        }

    }
}
