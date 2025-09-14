using Plugin.Core;
using Plugin.Core.Network;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Plugin.Core.SQL;
using Server.Game.Network;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.Enums;

namespace Server.Game.Data.Managers
{
    public static class ClanManager
    {
        public static List<ClanModel> Clans = new List<ClanModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM system_clan";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        long owner = long.Parse(Data["owner_id"].ToString());
                        if (owner == 0)
                        {
                            continue;
                        }
                        string BestEXP, BestParticipant, BestWins, BestKills, BestHeadshots;
                        ClanModel Clan = new ClanModel()
                        {
                            Id = int.Parse(Data["id"].ToString()),
                            Rank = int.Parse(Data["rank"].ToString()),
                            Name = Data["name"].ToString(),
                            OwnerId = owner,
                            Logo = uint.Parse(Data["logo"].ToString()),
                            NameColor = int.Parse(Data["name_color"].ToString()),
                            Info = Data["info"].ToString(),
                            News = Data["news"].ToString(),
                            CreationDate = uint.Parse(Data["create_date"].ToString()),
                            Authority = int.Parse(Data["authority"].ToString()),
                            RankLimit = int.Parse(Data["rank_limit"].ToString()),
                            MinAgeLimit = int.Parse(Data["min_age_limit"].ToString()),
                            MaxAgeLimit = int.Parse(Data["max_age_limit"].ToString()),
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            Points = int.Parse(Data["point"].ToString()),
                            MaxPlayers = int.Parse(Data["max_players"].ToString()),
                            Exp = int.Parse(Data["exp"].ToString()),
                            Effect = int.Parse(Data["effects"].ToString())
                        };
                        BestEXP = Data["best_exp"].ToString();
                        BestParticipant = Data["best_participants"].ToString();
                        BestWins = Data["best_wins"].ToString();
                        BestKills = Data["best_kills"].ToString();
                        BestHeadshots = Data["best_headshots"].ToString();
                        Clan.BestPlayers.SetPlayers(BestEXP, BestParticipant, BestWins, BestKills, BestHeadshots);
                        Clans.Add(Clan);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static List<ClanModel> GetClanListPerPage(int Page)
        {
            List<ClanModel> Clans = new List<ClanModel>();
            if (Page == 0)
            {
                return Clans;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@page", (170 * Page));
                    Command.CommandText = "SELECT * FROM system_clan ORDER BY id DESC OFFSET @page LIMIT 170";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        long OwnerId = long.Parse(Data["owner_id"].ToString());
                        if (OwnerId == 0)
                        {
                            continue;
                        }
                        string BestEXP, BestParticipant, BestWins, BestKills, BestHeadshots;
                        ClanModel Clan = new ClanModel()
                        {
                            Id = int.Parse(Data["id"].ToString()),
                            Rank = byte.Parse(Data["rank"].ToString()),
                            Name = Data["name"].ToString(),
                            OwnerId = OwnerId,
                            Logo = uint.Parse(Data["logo"].ToString()),
                            NameColor = byte.Parse(Data["name_color"].ToString()),
                            Info = Data["info"].ToString(),
                            News = Data["news"].ToString(),
                            CreationDate = uint.Parse(Data["create_date"].ToString()),
                            Authority = int.Parse(Data["authority"].ToString()),
                            RankLimit = int.Parse(Data["rank_limit"].ToString()),
                            MinAgeLimit = int.Parse(Data["age_limit_start"].ToString()),
                            MaxAgeLimit = int.Parse(Data["age_limit_end"].ToString()),
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            Points = int.Parse(Data["point"].ToString()),
                            MaxPlayers = int.Parse(Data["max_players"].ToString()),
                            Exp = int.Parse(Data["exp"].ToString()),
                            Effect = byte.Parse(Data["effects"].ToString())
                        };
                        BestEXP = Data["best_exp"].ToString();
                        BestParticipant = Data["best_participants"].ToString();
                        BestWins = Data["best_wins"].ToString();
                        BestKills = Data["best_kills"].ToString();
                        BestHeadshots = Data["best_headshots"].ToString();
                        Clan.BestPlayers.SetPlayers(BestEXP, BestParticipant, BestWins, BestKills, BestHeadshots);
                        Clans.Add(Clan);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Clans;
        }
        public static ClanModel GetClan(int id)
        {
            if (id == 0)
            {
                return new ClanModel();
            }
            lock (Clans)
            {
                for (int i = 0; i < Clans.Count; i++)
                {
                    ClanModel c = Clans[i];
                    if (c.Id == id)
                    {
                        return c;
                    }
                }
            }
            return new ClanModel();
        }
        public static List<Account> GetClanPlayers(int ClanId, long Exception, bool UseCache)
        {
            List<Account> Players = new List<Account>();
            if (ClanId == 0)
            {
                return Players;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.CommandText = "SELECT player_id, nickname, nick_color, rank, online, clan_access, clan_date, status FROM accounts WHERE clan_id=@clan";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        long PlayerId = long.Parse(Data["player_id"].ToString());
                        if (PlayerId == Exception)
                        {
                            continue;
                        }
                        Account Player = new Account()
                        {
                            PlayerId = PlayerId,
                            Nickname = Data["nickname"].ToString(),
                            NickColor = int.Parse(Data["nick_color"].ToString()),
                            Rank = int.Parse(Data["rank"].ToString()),
                            IsOnline = bool.Parse(Data["online"].ToString()),
                            ClanId = ClanId,
                            ClanAccess = int.Parse(Data["clan_access"].ToString()),
                            ClanDate = uint.Parse(Data["clan_date"].ToString())
                        };
                        Player.Status.SetData(uint.Parse(Data["status"].ToString()), Player.PlayerId);
                        if (UseCache)
                        {
                            Account Member = AccountManager.GetAccount(Player.PlayerId, true);
                            if (Member != null)
                            {
                                Player.Connection = Member.Connection;
                            }
                        }
                        Players.Add(Player);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Players;
        }
        public static List<Account> GetClanPlayers(int ClanId, long Exception, bool UseCache, bool IsOnline)
        {
            List<Account> Players = new List<Account>();
            if (ClanId == 0)
            {
                return Players;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.Parameters.AddWithValue("@on", IsOnline);
                    Command.CommandText = "SELECT player_id, nickname, nick_color, rank, clan_access, clan_date, status FROM accounts WHERE clan_id=@clan AND online=@on";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        long PlayerId = Data.GetInt64(0);
                        if (PlayerId == Exception)
                        {
                            continue;
                        }
                        Account Player = new Account()
                        {
                            PlayerId = PlayerId,
                            Nickname = Data["nickname"].ToString(),
                            NickColor = int.Parse(Data["nick_color"].ToString()),
                            Rank = int.Parse(Data["rank"].ToString()),
                            IsOnline = IsOnline,
                            ClanId = ClanId,
                            ClanAccess = int.Parse(Data["clan_access"].ToString()),
                            ClanDate = uint.Parse(Data["clan_date"].ToString())
                        };
                        Player.Status.SetData(uint.Parse(Data["status"].ToString()), Player.PlayerId);
                        if (UseCache)
                        {
                            Account Member = AccountManager.GetAccount(Player.PlayerId, true);
                            if (Member != null)
                            {
                                Player.Connection = Member.Connection;
                            }
                        }
                        Players.Add(Player);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Players;
        }
        public static void SendPacket(GameServerPacket Packet, List<Account> Players)
        {
            if (Players.Count == 0)
            {
                return;
            }

            byte[] Data = Packet.GetCompleteBytes("ClanManager.SendPacket");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name, false);
            }
        }
        public static void SendPacket(GameServerPacket Packet, List<Account> Players, long Exception)
        {
            if (Players.Count == 0)
            {
                return;
            }

            byte[] Data = Packet.GetCompleteBytes("ClanManager.SendPacket");
            foreach (Account Player in Players)
            {
                if (Player.PlayerId != Exception)
                {
                    Player.SendCompletePacket(Data, Packet.GetType().Name, false);
                }
            }
        }
        public static void SendPacket(GameServerPacket Packet, int ClanId, long Exception, bool UseCache, bool IsOnline)
        {
            SendPacket(Packet, GetClanPlayers(ClanId, Exception, UseCache, IsOnline));
        }
        public static bool RemoveClan(ClanModel clan)
        {
            lock (Clans)
            {
                return Clans.Remove(clan);
            }
        }
        public static void AddClan(ClanModel clan)
        {
            lock (Clans)
            {
                Clans.Add(clan);
            }
        }
        public static bool IsClanNameExist(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }
            try
            {
                int value = 0;
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@name", name);
                    command.CommandText = "SELECT COUNT(*) FROM system_clan WHERE name=@name";
                    value = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
                return value > 0;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return true;
            }
        }
        public static bool IsClanLogoExist(uint logo)
        {
            try
            {
                int value = 0;
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@logo", (long)logo);
                    command.CommandText = "SELECT COUNT(*) FROM system_clan WHERE logo=@logo";
                    value = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
                return value > 0;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return true;
            }
        }
    }
}
