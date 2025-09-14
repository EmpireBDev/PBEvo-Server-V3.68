
namespace Executable.Forms
{
    partial class FormConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            this.HighRB = new System.Windows.Forms.RadioButton();
            this.DefaultRB = new System.Windows.Forms.RadioButton();
            this.ClearLogBTN = new System.Windows.Forms.Button();
            this.ChangeLogBTN = new System.Windows.Forms.Button();
            this.Reload1BTN = new System.Windows.Forms.Button();
            this.Reload3BTN = new System.Windows.Forms.Button();
            this.Reload5BTN = new System.Windows.Forms.Button();
            this.Reload4BTN = new System.Windows.Forms.Button();
            this.Reload2BTN = new System.Windows.Forms.Button();
            this.OpenConfigBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HighRB
            // 
            this.HighRB.AutoCheck = false;
            this.HighRB.AutoSize = true;
            this.HighRB.ForeColor = System.Drawing.Color.Silver;
            this.HighRB.Location = new System.Drawing.Point(105, 41);
            this.HighRB.Name = "HighRB";
            this.HighRB.Size = new System.Drawing.Size(71, 19);
            this.HighRB.TabIndex = 136;
            this.HighRB.Text = "High Log";
            this.HighRB.UseVisualStyleBackColor = true;
            // 
            // DefaultRB
            // 
            this.DefaultRB.AutoCheck = false;
            this.DefaultRB.AutoSize = true;
            this.DefaultRB.Checked = true;
            this.DefaultRB.Cursor = System.Windows.Forms.Cursors.Default;
            this.DefaultRB.ForeColor = System.Drawing.Color.Silver;
            this.DefaultRB.Location = new System.Drawing.Point(15, 41);
            this.DefaultRB.Name = "DefaultRB";
            this.DefaultRB.Size = new System.Drawing.Size(84, 19);
            this.DefaultRB.TabIndex = 135;
            this.DefaultRB.TabStop = true;
            this.DefaultRB.Text = "Default Log";
            this.DefaultRB.UseVisualStyleBackColor = true;
            // 
            // ClearLogBTN
            // 
            this.ClearLogBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClearLogBTN.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.ClearLogBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearLogBTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.ClearLogBTN.ForeColor = System.Drawing.Color.Gold;
            this.ClearLogBTN.Location = new System.Drawing.Point(15, 160);
            this.ClearLogBTN.Name = "ClearLogBTN";
            this.ClearLogBTN.Size = new System.Drawing.Size(161, 69);
            this.ClearLogBTN.TabIndex = 134;
            this.ClearLogBTN.Text = "Clear Log Files";
            this.ClearLogBTN.UseVisualStyleBackColor = true;
            this.ClearLogBTN.Click += new System.EventHandler(this.ClearLogBTN_Click);
            // 
            // ChangeLogBTN
            // 
            this.ChangeLogBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChangeLogBTN.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.ChangeLogBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeLogBTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.ChangeLogBTN.ForeColor = System.Drawing.Color.Gold;
            this.ChangeLogBTN.Location = new System.Drawing.Point(15, 74);
            this.ChangeLogBTN.Name = "ChangeLogBTN";
            this.ChangeLogBTN.Size = new System.Drawing.Size(161, 70);
            this.ChangeLogBTN.TabIndex = 133;
            this.ChangeLogBTN.Text = "Change Log Mode";
            this.ChangeLogBTN.UseVisualStyleBackColor = true;
            this.ChangeLogBTN.Click += new System.EventHandler(this.ChangeLogBTN_Click);
            // 
            // Reload1BTN
            // 
            this.Reload1BTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Reload1BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reload1BTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.Reload1BTN.ForeColor = System.Drawing.Color.Silver;
            this.Reload1BTN.Location = new System.Drawing.Point(182, 74);
            this.Reload1BTN.Name = "Reload1BTN";
            this.Reload1BTN.Size = new System.Drawing.Size(223, 26);
            this.Reload1BTN.TabIndex = 128;
            this.Reload1BTN.Text = "Reload Config";
            this.Reload1BTN.UseVisualStyleBackColor = true;
            // 
            // Reload3BTN
            // 
            this.Reload3BTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Reload3BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reload3BTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.Reload3BTN.ForeColor = System.Drawing.Color.Silver;
            this.Reload3BTN.Location = new System.Drawing.Point(182, 117);
            this.Reload3BTN.Name = "Reload3BTN";
            this.Reload3BTN.Size = new System.Drawing.Size(223, 26);
            this.Reload3BTN.TabIndex = 130;
            this.Reload3BTN.Text = "Reload Events";
            this.Reload3BTN.UseVisualStyleBackColor = true;
            // 
            // Reload5BTN
            // 
            this.Reload5BTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Reload5BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reload5BTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.Reload5BTN.ForeColor = System.Drawing.Color.Silver;
            this.Reload5BTN.Location = new System.Drawing.Point(15, 246);
            this.Reload5BTN.Name = "Reload5BTN";
            this.Reload5BTN.Size = new System.Drawing.Size(358, 26);
            this.Reload5BTN.TabIndex = 132;
            this.Reload5BTN.Text = "Reload Attachments";
            this.Reload5BTN.UseVisualStyleBackColor = true;
            // 
            // Reload4BTN
            // 
            this.Reload4BTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Reload4BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reload4BTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.Reload4BTN.ForeColor = System.Drawing.Color.Silver;
            this.Reload4BTN.Location = new System.Drawing.Point(182, 203);
            this.Reload4BTN.Name = "Reload4BTN";
            this.Reload4BTN.Size = new System.Drawing.Size(223, 26);
            this.Reload4BTN.TabIndex = 131;
            this.Reload4BTN.Text = "Reload Item Rules";
            this.Reload4BTN.UseVisualStyleBackColor = true;
            // 
            // Reload2BTN
            // 
            this.Reload2BTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Reload2BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reload2BTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.Reload2BTN.ForeColor = System.Drawing.Color.Silver;
            this.Reload2BTN.Location = new System.Drawing.Point(182, 160);
            this.Reload2BTN.Name = "Reload2BTN";
            this.Reload2BTN.Size = new System.Drawing.Size(223, 26);
            this.Reload2BTN.TabIndex = 129;
            this.Reload2BTN.Text = "Reload Shop";
            this.Reload2BTN.UseVisualStyleBackColor = true;
            // 
            // OpenConfigBTN
            // 
            this.OpenConfigBTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OpenConfigBTN.BackgroundImage")));
            this.OpenConfigBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OpenConfigBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenConfigBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenConfigBTN.Font = new System.Drawing.Font("Roboto Slab", 8F);
            this.OpenConfigBTN.ForeColor = System.Drawing.Color.Silver;
            this.OpenConfigBTN.Location = new System.Drawing.Point(379, 246);
            this.OpenConfigBTN.Name = "OpenConfigBTN";
            this.OpenConfigBTN.Size = new System.Drawing.Size(26, 26);
            this.OpenConfigBTN.TabIndex = 137;
            this.OpenConfigBTN.UseVisualStyleBackColor = true;
            this.OpenConfigBTN.Click += new System.EventHandler(this.OpenConfigBTN_Click);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(420, 313);
            this.Controls.Add(this.OpenConfigBTN);
            this.Controls.Add(this.HighRB);
            this.Controls.Add(this.DefaultRB);
            this.Controls.Add(this.ClearLogBTN);
            this.Controls.Add(this.ChangeLogBTN);
            this.Controls.Add(this.Reload1BTN);
            this.Controls.Add(this.Reload3BTN);
            this.Controls.Add(this.Reload5BTN);
            this.Controls.Add(this.Reload4BTN);
            this.Controls.Add(this.Reload2BTN);
            this.Font = new System.Drawing.Font("Roboto Slab", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConfig";
            this.Text = "FormMonitor";
            this.Load += new System.EventHandler(this.FormConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenConfigBTN;
        private System.Windows.Forms.RadioButton HighRB;
        private System.Windows.Forms.RadioButton DefaultRB;
        private System.Windows.Forms.Button ClearLogBTN;
        private System.Windows.Forms.Button ChangeLogBTN;
        private System.Windows.Forms.Button Reload1BTN;
        private System.Windows.Forms.Button Reload3BTN;
        private System.Windows.Forms.Button Reload5BTN;
        private System.Windows.Forms.Button Reload4BTN;
        private System.Windows.Forms.Button Reload2BTN;
    }
}