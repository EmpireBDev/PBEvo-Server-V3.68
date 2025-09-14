
namespace Executable.Forms
{
    partial class FormAdditional
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdditional));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RefresherT = new System.Windows.Forms.Timer(this.components);
            this.label25 = new System.Windows.Forms.Label();
            this.ServerVersion = new System.Windows.Forms.Label();
            this.LocalAddress = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label115 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label111 = new System.Windows.Forms.Label();
            this.AutoAccount = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.InternetCafe = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label119 = new System.Windows.Forms.Label();
            this.label114 = new System.Windows.Forms.Label();
            this.RuleIndex = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.AutoBan = new System.Windows.Forms.Label();
            this.RunTimeline = new System.Windows.Forms.Label();
            this.ConfigIndex = new System.Windows.Forms.Label();
            this.label117 = new System.Windows.Forms.Label();
            this.Label116 = new System.Windows.Forms.Label();
            this.label112 = new System.Windows.Forms.Label();
            this.ServerRegion = new System.Windows.Forms.Label();
            this.label118 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.ServerRegion);
            this.groupBox1.Controls.Add(this.label118);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.label112);
            this.groupBox1.Controls.Add(this.Label116);
            this.groupBox1.Controls.Add(this.label117);
            this.groupBox1.Controls.Add(this.ConfigIndex);
            this.groupBox1.Controls.Add(this.RunTimeline);
            this.groupBox1.Controls.Add(this.AutoBan);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.RuleIndex);
            this.groupBox1.Controls.Add(this.label114);
            this.groupBox1.Controls.Add(this.label119);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.InternetCafe);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.AutoAccount);
            this.groupBox1.Controls.Add(this.label111);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.label115);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label113);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.LocalAddress);
            this.groupBox1.Controls.Add(this.ServerVersion);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(15, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 294);
            this.groupBox1.TabIndex = 90;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Additional Information";
            // 
            // RefresherT
            // 
            this.RefresherT.Interval = 1000;
            this.RefresherT.Tick += new System.EventHandler(this.RefresherT_Tick);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label25.ForeColor = System.Drawing.Color.Silver;
            this.label25.Location = new System.Drawing.Point(202, 29);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(12, 19);
            this.label25.TabIndex = 63;
            this.label25.Text = ":";
            // 
            // VersionRegion
            // 
            this.ServerVersion.AutoSize = true;
            this.ServerVersion.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.ServerVersion.ForeColor = System.Drawing.Color.Silver;
            this.ServerVersion.Location = new System.Drawing.Point(218, 29);
            this.ServerVersion.Name = "VersionRegion";
            this.ServerVersion.Size = new System.Drawing.Size(38, 19);
            this.ServerVersion.TabIndex = 74;
            this.ServerVersion.Text = "V3.0";
            // 
            // LocalAddress
            // 
            this.LocalAddress.AutoSize = true;
            this.LocalAddress.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.LocalAddress.ForeColor = System.Drawing.Color.Silver;
            this.LocalAddress.Location = new System.Drawing.Point(218, 85);
            this.LocalAddress.Name = "LocalAddress";
            this.LocalAddress.Size = new System.Drawing.Size(50, 19);
            this.LocalAddress.TabIndex = 67;
            this.LocalAddress.Text = "0.0.0.0";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label43.ForeColor = System.Drawing.Color.Silver;
            this.label43.Location = new System.Drawing.Point(202, 253);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(12, 19);
            this.label43.TabIndex = 82;
            this.label43.Text = ":";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label17.ForeColor = System.Drawing.Color.Silver;
            this.label17.Location = new System.Drawing.Point(202, 85);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(12, 19);
            this.label17.TabIndex = 55;
            this.label17.Text = ":";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label113.ForeColor = System.Drawing.Color.Silver;
            this.label113.Location = new System.Drawing.Point(17, 113);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(100, 19);
            this.label113.TabIndex = 46;
            this.label113.Text = "Run Timeline";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label23.ForeColor = System.Drawing.Color.Silver;
            this.label23.Location = new System.Drawing.Point(202, 225);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(12, 19);
            this.label23.TabIndex = 61;
            this.label23.Text = ":";
            // 
            // label115
            // 
            this.label115.AutoSize = true;
            this.label115.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label115.ForeColor = System.Drawing.Color.Silver;
            this.label115.Location = new System.Drawing.Point(17, 169);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(80, 19);
            this.label115.TabIndex = 49;
            this.label115.Text = "Rule Index";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label44.ForeColor = System.Drawing.Color.Silver;
            this.label44.Location = new System.Drawing.Point(202, 197);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(12, 19);
            this.label44.TabIndex = 81;
            this.label44.Text = ":";
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label111.ForeColor = System.Drawing.Color.Silver;
            this.label111.Location = new System.Drawing.Point(17, 29);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(110, 19);
            this.label111.TabIndex = 50;
            this.label111.Text = "Server Version";
            // 
            // AutoAccount
            // 
            this.AutoAccount.AutoSize = true;
            this.AutoAccount.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.AutoAccount.ForeColor = System.Drawing.Color.Silver;
            this.AutoAccount.Location = new System.Drawing.Point(218, 225);
            this.AutoAccount.Name = "AutoAccount";
            this.AutoAccount.Size = new System.Drawing.Size(67, 19);
            this.AutoAccount.TabIndex = 76;
            this.AutoAccount.Text = "Disabled";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label26.ForeColor = System.Drawing.Color.Silver;
            this.label26.Location = new System.Drawing.Point(202, 169);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(12, 19);
            this.label26.TabIndex = 64;
            this.label26.Text = ":";
            // 
            // InternetCafe
            // 
            this.InternetCafe.AutoSize = true;
            this.InternetCafe.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.InternetCafe.ForeColor = System.Drawing.Color.Silver;
            this.InternetCafe.Location = new System.Drawing.Point(218, 197);
            this.InternetCafe.Name = "InternetCafe";
            this.InternetCafe.Size = new System.Drawing.Size(63, 19);
            this.InternetCafe.TabIndex = 83;
            this.InternetCafe.Text = "Enabled";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label20.ForeColor = System.Drawing.Color.Silver;
            this.label20.Location = new System.Drawing.Point(202, 113);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(12, 19);
            this.label20.TabIndex = 58;
            this.label20.Text = ":";
            // 
            // label119
            // 
            this.label119.AutoSize = true;
            this.label119.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label119.ForeColor = System.Drawing.Color.Silver;
            this.label119.Location = new System.Drawing.Point(17, 253);
            this.label119.Name = "label119";
            this.label119.Size = new System.Drawing.Size(123, 19);
            this.label119.TabIndex = 80;
            this.label119.Text = "Auto Ban System";
            // 
            // label114
            // 
            this.label114.AutoSize = true;
            this.label114.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label114.ForeColor = System.Drawing.Color.Silver;
            this.label114.Location = new System.Drawing.Point(17, 141);
            this.label114.Name = "label114";
            this.label114.Size = new System.Drawing.Size(94, 19);
            this.label114.TabIndex = 48;
            this.label114.Text = "Config Index";
            // 
            // RuleIndex
            // 
            this.RuleIndex.AutoSize = true;
            this.RuleIndex.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.RuleIndex.ForeColor = System.Drawing.Color.Silver;
            this.RuleIndex.Location = new System.Drawing.Point(218, 169);
            this.RuleIndex.Name = "RuleIndex";
            this.RuleIndex.Size = new System.Drawing.Size(15, 19);
            this.RuleIndex.TabIndex = 73;
            this.RuleIndex.Text = "1";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label22.ForeColor = System.Drawing.Color.Silver;
            this.label22.Location = new System.Drawing.Point(202, 141);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(12, 19);
            this.label22.TabIndex = 60;
            this.label22.Text = ":";
            // 
            // AutoBan
            // 
            this.AutoBan.AutoSize = true;
            this.AutoBan.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.AutoBan.ForeColor = System.Drawing.Color.Silver;
            this.AutoBan.Location = new System.Drawing.Point(218, 253);
            this.AutoBan.Name = "AutoBan";
            this.AutoBan.Size = new System.Drawing.Size(63, 19);
            this.AutoBan.TabIndex = 84;
            this.AutoBan.Text = "Enabled";
            // 
            // RunTimeline
            // 
            this.RunTimeline.AutoSize = true;
            this.RunTimeline.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.RunTimeline.ForeColor = System.Drawing.Color.Silver;
            this.RunTimeline.Location = new System.Drawing.Point(218, 113);
            this.RunTimeline.Name = "RunTimeline";
            this.RunTimeline.Size = new System.Drawing.Size(41, 19);
            this.RunTimeline.TabIndex = 70;
            this.RunTimeline.Text = "2000";
            // 
            // ConfigIndex
            // 
            this.ConfigIndex.AutoSize = true;
            this.ConfigIndex.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.ConfigIndex.ForeColor = System.Drawing.Color.Silver;
            this.ConfigIndex.Location = new System.Drawing.Point(218, 141);
            this.ConfigIndex.Name = "ConfigIndex";
            this.ConfigIndex.Size = new System.Drawing.Size(15, 19);
            this.ConfigIndex.TabIndex = 72;
            this.ConfigIndex.Text = "1";
            // 
            // label117
            // 
            this.label117.AutoSize = true;
            this.label117.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label117.ForeColor = System.Drawing.Color.Silver;
            this.label117.Location = new System.Drawing.Point(17, 225);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(101, 19);
            this.label117.TabIndex = 52;
            this.label117.Text = "Auto Account";
            // 
            // Label116
            // 
            this.Label116.AutoSize = true;
            this.Label116.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.Label116.ForeColor = System.Drawing.Color.Silver;
            this.Label116.Location = new System.Drawing.Point(17, 197);
            this.Label116.Name = "Label116";
            this.Label116.Size = new System.Drawing.Size(118, 19);
            this.Label116.TabIndex = 79;
            this.Label116.Text = "Internet PC Cafe";
            // 
            // label112
            // 
            this.label112.AutoSize = true;
            this.label112.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label112.ForeColor = System.Drawing.Color.Silver;
            this.label112.Location = new System.Drawing.Point(17, 85);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(102, 19);
            this.label112.TabIndex = 43;
            this.label112.Text = "Local Address";
            // 
            // AntiPEF
            // 
            this.ServerRegion.AutoSize = true;
            this.ServerRegion.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.ServerRegion.ForeColor = System.Drawing.Color.Silver;
            this.ServerRegion.Location = new System.Drawing.Point(218, 57);
            this.ServerRegion.Name = "AntiPEF";
            this.ServerRegion.Size = new System.Drawing.Size(62, 19);
            this.ServerRegion.TabIndex = 87;
            this.ServerRegion.Text = "Nations";
            // 
            // label118
            // 
            this.label118.AutoSize = true;
            this.label118.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label118.ForeColor = System.Drawing.Color.Silver;
            this.label118.Location = new System.Drawing.Point(17, 57);
            this.label118.Name = "label118";
            this.label118.Size = new System.Drawing.Size(104, 19);
            this.label118.TabIndex = 85;
            this.label118.Text = "Server Region";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Roboto Slab", 10F);
            this.label28.ForeColor = System.Drawing.Color.Silver;
            this.label28.Location = new System.Drawing.Point(202, 57);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(12, 19);
            this.label28.TabIndex = 86;
            this.label28.Text = ":";
            // 
            // FormAdditional
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(420, 313);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Roboto Slab", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAdditional";
            this.Text = "FormAdditional";
            this.Load += new System.EventHandler(this.FormAdditional_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer RefresherT;
        private System.Windows.Forms.Label ServerRegion;
        private System.Windows.Forms.Label label118;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.Label Label116;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.Label ConfigIndex;
        private System.Windows.Forms.Label RunTimeline;
        private System.Windows.Forms.Label AutoBan;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label RuleIndex;
        private System.Windows.Forms.Label label114;
        private System.Windows.Forms.Label label119;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label InternetCafe;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label AutoAccount;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label115;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label LocalAddress;
        private System.Windows.Forms.Label ServerVersion;
        private System.Windows.Forms.Label label25;
    }
}