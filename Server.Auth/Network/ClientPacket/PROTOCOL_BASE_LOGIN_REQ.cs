using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Sync.Server;
using Server.Auth.Network.ServerPacket;
using System;
using System.Net.NetworkInformation;
using Plugin.Core.Models;
using Server.Auth.Data.Models;
using Plugin.Core.SQL;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_LOGIN_REQ : AuthClientPacket
    {
        private string Username;
        private string Password;
        private string GameVersion;
        private string PublicIP;
        //private string D3D9MD5;
        private string UserFileListMD5;
        private ClientLocale Region;
        private PhysicalAddress MacAddress;
        public PROTOCOL_BASE_LOGIN_REQ(AuthClient Client, byte[] Buff)
        {
            Makeme(Client, Buff);
        }
        public override void Read()
        {
            ReadB(4);
            MacAddress = new PhysicalAddress(ReadB(6));
            ReadB(2);
            ReadB(18);
            short v = ReadH();
            short h = ReadH();
            ReadB(10);
            UserFileListMD5 = ReadS(32);
            ReadB(32);
            ReadC();
            Region = (ClientLocale)ReadC();
            GameVersion = $"{ReadC()}.{ReadH()}";
            ReadH();
            Password = ReadS(ReadC());
            Username = ReadS(ReadC());
            PublicIP = Client.GetIPAddress();
        }
        public override void Run()
        {
            try
            {
                /*
                bool IsValid = DirectLibraryXML.IsValid(D3D9MD5);
                if (!IsValid)
                {
                    CLogger.Warning($"No listed: {D3D9MD5} [{Username}]", null);
                }
                */
                ServerConfig CFG = AuthXender.Client.Config;
                if (CFG == null || !ConfigLoader.IsTestMode && !ConfigLoader.GameLocales.Contains(Region) || Username.Length < ConfigLoader.MinUserSize || !ConfigLoader.IsTestMode && Password.Length < ConfigLoader.MinPassSize || MacAddress.GetAddressBytes() == new byte[6] || GameVersion != CFG.ClientVersion || (CFG.AccessUFL && UserFileListMD5 != CFG.UserFileList)) // || ConfigLoader.LauncherKey > 0 && key != ConfigLoader.LauncherKey)
                {
                    string Message = "";
                    if (CFG == null)
                    {
                        Message = $"Invalid server config [{Username}]";
                    }
                    else if (!ConfigLoader.IsTestMode && !ConfigLoader.GameLocales.Contains(Region))
                    {
                        Message = $"Country: {Region} of blocked client [{Username}]";
                    }
                    else if (Username.Length < ConfigLoader.MinUserSize)
                    {
                        Message = $"Username too short [{Username}]";
                    }
                    else if (!ConfigLoader.IsTestMode && Password.Length < ConfigLoader.MinPassSize)
                    {
                        Message = $"Password too short [{Username}]";
                    }
                    else if (MacAddress.GetAddressBytes() == new byte[6])
                    {
                        Message = $"Invalid MAC Address [{Username}]";
                    }
                    else if (GameVersion != CFG.ClientVersion)
                    {
                        Message = $"Version: {GameVersion} not supported [{Username}]";
                    }
                    else if (CFG.AccessUFL && UserFileListMD5 != CFG.UserFileList)
                    {
                        Message = $"UserFileList: {UserFileListMD5} not supported [{Username}]";
                    }
                    else
                    {
                        Message = $"There is something wrong happened when trying to login {Username}";
                    }
                    Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_DISCONNECTIONSUCCESS_ACK(0x80000100, false));
                    CLogger.Print(Message, LoggerType.Warning);
                    Client.Close(1000, true);
                }
                else
                {
                    Client.Player = AccountManager.GetAccountDB(Username, null, 0, 95);
                    if (Client.Player == null && ConfigLoader.AutoAccount && !AccountManager.CreateAccount(out Client.Player, Username, Password))
                    {
                        Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum.LOGIN_DELETE_ACCOUNT, Username, 0));
                        CLogger.Print($"Failed to create account automatically [{Username}]", LoggerType.Warning);
                        Client.Close(1000, false);
                    }
                    else
                    {
                        Account Player = Client.Player;
                        if (Player == null || !Player.ComparePassword(Password))
                        {
                            string Message = "";
                            EventErrorEnum ErrorEvent = EventErrorEnum.FAIL;
                            if (Player == null)
                            {
                                Message = "Account returned from DB is null";
                                ErrorEvent = EventErrorEnum.LOGIN_DELETE_ACCOUNT;
                            }
                            else if (!Player.ComparePassword(Password))
                            {
                                Message = "Invalid password";
                                ErrorEvent = EventErrorEnum.LOGIN_ID_PASS_INCORRECT;
                            }
                            Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(ErrorEvent, Username, 0));
                            CLogger.Print(Message + " [" + Username + "]", LoggerType.Warning);
                            Client.Close(1000, false);
                        }
                        else if (Player.Access >= AccessLevel.NORMAL)
                        {
                            if (Player.MacAddress != MacAddress)
                            {
                                ComDiv.UpdateDB("accounts", "mac_address", MacAddress, "player_id", Player.PlayerId);
                            }
                            DaoManagerSQL.GetBanStatus(MacAddress.ToString(), PublicIP, out bool MAC, out bool IP4);
                            if (MAC || IP4)
                            {
                                CLogger.Print($"{(MAC ? "MAC Address blocked" : "IP4 Address blocked")} [{Username}]", LoggerType.Warning);
                                Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(IP4 ? EventErrorEnum.LOGIN_BLOCK_IP : EventErrorEnum.EVENT_LOG_IN_BLOCK_ACCOUNT, Username, 0));
                                Client.Close(1000, false);
                            }
                            else if (Player.IsGM() && CFG.OnlyGM || Player.Access >= AccessLevel.NORMAL && !CFG.OnlyGM)
                            {
                                Account pCache = AccountManager.GetAccount(Player.PlayerId, true);
                                if (!Player.IsOnline)
                                {
                                    BanHistory htb = DaoManagerSQL.GetAccountBan(Player.BanObjectId);
                                    if (htb.EndDate > DateTimeUtil.Now())
                                    {
                                        Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum.EVENT_LOG_IN_BLOCK_ACCOUNT, Username, 0));
                                        CLogger.Print("Account with ban is Active [" + Username + "]", LoggerType.Warning);
                                        Client.Close(1000, false);
                                    }
                                    else
                                    {
                                        Player.SetPlayerId(Player.PlayerId, 991);
                                        Player.ClanPlayers = ClanManager.GetClanPlayers(Player.ClanId, Player.PlayerId);
                                        Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(0, Username, Player.PlayerId));
                                        Client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, Player));
                                        if (Player.ClanId > 0)
                                        {
                                            Client.SendPacket(new PROTOCOL_CS_MEMBER_INFO_ACK(Player.ClanPlayers));
                                        }
                                        Player.Status.SetData(4294967295, Player.PlayerId);
                                        Player.Status.UpdateServer(0);
                                        Player.SetOnlineStatus(true);
                                        if (pCache != null)
                                        {
                                            pCache.Connection = Client;
                                        }
                                        SendRefresh.RefreshAccount(Player, true);
                                    }
                                }
                                else
                                {
                                    Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum.EVENT_LOG_IN_ALREADY_LOGIN, Username, 0));
                                    CLogger.Print($"Account online [{Username}]", LoggerType.Warning);
                                    if (pCache != null && pCache.Connection != null)
                                    {
                                        pCache.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(1));
                                        pCache.SendPacket(new PROTOCOL_SERVER_MESSAGE_ERROR_ACK(0x80001000));
                                        pCache.Close(1000);
                                    }
                                    else
                                    {
                                        AuthLogin.SendLoginKickInfo(Player);
                                    }
                                    Client.Close(1000, false);
                                }
                            }
                            else
                            {
                                Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum.EVENT_LOG_IN_TIME_OUT_2, Username, 0));
                                CLogger.Print($"Invalid access level [{Username}]", LoggerType.Warning);
                                Client.Close(1000, false);
                            }
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum.EVENT_LOG_IN_BLOCK_ACCOUNT, Username, 0));
                            CLogger.Print($"Permanently banned [{Username}]", LoggerType.Warning);
                            Client.Close(1000, false);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
