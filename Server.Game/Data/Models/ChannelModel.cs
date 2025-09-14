using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Sync.Server;
using Server.Game.Network;
using System;
using System.Collections.Generic;

namespace Server.Game.Data.Models
{
    public class ChannelModel
    {
        public int Id;
        public ChannelType Type;
        public int ServerId;
        public int MaxRooms;
        public int ExpBonus;
        public int GoldBonus;
        public int CashBonus;
        public string Password;
        public List<PlayerSession> Players = new List<PlayerSession>();
        public List<RoomModel> Rooms = new List<RoomModel>();
        public List<MatchModel> Matches = new List<MatchModel>();
        private DateTime LastRoomsSync = DateTimeUtil.Now();
        public ChannelModel(int ServerId)
        {
            this.ServerId = ServerId;
        }
        public PlayerSession GetPlayer(int session)
        {
            lock (Players)
            {
                foreach (PlayerSession Session in Players)
                {
                    if (Session.SessionId == session)
                    {
                        return Session;
                    }
                }
                return null;
            }
        }
        public PlayerSession GetPlayer(int SessionId, out int Index)
        {
            Index = -1;
            lock (Players)
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    PlayerSession Session = Players[i];
                    if (Session.SessionId == SessionId)
                    {
                        Index = i;
                        return Session;
                    }
                }
                return null;
            }
        }
        public bool AddPlayer(PlayerSession pS)
        {
            lock (Players)
            {
                if (!Players.Contains(pS))
                {
                    Players.Add(pS);
                    UpdateServer.RefreshSChannel(ServerId);
                    UpdateChannel.RefreshChannel(ServerId, Id, Players.Count);
                    return true;
                }
                return false;
            }
        }
        public void RemoveMatch(int matchId)
        {
            lock (Matches)
            {
                for (int i = 0; i < Matches.Count; ++i)
                {
                    if (matchId == Matches[i].MatchId)
                    {
                        Matches.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public void AddMatch(MatchModel match)
        {
            lock (Matches)
            {
                if (!Matches.Contains(match))
                {
                    Matches.Add(match);
                }
            }
        }
        public void AddRoom(RoomModel room)
        {
            lock (Rooms)
            {
                Rooms.Add(room);
            }
        }
        public void RemoveEmptyRooms()
        {
            lock (Rooms)
            {
                if (ComDiv.GetDuration(LastRoomsSync) >= 2)
                {
                    LastRoomsSync = DateTimeUtil.Now();
                    for (int i = 0; i < Rooms.Count; ++i)
                    {
                        RoomModel Room = Rooms[i];
                        if (Room.GetCountPlayers() < 1)
                        {
                            Rooms.RemoveAt(i--);
                        }
                    }
                }
            }
        }
        public MatchModel GetMatch(int id)
        {
            lock (Matches)
            {
                foreach (MatchModel Match in Matches)
                {
                    if (Match.MatchId == id)
                    {
                        return Match;
                    }
                }
                return null;
            }
        }
        public MatchModel GetMatch(int id, int clan)
        {
            lock (Matches)
            {
                foreach (MatchModel Match in Matches)
                {
                    if (Match.FriendId == id && Match.Clan.Id == clan)
                    {
                        return Match;
                    }
                }
                return null;
            }
        }
        public RoomModel GetRoom(int id)
        {
            lock (Rooms)
            {
                foreach (RoomModel room in Rooms)
                {
                    if (room.RoomId == id)
                    {
                        return room;
                    }
                }
                return null;
            }
        }
        public List<Account> GetWaitPlayers()
        {
            List<Account> List = new List<Account>();
            lock (Players)
            {
                foreach (PlayerSession Session in Players)
                {
                    Account Player = AccountManager.GetAccount(Session.PlayerId, true);
                    if (Player != null && Player.Room == null && !string.IsNullOrEmpty(Player.Nickname))
                    {
                        List.Add(Player);
                    }
                }
            }
            return List;
        }
        public void SendPacketToWaitPlayers(GameServerPacket Packet)
        {
            List<Account> Players = GetWaitPlayers();
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Channel.SendPacketToWaitPlayers");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public bool RemovePlayer(Account Player)
        {
            bool Result = false;
            Player.ChannelId = -1;
            Player.ServerId = -1;
            if (Player.Session != null)
            {
                lock (Players)
                {
                    Result = Players.Remove(Player.Session);
                }
                UpdateChannel.RefreshChannel(ServerId, Id, Players.Count);
                if (Result)
                {
                    UpdateServer.RefreshSChannel(ServerId);
                }
            }
            return Result;
        }
    }
}