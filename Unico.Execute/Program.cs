using Console = Plugin.Core.Colorful.Console;
using Executable.Forms;
using Executable.Utility;
using Plugin.Core;
using Plugin.Core.Colorful;
using Plugin.Core.Enums;
using Plugin.Core.Filters;
using Plugin.Core.JSON;
using Plugin.Core.Managers;
using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.RAW;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Auth;
using Server.Game;
using Server.Match;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Executable.UDP.Server;
using Executable.UDP;

namespace Executable
{
    public class Program
    {
        #region FirewallAction
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        private static bool Handler(CtrlType sig)
        {
            switch (sig) 
            {
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT: FirewallUtil.RemoveFirewallRule(Executable.FullName); return true;
                case CtrlType.CTRL_C_EVENT:
                default: return false;
            }
        }
        #endregion FirewallAction
        protected static string TitleARG = "", Ver = "V3.68";
        protected static Mutex Mutexue = null;
        protected static readonly FileInfo Executable = new FileInfo(Assembly.GetExecutingAssembly().Location);
        protected static readonly int ProcessId = Process.GetCurrentProcess().Id;
        [STAThread]
        protected static void Main(string[] Args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomainOnUnhandledException);
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressEvent);
            try
            {
                Console.SetWindowSize(160, 40); //Width, Height
                Console.CursorVisible = false;
                Console.TreatControlCAsInput = false;
                WindowUtility.MoveWindowToCenter();
                string FileVersion = FileVersionInfo.GetVersionInfo(Executable.Name).FileVersion;
                Console.Title = $"Point Blank (RUS-{Ver}) Server {FileVersion}";
                CLogger.StartedFor = "Server";
                CLogger.CheckDirectorys();
                Console.Clear();
                foreach (Process P in Process.GetProcessesByName("PointBlank"))
                {
                    P.Kill();
                }
                Mutexue = new Mutex(true, Executable.Name, out bool CreatedNew);
                if (!CreatedNew)
                {
                    CLogger.Print("The server is already running! Exiting...", LoggerType.Warning);
                    MessageBox.Show("The server is already running!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    Thread.Sleep(1000);
                    WindowUtility.KillProcessAndChildren(ProcessId);
                }
                else
                {
                    /*
                    DateTime LiveDate = GetDate();
                    if (LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 240111235959)
                    {
                        WindowUtility.KillProcessAndChildren(ProcessId);
                        return;
                    }
                    */
                    if (MemoryUtility.IsAdministrator())
                    {
                        SetConsoleCtrlHandler(new EventHandler(Handler), true);
                        FirewallUtil.AddFirewallRule(Executable.FullName);
                    }
                    TitleARG = MemoryUtility.IsAdministrator() ? "ADMINISTRATOR" : "SUPERUSER";
                    DateTime Date = Executable.LastWriteTime;
                    StringBuilder OwnTitle = new StringBuilder();
                    OwnTitle.AppendLine(@"__________                 ______      _____");
                    OwnTitle.AppendLine(@"___  ____/______ _______  ____  /_____ __  /______________");
                    OwnTitle.AppendLine(@"__  __/  __  __ `__ \  / / /_  /_  __ `/  __/  __ \_  ___/");
                    OwnTitle.AppendLine(@"_  /___  _  / / / / / /_/ /_  / / /_/ // /_ / /_/ /  /");
                    OwnTitle.AppendLine(@"/_____/  /_/ /_/ /_/\__,_/ /_/  \__,_/ \__/ \____//_/");
                    OwnTitle.AppendLine("Contributors{0} {1}");
                    OwnTitle.AppendLine("");
                    OwnTitle.AppendLine(PrintRight("MoMz Games"));
                    OwnTitle.AppendLine(PrintRight($"{Ver} +Build: {Date:yyMMddHHmmss}"));
                    OwnTitle.AppendLine(PrintRight("Copyright 2021 Zepetto Co. All right reserved"));
                    Formatter[] TitleFormat = new Formatter[]
                    {
                        new Formatter(":", ColorUtil.Yellow),
                        new Formatter("Garry, zOne62, Pavel, Monester, Hannie, Fusion", ColorUtil.Cyan)
                    };
                    Console.WriteLineFormatted(OwnTitle.ToString(), ColorUtil.White, TitleFormat);
                    bool IncludeArgument = ArgumentChecker(Args).Equals("-supc");
                    if (IncludeArgument)
                    {
                        new Thread(() => LoadMonitorForm()).Start();
                    }
                    LoadAllServerPlugin(IncludeArgument ? 1 : 0, FileVersion);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            finally
            {
                Mutexue.ReleaseMutex();
                Process.GetCurrentProcess().WaitForExit();
            }
        }
        private static string PrintRight(string Text)
        {
            StringBuilder ST = new StringBuilder(80);
            if (Text != null)
            {
                ST.Append(" ");
            }
            int Count = 79 - Text.Length;
            while (ST.Length != Count)
            {
                ST.Append(" ");
            }
            ST.Append(Text);
            return ST.ToString();
        }
        private static void LoadAllServerPlugin(int TitleType, string Version)
        {
            PrintSection("Config", "Begin");
            ConfigLoader.LoadTimeline();
            ConfigLoader.Load();
            ServerConfigJSON.Load();
            PrintSection("Config", "Ended");
            PrintSection("Events Data", "Begin");
            EventLoginSync.Load();
            EventMapSync.Load();
            EventPlaytimeSync.Load();
            EventQuestSync.Load();
            EventRankUpSync.Load();
            EventVisitSync.Load();
            EventXmasSync.Load();
            PrintSection("Events Data", "Ended");
            PrintSection("Shop Data", "Begin");
            ShopManager.Load(1);
            ShopManager.Load(2);
            PrintSection("Shop Data", "Ended");
            PrintSection("Mission Cards", "Begin");
            MissionCardRAW.LoadBasicCards(1);
            MissionCardRAW.LoadBasicCards(2);
            PrintSection("Mission Cards", "Ended");
            PrintSection("Server Data", "Begin");
            TemplatePackXML.Load();
            TitleSystemXML.Load();
            TitleAwardXML.Load();
            MissionAwardXML.Load();
            MissionConfigXML.Load();
            SChannelXML.Load();
            SynchronizeXML.Load();
            SystemMapXML.Load();
            ClanRankXML.Load();
            PlayerRankXML.Load();
            CouponEffectXML.Load();
            PermissionXML.Load();
            RandomBoxXML.Load();
            GameRuleXML.Load();
            DirectLibraryXML.Load();
            InternetCafeXML.Load();
            RedeemCodeXML.Load();
            CommandHelperJSON.Load();
            HalfUtil.Load();
            NickFilter.Load();
            Translation.Load();
            PrintSection("Server Data", "Ended");
            Thread.Sleep(250);
            PrintSection("Plugin Status", "Begin");
            Communication.Start(new IPEndPoint(IPAddress.Parse(ConfigLoader.Ip4Address), 1909));
            CLogger.Print("All Server Plugins Successfully Loaded", LoggerType.Info);
            PrintSection("Plugin Status", "Ended");
            ValidateSockets(ValidateSocketDB(), TitleType, Version);
        }
        private static void PrintSection(string Name, string Type)
        {
            StringBuilder ST = new StringBuilder(80);
            if (Name != null)
            {
                ST.Append("---[").Append(Name).Append(']');
            }
            string End = (Type == null) ? "" : ($"[{Type}]---");
            int Count = 79 - End.Length;
            while (ST.Length != Count)
            {
                ST.Append('-');
            }
            ST.Append(End);
            Console.WriteLine($"{(Type.Equals("Ended") ? $"{ST}\n" : $"\n{ST}")}");
        }
        private static bool ValidateSocketDB() => ComDiv.ValidateAllPlayersAccount() && CheckAllSockets();
        private static bool CheckAllSockets()
        {
            try
            {
                #region Auth Server
                PrintSection("Auth Server", "Begin");
                Server.Auth.Data.XML.ChannelsXML.Load();
                AuthXender.GetPlugin(ConfigLoader.Ip4Address, ConfigLoader.DEFAULT_PORT[0]);
                PrintSection("Auth Server", "Ended");
                #endregion Auth Server
                #region Game Server
                PrintSection("Game Server", "Begin");
                Server.Game.Data.XML.ChannelsXML.Load();
                Server.Game.Data.Managers.ClanManager.Load();
                Server.Game.Data.Managers.CommandManager.Load();
                foreach (SChannelModel Server in SChannelXML.Servers)
                {
                    if (Server.Id >= 1 && Server.Port >= ConfigLoader.DEFAULT_PORT[1])
                    {
                        GameXender.GetPlugin(Server.Id, ConfigLoader.Ip4Address, Server.Port);
                    }
                }
                PrintSection("Game Server", "Ended");
                #endregion Game Server
                #region Battle Server
                PrintSection("Battle Server", "Begin");
                Server.Match.Data.XML.MapStructureXML.Load();
                Server.Match.Data.XML.CharaStructureXML.Load();
                Server.Match.Data.XML.ItemStatisticXML.Load();
                MatchXender.GetPlugin(ConfigLoader.Ip4Address, ConfigLoader.DEFAULT_PORT[2]);
                PrintSection("Battle Server", "Ended");
                #endregion Battle Server
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        private static void ValidateSockets(bool SocketActive, int TitleType, string Version)
        {
            StringUtility.ServerVersionL = Version;
            StringUtility.ServerStatusL = SocketActive ? "SERVER ONLINE" : "SERVER OFFLINE";
            if (SocketActive)
            {
                PrintSection("Server Status", "Begin");
                CLogger.Print($"Startup Successful, Server Runtime {DateTimeUtil.Now("yyyy")}", LoggerType.Info);
                PrintSection("Server Status", "Ended");
                Console.WriteLine("");
                try
                {
                    TitleInformation(TitleType, Version);
                }
                catch (Exception Ex)
                {
                    CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                }
            }
            else
            {
                PrintSection("Server Status", "Begin");
                CLogger.Print($"Startup Unsuccessful, Server Runtime {DateTimeUtil.Now("yyyy")}", LoggerType.Warning);
                PrintSection("Server Status", "Ended");
                Console.WriteLine("");
            }
        }
        private static async void TitleInformation(int TitleType, string Version)
        {
            while (true)
            {
                ForAsyncFormInformation();
                double MemUsages = MemoryUtility.GetMemoryUsage(), MemUsagePercent = MemoryUtility.GetMemoryUsagePercent();
                int Users = ComDiv.CountDB("SELECT COUNT(*) FROM accounts"), OnlineUsers = ComDiv.CountDB($"SELECT COUNT(*) FROM accounts WHERE online = {true}"); 
                Console.Title = $"Point Blank (RUS-{Ver}) Server {Version} </> {(TitleType == 1 ? $"RAM Usages: {MemUsages:0.0} MB)" : $"Users: {Users}; Online: {OnlineUsers}; RAM Usages: {MemUsages:0.0} MB ({MemUsagePercent:0.0}%)")} -{TitleARG} </> Timeline: {DateTimeUtil.Now("dddd, MMMM dd, yyyy - HH:mm:ss")}";
                await Task.Delay(1000);
            }
        }
        private static void LoadMonitorForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm Monitor = new MainForm(ProcessId, new DirectoryInfo($"{Executable.Directory}/Logs"))
            {
                TopMost = true
            };
            Application.Run(Monitor);
        }
        private static string GetLocalAddress()
        {
            string Address = "";
            try
            {
                IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress IP4Address in Host.AddressList)
                {
                    if (IP4Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Address = IP4Address.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return Address;
        }
        private static string GetGameLocale()
        {
            foreach (ClientLocale Region in ConfigLoader.GameLocales)
            {
                if (Region == ClientLocale.Russia)
                {
                    return Region.ToString();
                }
            }
            return "Outside!";
        }
        private static string ArgumentChecker(string[] Arguments)
        {
            foreach (string Argument in Arguments)
            {
                return Argument;
            }
            return "";
        }
        private static void ForAsyncFormInformation()
        {
            StringUtility.MemoryValueVPB = $"{Convert.ToInt32(MemoryUtility.GetMemoryUsage())}";
            StringUtility.MemoryUsageL = $"{MemoryUtility.GetMemoryUsagePercent():0.0}%";
            StringUtility.LogFileSize = $"{(((double)MemoryUtility.GetDirectorySize(new DirectoryInfo($"{Executable.Directory}/Logs"), true)) / (1024 * 1024)):N2}MB";
            StringUtility.RegisteredUserL = $"{ComDiv.CountDB("SELECT COUNT(*) FROM accounts")}";
            StringUtility.OnlineUserL = $"{ComDiv.CountDB($"SELECT COUNT(*) FROM accounts WHERE online = '{true}'")}";
            StringUtility.TotalClansL = $"{ComDiv.CountDB("SELECT COUNT(*) FROM system_clan")}";
            StringUtility.VipUserL = $"{ComDiv.CountDB($"SELECT COUNT(*) FROM accounts WHERE pc_cafe = '2' OR pc_cafe = '1'")}";
            StringUtility.UnknownUserL = $"{ComDiv.CountDB($"SELECT COUNT(*) FROM accounts WHERE nickname = ''")}";
            StringUtility.BannedPlayers = $"{ComDiv.CountDB($"SELECT COUNT(*) FROM base_auto_ban")}";
            StringUtility.RegShopItems = $"{(ComDiv.CountDB($"SELECT COUNT(*) FROM system_shop") + ComDiv.CountDB($"SELECT COUNT(*) FROM system_shop_effects") + ComDiv.CountDB($"SELECT COUNT(*) FROM system_shop_sets"))}";
            StringUtility.ShopCafeItems = $"{(ComDiv.CountDB($"SELECT COUNT(*) FROM system_shop WHERE item_visible = '{false}'") + ComDiv.CountDB($"SELECT COUNT(*) FROM system_shop_effects WHERE coupon_visible = '{false}'"))}";
            StringUtility.RepairableItems = $"{ComDiv.CountDB($"SELECT COUNT(*) FROM system_shop_repair")}";
            StringUtility.ForGameVersionL = $"V{ServerConfigJSON.GetConfig(ConfigLoader.ConfigId).ClientVersion}";
            StringUtility.ForGameRegionL = GetGameLocale();
            StringUtility.LocalAddressL = GetLocalAddress();
            StringUtility.ServerTimelineL = DateTimeUtil.Now("yyyy");
            StringUtility.SelectedServerConfigL = $"{ConfigLoader.ConfigId}";
            StringUtility.SelectedGameRuleL = $"{ConfigLoader.RuleId}";
            StringUtility.InternetCafeL = ConfigLoader.ICafeSystem ? "Enabled" : "Disabled";
            StringUtility.EnableAutoAccountL = ConfigLoader.AutoAccount ? "Enabled" : "Disabled";
            StringUtility.AutoBanPlayerL = ConfigLoader.AutoBan ? "Enabled" : "Disabled";
        }
        private static void CancelKeyPressEvent(object sender, ConsoleCancelEventArgs e)
        {
            ConfigLoader.Load();
            CLogger.Print("Application Settings Reloaded.", LoggerType.Info);
            SendMessage.FromServer("Welcome To The Server!");
            e.Cancel = true; //False = Exit
        }
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO Here!
            CLogger.Print($"Application Handle Exception Sender: {sender} Terminating: {e.IsTerminating} {(Exception)e.ExceptionObject}", LoggerType.Error);
        }
        public static DateTime GetDate()
        {
            try
            {
                using (WebResponse Response = WebRequest.Create("http://www.google.com").GetResponse())
                {
                    return DateTime.ParseExact(Response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal).ToUniversalTime();
                }
            }
            catch
            {
                return new DateTime();
            }
        }
    }
}
