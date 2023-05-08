using Microsoft.VisualBasic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace StegSnap.Server.UI
{
    public partial class MainForm : Form
    {
        public List<Guid> ConnectedClientIds= new List<Guid>();
        private ServerApp ServerApp;
        public MainForm()
        {
            InitializeComponent();
            GetIPAddress();

            ServerApp = new ServerApp((clientId, image, extractedMessage, errorMessage) =>
            {
                Invoke(new Action(() =>
                {
                    // Update the ListBox with the client ID
                    if (!ConnectedClientIds.Contains(clientId))
                    {
                        ConnectedClientIds.Add(clientId);
                        lb_clients.Items.Add(clientId);
                    }

                    tb_client_id.Text = clientId.ToString();

                    // Update the PictureBox with the received image
                    pb_snapshot_image.Image = image;

                    // Update the TextBox with the image path
                    tb_snapshot_message.Text = extractedMessage;

                    tb_error.Text = errorMessage;
                }));
            },
            (log) =>
            {
                Invoke(new Action(() =>
                {
                    AppendTextToRichTextBox(rtb_console, log);
                }));
            });
        }

        public void AppendTextToRichTextBox(RichTextBox richTextBox, string text)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action<RichTextBox, string>(AppendTextToRichTextBox), richTextBox, text);
            }
            else
            {
                richTextBox.AppendText(text + Environment.NewLine);
                richTextBox.ScrollToCaret();
            }
        }

        public void GetIPAddress()
        {
            string hostName = Dns.GetHostName(); 
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
            foreach (IPAddress ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork) // IPv4 address
                {
                    //Console.WriteLine("IPv4 Address: " + ipAddress);
                    tb_server_ip.Text = ipAddress.ToString();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rtb_console_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfigurationForm();
        }

        private void ShowConfigurationForm()
        {
            using (var configForm = new ConfigForm())
            {
                if (configForm.ShowDialog(this) == DialogResult.OK)
                {
                    if(!String.IsNullOrEmpty(configForm.StoragePath))
                    {
                        ServerApp.StoragePath = configForm.StoragePath;
                    }

                    if (configForm.SnapshotRequestIntervalSeconds.HasValue)
                    {
                        ServerApp.SnapshotRequestIntervalSeconds = configForm.SnapshotRequestIntervalSeconds;
                        ServerApp.UpdateInterval();
                    }
                }
            }
        }
    }
}