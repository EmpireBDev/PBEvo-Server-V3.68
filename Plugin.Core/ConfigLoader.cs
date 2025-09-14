using System;
using System.Collections.Generic;
using System.IO;
using Plugin.Core.Enums;
using Plugin.Core.Settings;

namespace Plugin.Core
{
    public static class ConfigLoader
    {
        public static int[] DEFAULT_PORT = new int[] { 39190, 39191, 40009 };
        public static string DatabaseName;
        public static string DatabaseHost;
        public static string DatabaseUsername;
        public static string DatabasePassword;
        public static string Ip4Address;
        public static string MatchPoint;
        public static string UdpVersion;
        public static bool IsTestMode;
        public static bool ShowMoreInfo;
        public static bool AutoAccount;
        public static bool DebugMode;
        public static bool WinCashPerBattle;
        public static bool ShowCashReceiveWarn;
        public static bool AutoBan;
        public static bool SendInfoToServ;
        public static bool SendFailMsg;
        public static bool EnableLog;
        public static bool UseMaxAmmoInDrop;
        public static bool UseHitMarker;
        public static bool ICafeSystem;
        public static bool IsDebugPing;
        public static bool IsLockedTime;
        public static bool AntiScript;
        public static bool LimitedPing;
        public static bool SendPingSync;
        public static bool AESProxy;
        public static int DatabasePort;
        public static int ConfigId;
        public static int MaxNickSize;
        public static int MinNickSize;
        public static int MinUserSize;
        public static int MinPassSize;
        public static int BackLog;
        public static int RuleId;
        public static int MaxLatency;
        public static int MaxRepeatLatency;
        public static int MaxActiveClans;
        public static int MinRankVote;
        public static int MaxExpReward;
        public static int MaxGoldReward;
        public static int MaxCashReward;
        public static int MinCreateGold;
        public static int MinCreateRank;
        public static int InternetCafeId;
        public static int LockYear;
        public static int BackYear;
        public static float MaxClanPoints;
        public static float PlantDuration;
        public static float DefuseDuration;
        public static ushort MaxDropWpnCount;
        public static double PingUpdateTime;
        public static UdpState UdpType;
        public static NationsEnum National;
        public static List<ClientLocale> GameLocales;
        public static void Load()
        {
            ConfigEngine CFG = new ConfigEngine("Config/Settings.ini", FileAccess.Read);
            /** Database Section **/
            DatabaseHost = CFG.ReadS("Host", "localhost", "Database");
            DatabaseName = CFG.ReadS("Name", "", "Database");
            DatabaseUsername = CFG.ReadS("User", "root", "Database");
            DatabasePassword = CFG.ReadS("Pass", "", "Database");
            DatabasePort = CFG.ReadD("Port", 0, "Database");
            /** Server Section **/
            ConfigId = CFG.ReadD("ConfigId", 1, "Server");
            RuleId = CFG.ReadD("RuleId", 1, "Server");
            BackLog = CFG.ReadD("BackLog", 3, "Server");
            DebugMode = CFG.ReadX("Debug", false, "Server");
            IsTestMode = CFG.ReadX("Test", false, "Server");
            ShowMoreInfo = CFG.ReadX("MoreInfo", false, "Server");
            IsDebugPing = CFG.ReadX("DebugPing", false, "Server");
            LimitedPing = CFG.ReadX("LimitedPing", false, "Server");
            Ip4Address = CFG.ReadS("Ip4Address", "127.0.0.1", "Server");
            MatchPoint = CFG.ReadS("MatchPoint", "127.0.0.1", "Server");
            AutoBan = CFG.ReadX("AutoBan", false, "Server");
            ICafeSystem = CFG.ReadX("ICafeSystem", true, "Server");
            InternetCafeId = CFG.ReadD("InternetCafeId", 1, "Server");
            /** Essentials Section **/
            AutoAccount = CFG.ReadX("AutoAccount", false, "Essentials");
            MinUserSize = CFG.ReadD("MinUserSize", 4, "Essentials");
            MinPassSize = CFG.ReadD("MinPassSize", 4, "Essentials");
            MinNickSize = CFG.ReadD("MinNickSize", 4, "Essentials");
            MaxNickSize = CFG.ReadD("MaxNickSize", 16, "Essentials");
            /** Internal Section **/
            MinRankVote = CFG.ReadD("MinRankVote", 0, "Internal");
            WinCashPerBattle = CFG.ReadX("WinCashPerBattle", true, "Internal");
            ShowCashReceiveWarn = CFG.ReadX("ShowCashReceiveWarn", true, "Internal");
            MaxExpReward = CFG.ReadD("MaxExpReward", 1000, "Internal");
            MaxGoldReward = CFG.ReadD("MaxGoldReward", 1000, "Internal");
            MaxCashReward = CFG.ReadD("MaxCashReward", 1000, "Internal");
            MinCreateRank = CFG.ReadD("MinCreateRank", 15, "Internal");
            MinCreateGold = CFG.ReadD("MinCreateGold", 7500, "Internal");
            MaxClanPoints = CFG.ReadT("MaxClanPoints", 0, "Internal");
            MaxActiveClans = CFG.ReadD("MaxActiveClans", 0, "Internal");
            MaxLatency = CFG.ReadD("MaxLatency", 0, "Internal");
            MaxRepeatLatency = CFG.ReadD("MaxRepeatLatency", 0, "Internal");
            PingUpdateTime = CFG.ReadF("PingUpdateTime", 5, "Internal");
            /** Others Section **/
            UdpType = (UdpState)CFG.ReadC("UdpType", 3, "Others");
            UdpVersion = CFG.ReadS("UdpVersion", "1508.7", "Others");
            SendInfoToServ = CFG.ReadX("SendInfoToServ", true, "Others");
            SendPingSync = CFG.ReadX("SendPingSync", true, "Others");
            AESProxy = CFG.ReadX("AESProxy", false, "Others");
            EnableLog = CFG.ReadX("EnableLog", false, "Others");
            PlantDuration = CFG.ReadT("PlantDuration", 5.5F, "Others");
            DefuseDuration = CFG.ReadT("DefuseDuration", 7.1F, "Others");
            SendFailMsg = CFG.ReadX("SendFailMsg", true, "Others");
            UseHitMarker = CFG.ReadX("UseHitMarker", false, "Others");
            UseMaxAmmoInDrop = CFG.ReadX("UseMaxAmmoInDrop", false, "Others");
            MaxDropWpnCount = CFG.ReadUH("MaxDropWpnCount", 0, "Others");
            AntiScript = CFG.ReadX("AntiScript", true, "Others");
            GameLocales = new List<ClientLocale>();
            National = (NationsEnum)Enum.Parse(typeof(NationsEnum), CFG.ReadS("National", "Global", "Essentials"));
            string strLocales = CFG.ReadS("Region", "None", "Essentials");
            foreach (string splitedLocale in strLocales.Split(','))
            {
                Enum.TryParse(splitedLocale, out ClientLocale clientLocale);
                GameLocales.Add(clientLocale);
            }
            CLogger.Print("Config Loader Successfully Loaded.", LoggerType.Info);
        }
        public static void LoadTimeline()
        {
            ConfigEngine CFG = new ConfigEngine("Config/Timeline.ini", FileAccess.Read);
            IsLockedTime = CFG.ReadX("LockedTime", true, "Addons");
            LockYear = CFG.ReadD("LockYear", 2000, "Runtime");
            BackYear = CFG.ReadD("BackYear", 10, "Runtime");
        }
    }
}