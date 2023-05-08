namespace StegSnap.Server.UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pb_snapshot_image = new System.Windows.Forms.PictureBox();
            this.tb_snapshot_message = new System.Windows.Forms.TextBox();
            this.lbl_snapshot_message = new System.Windows.Forms.Label();
            this.lbl_client_id = new System.Windows.Forms.Label();
            this.tb_client_id = new System.Windows.Forms.TextBox();
            this.gb_snapshot = new System.Windows.Forms.GroupBox();
            this.tb_error = new System.Windows.Forms.TextBox();
            this.lbl_error = new System.Windows.Forms.Label();
            this.rtb_console = new System.Windows.Forms.RichTextBox();
            this.lbl_console = new System.Windows.Forms.Label();
            this.lbl_server_ip = new System.Windows.Forms.Label();
            this.tb_server_ip = new System.Windows.Forms.TextBox();
            this.lbl_clients = new System.Windows.Forms.Label();
            this.lb_clients = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pb_snapshot_image)).BeginInit();
            this.gb_snapshot.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb_snapshot_image
            // 
            this.pb_snapshot_image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_snapshot_image.Location = new System.Drawing.Point(4, 20);
            this.pb_snapshot_image.Name = "pb_snapshot_image";
            this.pb_snapshot_image.Size = new System.Drawing.Size(640, 480);
            this.pb_snapshot_image.TabIndex = 0;
            this.pb_snapshot_image.TabStop = false;
            this.pb_snapshot_image.Click += new System.EventHandler(this.pb_snapshot_image_Click);
            // 
            // tb_snapshot_message
            // 
            this.tb_snapshot_message.Location = new System.Drawing.Point(118, 506);
            this.tb_snapshot_message.Name = "tb_snapshot_message";
            this.tb_snapshot_message.Size = new System.Drawing.Size(529, 23);
            this.tb_snapshot_message.TabIndex = 1;
            // 
            // lbl_snapshot_message
            // 
            this.lbl_snapshot_message.AutoSize = true;
            this.lbl_snapshot_message.Location = new System.Drawing.Point(6, 514);
            this.lbl_snapshot_message.Name = "lbl_snapshot_message";
            this.lbl_snapshot_message.Size = new System.Drawing.Size(108, 15);
            this.lbl_snapshot_message.TabIndex = 3;
            this.lbl_snapshot_message.Text = "Extracted message:";
            // 
            // lbl_client_id
            // 
            this.lbl_client_id.AutoSize = true;
            this.lbl_client_id.Location = new System.Drawing.Point(6, 543);
            this.lbl_client_id.Name = "lbl_client_id";
            this.lbl_client_id.Size = new System.Drawing.Size(54, 15);
            this.lbl_client_id.TabIndex = 4;
            this.lbl_client_id.Text = "Client Id:";
            // 
            // tb_client_id
            // 
            this.tb_client_id.Location = new System.Drawing.Point(118, 535);
            this.tb_client_id.Name = "tb_client_id";
            this.tb_client_id.Size = new System.Drawing.Size(529, 23);
            this.tb_client_id.TabIndex = 5;
            // 
            // gb_snapshot
            // 
            this.gb_snapshot.Controls.Add(this.tb_error);
            this.gb_snapshot.Controls.Add(this.lbl_error);
            this.gb_snapshot.Controls.Add(this.pb_snapshot_image);
            this.gb_snapshot.Controls.Add(this.lbl_client_id);
            this.gb_snapshot.Controls.Add(this.tb_client_id);
            this.gb_snapshot.Controls.Add(this.tb_snapshot_message);
            this.gb_snapshot.Controls.Add(this.lbl_snapshot_message);
            this.gb_snapshot.Location = new System.Drawing.Point(339, 9);
            this.gb_snapshot.Name = "gb_snapshot";
            this.gb_snapshot.Size = new System.Drawing.Size(658, 646);
            this.gb_snapshot.TabIndex = 6;
            this.gb_snapshot.TabStop = false;
            this.gb_snapshot.Text = "Latest Snapshot";
            // 
            // tb_error
            // 
            this.tb_error.Location = new System.Drawing.Point(118, 564);
            this.tb_error.Multiline = true;
            this.tb_error.Name = "tb_error";
            this.tb_error.Size = new System.Drawing.Size(529, 50);
            this.tb_error.TabIndex = 7;
            // 
            // lbl_error
            // 
            this.lbl_error.AutoSize = true;
            this.lbl_error.Location = new System.Drawing.Point(6, 567);
            this.lbl_error.Name = "lbl_error";
            this.lbl_error.Size = new System.Drawing.Size(84, 15);
            this.lbl_error.TabIndex = 6;
            this.lbl_error.Text = "Error Message:";
            this.lbl_error.Click += new System.EventHandler(this.lbl_error_Click);
            // 
            // rtb_console
            // 
            this.rtb_console.BackColor = System.Drawing.SystemColors.WindowText;
            this.rtb_console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtb_console.ForeColor = System.Drawing.Color.White;
            this.rtb_console.Location = new System.Drawing.Point(12, 314);
            this.rtb_console.Name = "rtb_console";
            this.rtb_console.ReadOnly = true;
            this.rtb_console.Size = new System.Drawing.Size(321, 341);
            this.rtb_console.TabIndex = 7;
            this.rtb_console.Text = "";
            this.rtb_console.TextChanged += new System.EventHandler(this.rtb_console_TextChanged);
            // 
            // lbl_console
            // 
            this.lbl_console.AutoSize = true;
            this.lbl_console.Location = new System.Drawing.Point(12, 296);
            this.lbl_console.Name = "lbl_console";
            this.lbl_console.Size = new System.Drawing.Size(88, 15);
            this.lbl_console.TabIndex = 8;
            this.lbl_console.Text = "Server Console:";
            // 
            // lbl_server_ip
            // 
            this.lbl_server_ip.AutoSize = true;
            this.lbl_server_ip.Location = new System.Drawing.Point(12, 32);
            this.lbl_server_ip.Name = "lbl_server_ip";
            this.lbl_server_ip.Size = new System.Drawing.Size(98, 15);
            this.lbl_server_ip.TabIndex = 9;
            this.lbl_server_ip.Text = "Server IP address:";
            // 
            // tb_server_ip
            // 
            this.tb_server_ip.Location = new System.Drawing.Point(116, 29);
            this.tb_server_ip.Name = "tb_server_ip";
            this.tb_server_ip.Size = new System.Drawing.Size(217, 23);
            this.tb_server_ip.TabIndex = 10;
            // 
            // lbl_clients
            // 
            this.lbl_clients.AutoSize = true;
            this.lbl_clients.Location = new System.Drawing.Point(12, 55);
            this.lbl_clients.Name = "lbl_clients";
            this.lbl_clients.Size = new System.Drawing.Size(46, 15);
            this.lbl_clients.TabIndex = 12;
            this.lbl_clients.Text = "Clients:";
            // 
            // lb_clients
            // 
            this.lb_clients.FormattingEnabled = true;
            this.lb_clients.ItemHeight = 15;
            this.lb_clients.Location = new System.Drawing.Point(12, 73);
            this.lb_clients.Name = "lb_clients";
            this.lb_clients.Size = new System.Drawing.Size(321, 214);
            this.lb_clients.TabIndex = 13;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1007, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.configureToolStripMenuItem.Text = "Configuration";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 660);
            this.Controls.Add(this.lb_clients);
            this.Controls.Add(this.lbl_clients);
            this.Controls.Add(this.tb_server_ip);
            this.Controls.Add(this.lbl_server_ip);
            this.Controls.Add(this.lbl_console);
            this.Controls.Add(this.rtb_console);
            this.Controls.Add(this.gb_snapshot);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "StegSnap";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_snapshot_image)).EndInit();
            this.gb_snapshot.ResumeLayout(false);
            this.gb_snapshot.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pb_snapshot_image;
        private TextBox tb_snapshot_message;
        private Label lbl_snapshot_message;
        private Label lbl_client_id;
        private TextBox tb_client_id;
        private GroupBox gb_snapshot;
        private RichTextBox rtb_console;
        private Label lbl_console;
        private Label lbl_server_ip;
        private TextBox tb_server_ip;
        private Label lbl_clients;
        private ListBox lb_clients;
        private TextBox tb_error;
        private Label lbl_error;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem configureToolStripMenuItem;
    }
}