using Executable.Utility;
using Plugin.Core;
using Plugin.Core.Settings;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Executable.Forms
{
    public partial class FormConfig : Form
    {
        private readonly DirectoryInfo ServerDir;
        public FormConfig(DirectoryInfo ServerDir)
        {
            InitializeComponent();
            this.ServerDir = ServerDir;
        }
        #region Attachments
        private void MoreInfoStatus(bool High, bool Default)
        {
            HighRB.Checked = High;
            DefaultRB.Checked = Default;
        }
        #endregion Attachments
        private void FormConfig_Load(object sender, EventArgs e)
        {
            float OpacityValue = float.Parse("10") / 100;
            BackgroundImage = ImageUtility.GetInstance().ChangeOpacity(Properties.Resources.PBLOGO_256, OpacityValue);
            BackgroundImageLayout = ImageLayout.Center;
            if (ConfigLoader.ShowMoreInfo)
            {
                MoreInfoStatus(true, false);
            }
            else
            {
                MoreInfoStatus(false, true);
            }
        }
        private void OpenConfigBTN_Click(object sender, EventArgs e)
        {
            new Thread(() => MemoryUtility.ShellMode(@"Config/Settings.ini", "notepad.exe", "open")).Start();
        }
        private void ChangeLogBTN_Click(object sender, EventArgs e)
        {
            ConfigEngine CFG = new ConfigEngine("Config/Settings.ini");
            if (!ConfigLoader.ShowMoreInfo)
            {
                ConfigLoader.ShowMoreInfo = true;
                MoreInfoStatus(true, false);
            }
            else
            {
                ConfigLoader.ShowMoreInfo = false;
                MoreInfoStatus(false, true);
            }
            new Thread(() =>
            {
                string Key = "MoreInfo", Section = "Server";
                if (!CFG.KeyExists(Key, Section))
                {
                    MessageBox.Show($"Key: '{Key}' on Section '{Section}' doesn't exist!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                CFG.WriteX(Key, ConfigLoader.ShowMoreInfo, Section);
            }).Start();
            MessageBox.Show($"Logs mode changed to {(ConfigLoader.ShowMoreInfo ? "High." : "Default.")}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ClearLogBTN_Click(object sender, EventArgs e)
        {
            new Thread(() => MemoryUtility.ClearCache(ServerDir)).Start();
            MessageBox.Show("Logs has been cleared!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
