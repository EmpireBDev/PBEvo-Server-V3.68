using Plugin.Core.Enums;

namespace Plugin.Core.Models
{
    public class ServerConfig
    {
        public int ConfigId;
        public int ChannelAnnounceColor;
        public int ChatAnnounceColor;
        public bool OnlyGM;
        public bool AccessUFL;
        public bool Missions;
        public bool GiftSystem;
        public bool EnableClan;
        public bool EnableTicket;
        public bool EnableTags;
        public bool EnableBlood;
        public bool OfficialBannerEnabled;
        public string UserFileList;
        public string ClientVersion;
        public string ExitURL;
        public string ShopURL;
        public string OfficialSteam;
        public string OfficialBanner;
        public string OfficialAddress;
        public string ChannelAnnouce;
        public string ChatAnnounce;
        public ShowroomView Showroom;
        public ServerConfig()
        {
            UserFileList = "";
            ClientVersion = "";
            ExitURL = "";
            ShopURL = "";
            OfficialSteam = "";
            OfficialBanner = "";
            OfficialAddress = "";
            ChannelAnnouce = "";
            ChatAnnounce = "";
            Showroom = ShowroomView.S_Default;
        }
    }
}
