using Server.Game.Network.ServerPacket;
using Server.Game.Data.Managers;
using Server.Game.Data.Utils;
using System.Collections.Generic;
using Plugin.Core.Enums;
using Server.Game.Network;
using Plugin.Core.Models;
using Server.Game.Data.XML;

namespace Server.Game.Data.Models
{
    public class MatchModel
    {
        public ClanModel Clan;
        public int Training;
        public int ServerId;
        public int ChannelId;
        public int MatchId = -1;
        public int Leader;
        public int FriendId;
        public SlotMatch[] Slots = new SlotMatch[8];
        public MatchState State = MatchState.Ready;
        public MatchModel(ClanModel Clan)
        {
            this.Clan = Clan;
            for (int index = 0; index < 8; ++index)
            {
                Slots[index] = new SlotMatch(index);
            }
        }
        public bool GetSlot(int slotId, out SlotMatch slot)
        {
            lock (Slots)
            {
                slot = null;
                if (slotId >= 0 && slotId <= 15)
                {
                    slot = Slots[slotId];
                }
                return slot != null;
            }
        }
        public SlotMatch GetSlot(int slotId)
        {
            lock (Slots)
            {
                if (slotId >= 0 && slotId <= 15)
                {
                    return Slots[slotId];
                }
                return null;
            }
        }
        public void SetNewLeader(int Leader, int OldLeader)
        {
            lock (Slots)
            {
                if (Leader == -1)
                {
                    for (int i = 0; i < Training; ++i)
                    {
                        if (i != OldLeader && Slots[i].PlayerId > 0)
                        {
                            this.Leader = i;
                            break;
                        }
                    }
                }
                else
                {
                    this.Leader = Leader;
                }
            }
        }
        public bool AddPlayer(Account player)
        {
            lock (Slots)
            {
                for (int i = 0; i < Training; i++)
                {
                    SlotMatch slot = Slots[i];
                    if (slot.PlayerId == 0 && slot.State == SlotMatchState.Empty)
                    {
                        slot.PlayerId = player.PlayerId;
                        slot.State = SlotMatchState.Normal;
                        player.Match = this;
                        player.MatchSlot = i;
                        player.Status.UpdateClanMatch((byte)FriendId);
                        AllUtils.SyncPlayerToClanMembers(player);
                        return true;
                    }
                }
            }
            return false;
        }
        public Account GetPlayerBySlot(SlotMatch slot)
        {
            try
            {
                long id = slot.PlayerId;
                return id > 0 ? AccountManager.GetAccount(id, true) : null;
            }
            catch
            {
                return null;
            }
        }
        public Account GetPlayerBySlot(int slotId)
        {
            try
            {
                long id = Slots[slotId].PlayerId;
                return id > 0 ? AccountManager.GetAccount(id, true) : null;
            }
            catch
            {
                return null;
            }
        }
        public List<Account> GetAllPlayers(int exception)
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                for (int i = 0; i < 8; i++)
                {
                    long id = Slots[i].PlayerId;
                    if (id > 0 && i != exception)
                    {
                        Account p = AccountManager.GetAccount(id, true);
                        if (p != null)
                        {
                            list.Add(p);
                        }
                    }
                }
            }
            return list;
        }
        public List<Account> GetAllPlayers()
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                for (int i = 0; i < 8; i++)
                {
                    long PlayerId = Slots[i].PlayerId;
                    if (PlayerId > 0)
                    {
                        Account p = AccountManager.GetAccount(PlayerId, true);
                        if (p != null)
                        {
                            list.Add(p);
                        }
                    }
                }
            }
            return list;
        }
        public void SendPacketToPlayers(GameServerPacket Packet)
        {
            List<Account> Players = GetAllPlayers();
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Match.SendPacketToPlayers(SendPacket)");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet, int Exception)
        {
            List<Account> Players = GetAllPlayers(Exception);
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Match.SendPacketToPlayers(SendPacket,int)");
            foreach(Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public Account GetLeader()
        {
            try
            {
                return AccountManager.GetAccount(Slots[Leader].PlayerId, true);
            }
            catch
            {
                return null;
            }
        }
        public int GetServerInfo()
        {
            return ChannelId + ServerId * 10;
        }
        public int GetCountPlayers()
        {
            lock (Slots)
            {
                int count = 0;
                foreach (SlotMatch s in Slots)
                {
                    if (s.PlayerId > 0)
                    {
                        ++count;
                    }
                }
                return count;
            }
        }
        private void BaseRemovePlayer(Account Player)
        {
            lock (Slots)
            {
                if (!GetSlot(Player.MatchSlot, out SlotMatch slot))
                {
                    return;
                }
                if (slot.PlayerId == Player.PlayerId)
                {
                    slot.PlayerId = 0;
                    slot.State = SlotMatchState.Empty;
                }
            }
        }
        public bool RemovePlayer(Account Player)
        {
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return false;
            }
            BaseRemovePlayer(Player);
            if (GetCountPlayers() == 0)
            {
                Channel.RemoveMatch(MatchId);
            }
            else
            {
                if (Player.MatchSlot == Leader)
                {
                    SetNewLeader(-1, -1);
                }
                using (PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK packet = new PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(this))
                {
                    SendPacketToPlayers(packet);
                }
            }
            Player.MatchSlot = -1;
            Player.Match = null;
            return true;
        }
    }
}
