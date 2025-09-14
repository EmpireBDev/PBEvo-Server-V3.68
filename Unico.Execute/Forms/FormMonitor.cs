using Executable.Utility;
using System;
using System.Windows.Forms;

namespace Executable.Forms
{
    public partial class FormMonitor : Form
    {
        public FormMonitor()
        {
            InitializeComponent();
        }
        private void FormMonitor_Load(object sender, EventArgs e)
        {
            float OpacityValue = float.Parse("10") / 100;
            BackgroundImage = ImageUtility.GetInstance().ChangeOpacity(Properties.Resources.PBLOGO_256, OpacityValue);
            BackgroundImageLayout = ImageLayout.Center;
            RegisteredUsers.Text = "Loading...";
            OnlinePlayers.Text = "Loading...";
            TotalClans.Text = "Loading...";
            VipUsers.Text = "Loading...";
            UnknownUsers.Text = "Loading...";
            TotalBannedPlayers.Text = "Loading...";
            RegisteredShopItems.Text = "Loading...";
            ShopCafeItems.Text = "Loading...";
            RepairableItems.Text = "Loading...";
            RefresherT.Start();
        }
        private void RefresherT_Tick(object sender, EventArgs e)
        {
            RegisteredUsers.Text = StringUtility.RegisteredUserL;
            OnlinePlayers.Text = StringUtility.OnlineUserL;
            TotalClans.Text = StringUtility.TotalClansL;
            VipUsers.Text = StringUtility.VipUserL;
            UnknownUsers.Text = StringUtility.UnknownUserL;
            TotalBannedPlayers.Text = StringUtility.BannedPlayers;
            RegisteredShopItems.Text = StringUtility.RegShopItems;
            ShopCafeItems.Text = StringUtility.ShopCafeItems;
            RepairableItems.Text = StringUtility.RepairableItems;
        }
    }
}
