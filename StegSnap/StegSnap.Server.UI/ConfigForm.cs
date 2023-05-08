using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegSnap.Server.UI
{
    public partial class ConfigForm : Form
    {
        public int? SnapshotRequestIntervalSeconds { get; set; }
        public string StoragePath { get; set; }
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbx_seconds.Text))
            {
                SnapshotRequestIntervalSeconds = Int32.Parse(tbx_seconds.Text);
            }
            if (!String.IsNullOrEmpty(tbx_storage.Text))
            {
                StoragePath = tbx_storage.Text;
            }
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select server storage directory";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbx_storage.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
