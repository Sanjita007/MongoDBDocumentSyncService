using BusinessLogicLayer.BusinessLogic;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            pgTask.MarqueeAnimationSpeed = 0;
            CheckForIllegalCrossThreadCalls = false;
        }

        void StartProgress()
        {
            pgTask.MarqueeAnimationSpeed = 100;
        }

        void StopProgress()
        {
            pgTask.MarqueeAnimationSpeed = 0;
        }

        void EnableControls(bool enable)
        {
            btnBackup.Enabled = btnSyncDocuments.Enabled = enable;
        }

        private void btnSyncDocuments_Click(object sender, EventArgs e)
        {
            string cStr = ConfigurationManager.ConnectionStrings["MDSSConnectionString"].ConnectionString;

            EnableControls(false);
            StartProgress();

            BLLDocument bll = new BLLDocument(cStr);
            new Task(delegate
            {
                bll.SyncDocuments();
                EnableControls(true);
                StopProgress();
                MessageBox.Show("Documents Synced Successfully!");
            }).Start();

        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            StartProgress();

            string cStr = ConfigurationManager.ConnectionStrings["MDSSConnectionString"].ConnectionString;

            BLLDocumentBackup bll = new BLLDocumentBackup(cStr);

            new Task(delegate
            {
                bll.DumpDocumentBackup("C:D/MONGO/");
                EnableControls(true);
                StopProgress();
                MessageBox.Show("Backup completed Successfully!");

            }).Start();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
