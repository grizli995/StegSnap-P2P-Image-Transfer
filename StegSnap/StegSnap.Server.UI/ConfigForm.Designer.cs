namespace StegSnap.Server.UI
{
    partial class ConfigForm
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
            this.lbl_seconds = new System.Windows.Forms.Label();
            this.tbx_seconds = new System.Windows.Forms.TextBox();
            this.tbx_storage = new System.Windows.Forms.TextBox();
            this.lbl_snapshot_interval = new System.Windows.Forms.Label();
            this.lbl_configuration_storage_path = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_browse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_seconds
            // 
            this.lbl_seconds.AutoSize = true;
            this.lbl_seconds.Location = new System.Drawing.Point(205, 55);
            this.lbl_seconds.Name = "lbl_seconds";
            this.lbl_seconds.Size = new System.Drawing.Size(50, 15);
            this.lbl_seconds.TabIndex = 12;
            this.lbl_seconds.Text = "seconds";
            // 
            // tbx_seconds
            // 
            this.tbx_seconds.Location = new System.Drawing.Point(165, 47);
            this.tbx_seconds.Name = "tbx_seconds";
            this.tbx_seconds.Size = new System.Drawing.Size(34, 23);
            this.tbx_seconds.TabIndex = 11;
            // 
            // tbx_storage
            // 
            this.tbx_storage.Location = new System.Drawing.Point(119, 20);
            this.tbx_storage.Name = "tbx_storage";
            this.tbx_storage.Size = new System.Drawing.Size(288, 23);
            this.tbx_storage.TabIndex = 10;
            // 
            // lbl_snapshot_interval
            // 
            this.lbl_snapshot_interval.AutoSize = true;
            this.lbl_snapshot_interval.Location = new System.Drawing.Point(13, 55);
            this.lbl_snapshot_interval.Name = "lbl_snapshot_interval";
            this.lbl_snapshot_interval.Size = new System.Drawing.Size(146, 15);
            this.lbl_snapshot_interval.TabIndex = 9;
            this.lbl_snapshot_interval.Text = "Snapshot Request Interval:";
            // 
            // lbl_configuration_storage_path
            // 
            this.lbl_configuration_storage_path.AutoSize = true;
            this.lbl_configuration_storage_path.Location = new System.Drawing.Point(12, 28);
            this.lbl_configuration_storage_path.Name = "lbl_configuration_storage_path";
            this.lbl_configuration_storage_path.Size = new System.Drawing.Size(101, 15);
            this.lbl_configuration_storage_path.TabIndex = 8;
            this.lbl_configuration_storage_path.Text = "Storage Directory:";
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(326, 102);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 15;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // btn_ok
            // 
            this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_ok.Location = new System.Drawing.Point(407, 102);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 14;
            this.btn_ok.Text = "Submit";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_browse
            // 
            this.btn_browse.Location = new System.Drawing.Point(413, 20);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(75, 23);
            this.btn_browse.TabIndex = 13;
            this.btn_browse.Text = "Browse";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 128);
            this.Controls.Add(this.lbl_seconds);
            this.Controls.Add(this.tbx_seconds);
            this.Controls.Add(this.tbx_storage);
            this.Controls.Add(this.lbl_snapshot_interval);
            this.Controls.Add(this.lbl_configuration_storage_path);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_browse);
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lbl_seconds;
        private TextBox tbx_seconds;
        private TextBox tbx_storage;
        private Label lbl_snapshot_interval;
        private Label lbl_configuration_storage_path;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button btn_cancel;
        private Button btn_ok;
        private Button btn_browse;
    }
}