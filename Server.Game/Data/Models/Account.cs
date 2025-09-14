using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Managers;
using Server.Game.Data.Sync;
using Server.Game.Data.Utils;
using Server.Game.Data.XML;
using Server.Game.Network;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Game.Data.Models
{
    public class Account
    {
        public bool IsOnline;
        public bool HideGMcolor;
        public bool AntiKickGM;
        public bool LoadedShop;
        public string Nickname;
        public string Password;
        public string Username;
        public string HardwareId;
        public string Email;
        public string FindPlayer;
        public long PlayerId;
        public long BanObjectId;
        public uint LastRankUpDate;
        public uint LastLoginDate;
        public uint ClanDate;
        public IPAddress PublicIP;
        public CouponEffects Effects;
        public AccessLevel Access;
        public CafeEnum CafePC;
        public int InventoryPlus;
        public int Sight;
        public int FindClanId;
        public int LastRoomPage;
        public int LastPlayerPage;
        public int TourneyLevel;
        public int ServerId;
        public int ChannelId;
        public int ClanAccess;
        public int Exp;
        public int Gold;
        public int Cash;
        public int ClanId;
        public int SlotId;
        public int NickColor;
        public int Rank;
        public int Ribbon;
        public int Ensign;
        public int Medal;
        public int MasterMedal;
        public int MatchSlot;
        public int Age;
        public int Tags;
        public int NameCard;
        public GameClient Connection;
        public RoomModel Room;
        public PlayerSession Session;
        public MatchModel Match;
        public PlayerConfig Config = new PlayerConfig();
        public PlayerBonus Bonus = new PlayerBonus();
        public PlayerEvent Event = new PlayerEvent();
        public PlayerTitles Title = new PlayerTitles();
        public PlayerInventory Inventory = new PlayerInventory();
        public PlayerStatistic Statistic = new PlayerStatistic();
        public PlayerCharacters Character = new PlayerCharacters();
        public PlayerEquipment Equipment = new PlayerEquipment();
        public PlayerFriends Friend = new PlayerFriends();
        public PlayerQuickstart Quickstart = new PlayerQuickstart();
        public PlayerMissions Mission = new PlayerMissions();
        public AccountStatus Status = new AccountStatus();
        public List<Account> ClanPlayers = new List<Account>();
        public List<PlayerTopup> TopUps = new List<PlayerTopup>();
        public DateTime LastLobbyEnter = DateTimeUtil.Now();
        public DateTime LastPingDebug;
        public Account()
        {
            Nickname = "";
            Password = "";
            Username = "";
            HardwareId = "";
            Email = "";
            FindPlayer = "";
            ServerId = -1;
            ChannelId = -1;
            SlotId = -1;
            MatchSlot = -1;
        }
        public void SimpleClear()
        {
            Title = new PlayerTitles();
            Equipment = new PlayerEquipment();
            Inventory = new PlayerInventory();
            Status = new AccountStatus();
            Character = new PlayerCharacters();
            Statistic = new PlayerStatistic();
            Quickstart = new PlayerQuickstart();
            Mission = new PlayerMissions();
            Bonus = new PlayerBonus();
            Event = new PlayerEvent();
            Config = new PlayerConfig();
            TopUps.Clear();
            Friend.CleanList();
            Session = null;
            Match = null;
            Room = null;
            Connection = null;
        }
        public void SetPublicIP(IPAddress address)
        {
            if (address == null)
            {
                PublicIP = new IPAddress(new byte[4]);
            }
            PublicIP = address;
        }
        public void SetPublicIP(string address)
        {
            PublicIP = IPAddress.Parse(address);
        }
        public ChannelModel GetChannel()
        {
            return ChannelsXML.GetChannel(ServerId, ChannelId);
        }
        public void ResetPages()
        {
            LastRoomPage = 0;
            LastPlayerPage = 0;
        }
        public bool GetChannel(out ChannelModel channel)
        {
            channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            return channel != null;
        }
        public void SetOnlineStatus(bool online)
        {
            if (IsOnline != online && ComDiv.UpdateDB("accounts", "online", online, "player_id", PlayerId))
            {
                IsOnline = online;
                CLogger.Print($"Account User: {Username}, Player UID: {PlayerId}, Is {(IsOnline ? "Connected" : "Disconnected")}", LoggerType.Info);
            }
        }
        public void UpdateCacheInfo()
        {
            if (PlayerId == 0)
            {
                return;
            }
            lock (AccountManager.Accounts)
            {
                AccountManager.Accounts[PlayerId] = this;
            }
        }
        public int GetRank()
        {
            return Bonus == null || Bonus.FakeRank == 55 ? Rank : Bonus.FakeRank;
        }
        public void Close(int time, bool kicked = false)
        {
            if (Connection != null)
            {
                Connection.Close(time, true, kicked);
            }
        }
        public void SendPacket(GameServerPacket Packet)
        {
            if (Connection != null)
            {
                Connection.SendPacket(Packet);
            }
        }
        public void SendPacket(GameServerPacket Packet, bool OnlyInServer)
        {
            if (Connection != null)
            {
                Connection.SendPacket(Packet);
            }
            else if (!OnlyInServer && Status.ServerId != 255 && Status.ServerId != ServerId)
            {
                GameXender.Sync.SendBytes(PlayerId, Packet, Status.ServerId);
            }
        }
        public void SendPacket(byte[] Data, string PacketName)
        {
            if (Connection != null)
            {
                Connection.SendPacket(Data, PacketName);
            }
        }
        public void SendPacket(byte[] Data, string PacketName, bool OnlyInServer)
        {
            if (Connection != null)
            {
                Connection.SendPacket(Data, PacketName);
            }
            else if (!OnlyInServer && Status.ServerId != 255 && Status.ServerId != ServerId)
            {
                GameXender.Sync.SendBytes(PlayerId, PacketName, Data, Status.ServerId);
            }
        }
        public void SendCompletePacket(byte[] Data, string PacketName)
        {
            if (Connection != null)
            {
                Connection.SendCompletePacket(Data, PacketName);
            }
        }
        public void SendCompletePacket(byte[] Data, string PacketName, bool OnlyInServer)
        {
            if (Connection != null)
            {
                Connection.SendCompletePacket(Data, PacketName);
            }
            else if (!OnlyInServer && Status.ServerId != 255 && Status.ServerId != ServerId)
            {
                GameXender.Sync.SendCompleteBytes(PlayerId, PacketName, Data, Status.ServerId);
            }
        }
        public long StatusId()
        {
            return string.IsNullOrEmpty(Nickname) ? 0 : 1;
        }
        public int GetSessionId()
        {
            return Session != null ? Session.SessionId : 0;
        }
        public void SetPlayerId(long PlayerId, int LoadType)
        {
            this.PlayerId = PlayerId;
            GetAccountInfos(LoadType);
        }
        public void GetAccountInfos(int LoadType)
        {
            if (LoadType > 0 && PlayerId > 0)
            {
                if ((LoadType & 1) == 1)
                {
                    AllUtils.LoadPlayerEquipments(this);
                }
                if ((LoadType & 2) == 2)
                {
                    AllUtils.LoadPlayerCharacters(this);
                }
                if ((LoadType & 4) == 4)
                {
                    AllUtils.LoadPlayerStatistic(this);
                }
                if ((LoadType & 8) == 8)
                {
                    AllUtils.LoadPlayerTitles(this);
                }
                if ((LoadType & 16) == 16)
                {
                    AllUtils.LoadPlayerBonus(this);
                }
                if ((LoadType & 32) == 32)
                {
                    AllUtils.LoadPlayerFriend(this, true); //
                }
                if ((LoadType & 64) == 64)
                {
                    AllUtils.LoadPlayerEvent(this);
                }
                if ((LoadType & 128) == 128)
                {
                    AllUtils.LoadPlayerConfig(this);
                }
                if ((LoadType & 256) == 256)
                {
                    AllUtils.LoadPlayerFriend(this, false);
                }
                if ((LoadType & 512) == 512)
                {
                    AllUtils.LoadPlayerQuickstarts(this);
                }
            }
        }
        public bool UseChatGM()
        {
            return !HideGMcolor && (Rank == 53 || Rank == 54);
        }
        public bool IsGM()
        {
            return Rank == 53 || Rank == 54 || HaveGMLevel();
        }
        public bool HaveGMLevel()
        {
            return Access > AccessLevel.VIP;
        }
        public bool HaveAcessLevel()
        {
            return Access > AccessLevel.NORMAL;
        }
        public bool HavePermission(string Permission)
        {
            return PermissionXML.HavePermission(Permission, Access);
        }
    }
}
