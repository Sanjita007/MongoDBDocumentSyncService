
namespace ServiceTest
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSyncDocuments = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.pgTask = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnSyncDocuments
            // 
            this.btnSyncDocuments.Font = new System.Drawing.Font("Microsoft YaHei Light", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncDocuments.Location = new System.Drawing.Point(77, 60);
            this.btnSyncDocuments.Name = "btnSyncDocuments";
            this.btnSyncDocuments.Size = new System.Drawing.Size(220, 31);
            this.btnSyncDocuments.TabIndex = 0;
            this.btnSyncDocuments.Text = "Sync Documents";
            this.btnSyncDocuments.UseVisualStyleBackColor = true;
            this.btnSyncDocuments.Click += new System.EventHandler(this.btnSyncDocuments_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Font = new System.Drawing.Font("Microsoft YaHei Light", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackup.Location = new System.Drawing.Point(77, 125);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(220, 31);
            this.btnBackup.TabIndex = 0;
            this.btnBackup.Text = "Backup DBs";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // pgTask
            // 
            this.pgTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pgTask.Location = new System.Drawing.Point(0, 233);
            this.pgTask.Name = "pgTask";
            this.pgTask.Size = new System.Drawing.Size(390, 23);
            this.pgTask.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgTask.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 256);
            this.Controls.Add(this.pgTask);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnSyncDocuments);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSyncDocuments;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.ProgressBar pgTask;
    }
}

