using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Utils;
using Server.Auth.Network;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Server.Auth.Data.Models
{
    public class Account
    {
        public bool MyConfigsLoaded;
        public bool IsOnline;
        public AccessLevel Access;
        public CouponEffects Effects;
        public CafeEnum CafePC;
        public uint LastRankUpDate;
        public string Nickname;
        public string Password;
        public string Username;
        public string HardwareId;
        public string Email;
        public int NickColor;
        public int NameCard;
        public int InventoryPlus;
        public int TourneyLevel;
        public int Exp;
        public int Gold;
        public int ClanId;
        public int ClanAccess;
        public int Cash;
        public int Rank;
        public int Ribbon;
        public int Ensign;
        public int Medal;
        public int MasterMedal;
        public int Age;
        public int Tags;
        public long PlayerId;
        public long BanObjectId;
        public PhysicalAddress MacAddress;
        public AuthClient Connection;
        public PlayerBonus Bonus = new PlayerBonus();
        public PlayerConfig Config = new PlayerConfig();
        public PlayerEvent Event = new PlayerEvent();
        public PlayerTitles Title = new PlayerTitles();
        public PlayerInventory Inventory = new PlayerInventory();
        public AccountStatus Status = new AccountStatus();
        public PlayerFriends Friend = new PlayerFriends();
        public PlayerStatistic Statistic = new PlayerStatistic();
        public PlayerQuickstart Quickstart = new PlayerQuickstart();
        public PlayerCharacters Character = new PlayerCharacters();
        public PlayerEquipment Equipment = new PlayerEquipment();
        public PlayerMissions Mission = new PlayerMissions();
        public List<Account> ClanPlayers = new List<Account>();
        public List<PlayerTopup> TopUps = new List<PlayerTopup>();
        public DateTime LastLoginDate = DateTimeUtil.Now();
        public Account()
        {
            Nickname = "";
            Password = "";
            Username = "";
            HardwareId = "";
            Email = "";
        }
        public void SimpleClear()
        {
            Connection = null;
            Config = new PlayerConfig();
            Bonus = new PlayerBonus();
            Event = new PlayerEvent();
            Title = new PlayerTitles();
            Inventory = new PlayerInventory();
            Character = new PlayerCharacters();
            Equipment = new PlayerEquipment();
            Friend = new PlayerFriends();
            Status = new AccountStatus();
            Mission = new PlayerMissions();
            Quickstart = new PlayerQuickstart();
            ClanPlayers = new List<Account>();
            TopUps = new List<PlayerTopup>();
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
        public void Close(int time)
        {
            if (Connection != null)
            {
                Connection.Close(time, true);
            }
        }
        public void SendPacket(AuthServerPacket Packet)
        {
            if (Connection != null)
            {
                Connection.SendPacket(Packet);
            }
        }
        public void SendPacket(byte[] Data, string PacketName)
        {
            if (Connection != null)
            {
                Connection.SendPacket(Data, PacketName);
            }
        }
        public void SendCompletePacket(byte[] Data, string PacketName)
        {
            if (Connection != null)
            {
                Connection.SendCompletePacket(Data, PacketName);
            }
        }
        public long StatusId()
        {
            return string.IsNullOrEmpty(Nickname) ? 0 : 1;
        }
        public void SetPlayerId(long PlayerId, int LoadType)
        {
            this.PlayerId = PlayerId;
            GetAccountInfos(LoadType);
        }
        public bool ComparePassword(string Password)
        {
            return ConfigLoader.IsTestMode || this.Password == Password;
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
                    AllUtils.LoadPlayerFriend(this, true); //x
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
        public int GetRank()
        {
            return Bonus == null || Bonus.FakeRank == 55 ? Rank : Bonus.FakeRank;
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