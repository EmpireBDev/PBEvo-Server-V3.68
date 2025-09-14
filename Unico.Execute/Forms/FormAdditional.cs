using Executable.Utility;
using Plugin.Core.Utility;
using System;
using System.Windows.Forms;

namespace Executable.Forms
{
    public partial class FormAdditional : Form
    {
        public FormAdditional()
        {
            InitializeComponent();
        }
        private void FormAdditional_Load(object sender, EventArgs e)
        {
            float OpacityValue = float.Parse("10") / 100;
            BackgroundImage = ImageUtility.GetInstance().ChangeOpacity(Properties.Resources.PBLOGO_256, OpacityValue);
            BackgroundImageLayout = ImageLayout.Center;
            ServerVersion.Text = "Loading...";
            ServerRegion.Text = "Loading...";
            LocalAddress.Text = "Loading...";
            RunTimeline.Text = "Loading...";
            ConfigIndex.Text = "Loading...";
            RuleIndex.Text = "Loading...";
            InternetCafe.Text = "Loading...";
            AutoAccount.Text = "Loading...";
            AutoBan.Text = "Loading...";
            RefresherT.Start();
        }
        private void RefresherT_Tick(object sender, EventArgs e)
        {
            ServerVersion.Text = StringUtility.ForGameVersionL;
            ServerRegion.Text = StringUtility.ForGameRegionL;
            LocalAddress.Text = StringUtility.LocalAddressL;
            RunTimeline.Text = StringUtility.ServerTimelineL;
            ConfigIndex.Text = StringUtility.SelectedServerConfigL;
            RuleIndex.Text = StringUtility.SelectedGameRuleL;
            InternetCafe.Text = StringUtility.InternetCafeL;
            InternetCafe.ForeColor = (StringUtility.InternetCafeL.Equals("Enabled") ? ColorUtil.Green : StringUtility.InternetCafeL.Equals("Disabled") ? ColorUtil.Yellow : ColorUtil.Silver);
            AutoAccount.Text = StringUtility.EnableAutoAccountL;
            AutoAccount.ForeColor = (StringUtility.EnableAutoAccountL.Equals("Enabled") ? ColorUtil.Green : StringUtility.EnableAutoAccountL.Equals("Disabled") ? ColorUtil.Yellow : ColorUtil.Silver);
            AutoBan.Text = StringUtility.AutoBanPlayerL;
            AutoBan.ForeColor = (StringUtility.AutoBanPlayerL.Equals("Enabled") ? ColorUtil.Green : StringUtility.AutoBanPlayerL.Equals("Disabled") ? ColorUtil.Yellow : ColorUtil.Silver);
        }
    }
}
