
namespace Executable.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RamPercent = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.MonitorName = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RefreshBTN = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MinimizeBTN = new System.Windows.Forms.Button();
            this.CloseBTN = new System.Windows.Forms.Button();
            this.AppTitle = new System.Windows.Forms.Label();
            this.MonitorBTN = new System.Windows.Forms.Button();
            this.AdditionalBTN = new System.Windows.Forms.Button();
            this.ConfigBTN = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label51 = new System.Windows.Forms.Label();
            this.LogFileSize = new System.Windows.Forms.Label();
            this.AppStatus = new System.Windows.Forms.Label();
            this.RefresherT = new System.Windows.Forms.Timer(this.components);
            this.FormLoaderPNL = new System.Windows.Forms.Panel();
            this.MonitorLogo = new System.Windows.Forms.PictureBox();
            this.ServerTT = new System.Windows.Forms.ToolTip(this.components);
            this.AdditionalPanelN = new System.Windows.Forms.Panel();
            this.NavPNL = new System.Windows.Forms.Panel();
            this.MonitorPanelN = new System.Windows.Forms.Panel();
            this.ConfigPanelN = new System.Windows.Forms.Panel();
            this.FMonitorPanel = new System.Windows.Forms.Panel();
            this.FAdditionalPanel = new System.Windows.Forms.Panel();
            this.FConfigPanel = new System.Windows.Forms.Panel();
            this.MemoryVPB = new Executable.CTRL.VerticalProgressBar();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MonitorLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // RamPercent
            // 
            this.RamPercent.BackColor = System.Drawing.Color.Transparent;
            this.RamPercent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RamPercent.Font = new System.Drawing.Font("Roboto Slab", 18F);
            this.RamPercent.ForeColor = System.Drawing.Color.White;
            this.RamPercent.Location = new System.Drawing.Point(33, 205);
            this.RamPercent.Name = "RamPercent";
            this.RamPercent.Size = new System.Drawing.Size(125, 31);
            this.RamPercent.TabIndex = 91;
            this.RamPercent.Text = "99.9%";
            this.RamPercent.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label47
            // 
            this.label47.BackColor = System.Drawing.Color.Transparent;
            this.label47.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label47.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label47.ForeColor = System.Drawing.Color.White;
            this.label47.Location = new System.Drawing.Point(33, 182);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(125, 23);
            this.label47.TabIndex = 92;
            this.label47.Text = "Memory Usage";
            // 
            // label48
            // 
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label48.Font = new System.Drawing.Font("Roboto Slab SemiBold", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label48.ForeColor = System.Drawing.Color.White;
            this.label48.Location = new System.Drawing.Point(12, 84);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(394, 29);
            this.label48.TabIndex = 93;
            this.label48.Text = "Please Monitor Your Server";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MonitorName
            // 
            this.MonitorName.BackColor = System.Drawing.Color.Transparent;
            this.MonitorName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MonitorName.Font = new System.Drawing.Font("Roboto Slab", 10F, System.Drawing.FontStyle.Bold);
            this.MonitorName.ForeColor = System.Drawing.Color.White;
            this.MonitorName.Location = new System.Drawing.Point(56, 12);
            this.MonitorName.Name = "MonitorName";
            this.MonitorName.Size = new System.Drawing.Size(149, 38);
            this.MonitorName.TabIndex = 95;
            this.MonitorName.Text = "OSM - Monitor";
            this.MonitorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.MemoryVPB);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Font = new System.Drawing.Font("Roboto Slab", 6F);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(16, 178);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(11, 58);
            this.groupBox3.TabIndex = 96;
            this.groupBox3.TabStop = false;
            // 
            // RefreshBTN
            // 
            this.RefreshBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.RefreshBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RefreshBTN.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.RefreshBTN.FlatAppearance.BorderSize = 0;
            this.RefreshBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
            this.RefreshBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.RefreshBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBTN.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.RefreshBTN.ForeColor = System.Drawing.Color.White;
            this.RefreshBTN.Location = new System.Drawing.Point(313, 182);
            this.RefreshBTN.Name = "RefreshBTN";
            this.RefreshBTN.Size = new System.Drawing.Size(93, 54);
            this.RefreshBTN.TabIndex = 1;
            this.RefreshBTN.Text = "Refresh";
            this.RefreshBTN.UseVisualStyleBackColor = false;
            this.RefreshBTN.Click += new System.EventHandler(this.RefreshBTN_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Roboto Slab", 6F);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(16, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 1);
            this.groupBox2.TabIndex = 97;
            this.groupBox2.TabStop = false;
            // 
            // MinimizeBTN
            // 
            this.MinimizeBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MinimizeBTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.MinimizeBTN.FlatAppearance.BorderSize = 0;
            this.MinimizeBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeBTN.Font = new System.Drawing.Font("Roboto Slab", 14F);
            this.MinimizeBTN.ForeColor = System.Drawing.Color.White;
            this.MinimizeBTN.Location = new System.Drawing.Point(328, 1);
            this.MinimizeBTN.Name = "MinimizeBTN";
            this.MinimizeBTN.Size = new System.Drawing.Size(38, 48);
            this.MinimizeBTN.TabIndex = 99;
            this.MinimizeBTN.Text = "_";
            this.MinimizeBTN.UseVisualStyleBackColor = true;
            this.MinimizeBTN.Click += new System.EventHandler(this.MinimizeBTN_Click);
            // 
            // CloseBTN
            // 
            this.CloseBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseBTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.CloseBTN.FlatAppearance.BorderSize = 0;
            this.CloseBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.CloseBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.CloseBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseBTN.Font = new System.Drawing.Font("Roboto Slab", 15F);
            this.CloseBTN.ForeColor = System.Drawing.Color.White;
            this.CloseBTN.Location = new System.Drawing.Point(369, 1);
            this.CloseBTN.Name = "CloseBTN";
            this.CloseBTN.Size = new System.Drawing.Size(52, 48);
            this.CloseBTN.TabIndex = 98;
            this.CloseBTN.Text = "X";
            this.CloseBTN.UseVisualStyleBackColor = true;
            this.CloseBTN.Click += new System.EventHandler(this.CloseBTN_Click);
            // 
            // AppTitle
            // 
            this.AppTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AppTitle.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.AppTitle.ForeColor = System.Drawing.Color.Silver;
            this.AppTitle.Location = new System.Drawing.Point(14, 113);
            this.AppTitle.Name = "AppTitle";
            this.AppTitle.Size = new System.Drawing.Size(392, 17);
            this.AppTitle.TabIndex = 85;
            this.AppTitle.Text = "Version 1.2.34567.890";
            this.AppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MonitorBTN
            // 
            this.MonitorBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.MonitorBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MonitorBTN.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.MonitorBTN.FlatAppearance.BorderSize = 0;
            this.MonitorBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
            this.MonitorBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.MonitorBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MonitorBTN.Font = new System.Drawing.Font("Roboto Slab", 9F);
            this.MonitorBTN.ForeColor = System.Drawing.Color.White;
            this.MonitorBTN.Location = new System.Drawing.Point(1, 561);
            this.MonitorBTN.Name = "MonitorBTN";
            this.MonitorBTN.Size = new System.Drawing.Size(146, 54);
            this.MonitorBTN.TabIndex = 106;
            this.MonitorBTN.Text = "Monitor";
            this.MonitorBTN.UseVisualStyleBackColor = false;
            this.MonitorBTN.Click += new System.EventHandler(this.MonitorBTN_Click);
            // 
            // AdditionalBTN
            // 
            this.AdditionalBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.AdditionalBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AdditionalBTN.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.AdditionalBTN.FlatAppearance.BorderSize = 0;
            this.AdditionalBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
            this.AdditionalBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.AdditionalBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AdditionalBTN.Font = new System.Drawing.Font("Roboto Slab", 9F);
            this.AdditionalBTN.ForeColor = System.Drawing.Color.White;
            this.AdditionalBTN.Location = new System.Drawing.Point(146, 561);
            this.AdditionalBTN.Name = "AdditionalBTN";
            this.AdditionalBTN.Size = new System.Drawing.Size(130, 54);
            this.AdditionalBTN.TabIndex = 107;
            this.AdditionalBTN.Text = "Additional";
            this.AdditionalBTN.UseVisualStyleBackColor = false;
            this.AdditionalBTN.Click += new System.EventHandler(this.AdditionalBTN_Click);
            // 
            // ConfigBTN
            // 
            this.ConfigBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.ConfigBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfigBTN.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.ConfigBTN.FlatAppearance.BorderSize = 0;
            this.ConfigBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
            this.ConfigBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.ConfigBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConfigBTN.Font = new System.Drawing.Font("Roboto Slab", 9F);
            this.ConfigBTN.ForeColor = System.Drawing.Color.White;
            this.ConfigBTN.Location = new System.Drawing.Point(275, 561);
            this.ConfigBTN.Name = "ConfigBTN";
            this.ConfigBTN.Size = new System.Drawing.Size(146, 54);
            this.ConfigBTN.TabIndex = 108;
            this.ConfigBTN.Text = "Config";
            this.ConfigBTN.UseVisualStyleBackColor = false;
            this.ConfigBTN.Click += new System.EventHandler(this.ConfigBTN_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox4.Font = new System.Drawing.Font("Roboto Slab", 6F);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(167, 178);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1, 58);
            this.groupBox4.TabIndex = 111;
            this.groupBox4.TabStop = false;
            // 
            // label51
            // 
            this.label51.BackColor = System.Drawing.Color.Transparent;
            this.label51.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label51.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label51.ForeColor = System.Drawing.Color.White;
            this.label51.Location = new System.Drawing.Point(179, 182);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(125, 23);
            this.label51.TabIndex = 110;
            this.label51.Text = "Log Files";
            // 
            // LogFileSize
            // 
            this.LogFileSize.BackColor = System.Drawing.Color.Transparent;
            this.LogFileSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogFileSize.Font = new System.Drawing.Font("Roboto Slab", 18F);
            this.LogFileSize.ForeColor = System.Drawing.Color.White;
            this.LogFileSize.Location = new System.Drawing.Point(179, 205);
            this.LogFileSize.Name = "LogFileSize";
            this.LogFileSize.Size = new System.Drawing.Size(125, 31);
            this.LogFileSize.TabIndex = 109;
            this.LogFileSize.Text = "100MB";
            this.LogFileSize.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // AppStatus
            // 
            this.AppStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AppStatus.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.AppStatus.ForeColor = System.Drawing.Color.Silver;
            this.AppStatus.Location = new System.Drawing.Point(14, 143);
            this.AppStatus.Name = "AppStatus";
            this.AppStatus.Size = new System.Drawing.Size(392, 17);
            this.AppStatus.TabIndex = 112;
            this.AppStatus.Text = "Server Status";
            this.AppStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RefresherT
            // 
            this.RefresherT.Interval = 1000;
            this.RefresherT.Tick += new System.EventHandler(this.RefresherT_Tick);
            // 
            // FormLoaderPNL
            // 
            this.FormLoaderPNL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.FormLoaderPNL.Location = new System.Drawing.Point(1, 242);
            this.FormLoaderPNL.Name = "FormLoaderPNL";
            this.FormLoaderPNL.Size = new System.Drawing.Size(420, 313);
            this.FormLoaderPNL.TabIndex = 120;
            // 
            // MonitorLogo
            // 
            this.MonitorLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MonitorLogo.Image = global::Executable.Properties.Resources.PBLOGO_256;
            this.MonitorLogo.Location = new System.Drawing.Point(12, 12);
            this.MonitorLogo.Name = "MonitorLogo";
            this.MonitorLogo.Size = new System.Drawing.Size(38, 38);
            this.MonitorLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MonitorLogo.TabIndex = 94;
            this.MonitorLogo.TabStop = false;
            // 
            // ServerTT
            // 
            this.ServerTT.ToolTipTitle = "Monitor Tips";
            // 
            // AdditionalPanelN
            // 
            this.AdditionalPanelN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.AdditionalPanelN.Location = new System.Drawing.Point(146, 555);
            this.AdditionalPanelN.Name = "AdditionalPanelN";
            this.AdditionalPanelN.Size = new System.Drawing.Size(130, 6);
            this.AdditionalPanelN.TabIndex = 128;
            // 
            // NavPNL
            // 
            this.NavPNL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.NavPNL.Location = new System.Drawing.Point(146, 612);
            this.NavPNL.Name = "NavPNL";
            this.NavPNL.Size = new System.Drawing.Size(130, 2);
            this.NavPNL.TabIndex = 129;
            // 
            // MonitorPanelN
            // 
            this.MonitorPanelN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.MonitorPanelN.Location = new System.Drawing.Point(1, 555);
            this.MonitorPanelN.Name = "MonitorPanelN";
            this.MonitorPanelN.Size = new System.Drawing.Size(146, 6);
            this.MonitorPanelN.TabIndex = 129;
            // 
            // ConfigPanelN
            // 
            this.ConfigPanelN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ConfigPanelN.Location = new System.Drawing.Point(275, 555);
            this.ConfigPanelN.Name = "ConfigPanelN";
            this.ConfigPanelN.Size = new System.Drawing.Size(146, 6);
            this.ConfigPanelN.TabIndex = 129;
            // 
            // FMonitorPanel
            // 
            this.FMonitorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.FMonitorPanel.Location = new System.Drawing.Point(1, 616);
            this.FMonitorPanel.Name = "FMonitorPanel";
            this.FMonitorPanel.Size = new System.Drawing.Size(146, 3);
            this.FMonitorPanel.TabIndex = 130;
            // 
            // FAdditionalPanel
            // 
            this.FAdditionalPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.FAdditionalPanel.Location = new System.Drawing.Point(146, 616);
            this.FAdditionalPanel.Name = "FAdditionalPanel";
            this.FAdditionalPanel.Size = new System.Drawing.Size(130, 3);
            this.FAdditionalPanel.TabIndex = 131;
            // 
            // FConfigPanel
            // 
            this.FConfigPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.FConfigPanel.Location = new System.Drawing.Point(275, 616);
            this.FConfigPanel.Name = "FConfigPanel";
            this.FConfigPanel.Size = new System.Drawing.Size(146, 3);
            this.FConfigPanel.TabIndex = 131;
            // 
            // MemoryVPB
            // 
            this.MemoryVPB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MemoryVPB.BorderStyle = Executable.CTRL.BorderStyles.None;
            this.MemoryVPB.Color = System.Drawing.Color.DarkGray;
            this.MemoryVPB.ForeColor = System.Drawing.Color.Gray;
            this.MemoryVPB.Location = new System.Drawing.Point(3, 8);
            this.MemoryVPB.Maximum = 2000;
            this.MemoryVPB.Minimum = 0;
            this.MemoryVPB.Name = "MemoryVPB";
            this.MemoryVPB.Size = new System.Drawing.Size(5, 46);
            this.MemoryVPB.Step = 100;
            this.MemoryVPB.Style = Executable.CTRL.Styles.Solid;
            this.MemoryVPB.TabIndex = 90;
            this.MemoryVPB.Value = 1000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(422, 620);
            this.Controls.Add(this.NavPNL);
            this.Controls.Add(this.ConfigPanelN);
            this.Controls.Add(this.AdditionalPanelN);
            this.Controls.Add(this.MonitorPanelN);
            this.Controls.Add(this.FConfigPanel);
            this.Controls.Add(this.FAdditionalPanel);
            this.Controls.Add(this.FMonitorPanel);
            this.Controls.Add(this.FormLoaderPNL);
            this.Controls.Add(this.AdditionalBTN);
            this.Controls.Add(this.AppStatus);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label51);
            this.Controls.Add(this.LogFileSize);
            this.Controls.Add(this.ConfigBTN);
            this.Controls.Add(this.MonitorBTN);
            this.Controls.Add(this.AppTitle);
            this.Controls.Add(this.MinimizeBTN);
            this.Controls.Add(this.CloseBTN);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.RefreshBTN);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.MonitorName);
            this.Controls.Add(this.MonitorLogo);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.label47);
            this.Controls.Add(this.RamPercent);
            this.Font = new System.Drawing.Font("Roboto Slab", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OSM-Monitor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Monitor1_FormClosing);
            this.Load += new System.EventHandler(this.Monitor1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Monitor1_Paint);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MonitorLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CTRL.VerticalProgressBar MemoryVPB;
        private System.Windows.Forms.Label RamPercent;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.PictureBox MonitorLogo;
        private System.Windows.Forms.Label MonitorName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button RefreshBTN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button MinimizeBTN;
        private System.Windows.Forms.Button CloseBTN;
        private System.Windows.Forms.Label AppTitle;
        private System.Windows.Forms.Button MonitorBTN;
        private System.Windows.Forms.Button AdditionalBTN;
        private System.Windows.Forms.Button ConfigBTN;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label LogFileSize;
        private System.Windows.Forms.Label AppStatus;
        private System.Windows.Forms.Timer RefresherT;
        private System.Windows.Forms.Panel FormLoaderPNL;
        private System.Windows.Forms.ToolTip ServerTT;
        private System.Windows.Forms.Panel AdditionalPanelN;
        private System.Windows.Forms.Panel NavPNL;
        private System.Windows.Forms.Panel MonitorPanelN;
        private System.Windows.Forms.Panel ConfigPanelN;
        private System.Windows.Forms.Panel FMonitorPanel;
        private System.Windows.Forms.Panel FAdditionalPanel;
        private System.Windows.Forms.Panel FConfigPanel;
    }
}