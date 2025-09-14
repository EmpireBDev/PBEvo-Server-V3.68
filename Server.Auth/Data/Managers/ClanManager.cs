using Npgsql;
using Plugin.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Plugin.Core.SQL;
using Plugin.Core.Models;
using Server.Auth.Data.Models;
using Plugin.Core.Enums;

namespace Server.Auth.Data.Managers
{
    public class ClanManager
    {
        public static ClanModel GetClanDB(object Value, int Type)
        {
            ClanModel Clan = new ClanModel();
            if (Type == 1 && (int)Value <= 0 || Type == 0 && string.IsNullOrEmpty(Value.ToString()))
            {
                return Clan;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    string moreCmd = Type == 0 ? "name" : "id";
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@valor", Value);
                    Command.CommandText = "SELECT * FROM system_clan WHERE " + moreCmd + "=@valor";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Clan.Id = int.Parse(Data["id"].ToString());
                        Clan.Rank = byte.Parse(Data["rank"].ToString());
                        Clan.Name = Data["name"].ToString();
                        Clan.OwnerId = long.Parse(Data["owner_id"].ToString());
                        Clan.Logo = uint.Parse(Data["logo"].ToString());
                        Clan.NameColor = byte.Parse(Data["name_color"].ToString());
                        Clan.Effect = byte.Parse(Data["effects"].ToString());
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
                return Clan.Id == 0 ? new ClanModel() : Clan;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return new ClanModel();
            }
        }
        public static List<Account> GetClanPlayers(int ClanId, long Exception)
        {
            List<Account> Friends = new List<Account>();
            if (ClanId <= 0)
            {
                return Friends;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.CommandText = "SELECT player_id, nickname, rank, online, status FROM accounts WHERE clan_id=@clan";
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
                            Rank = byte.Parse(Data["rank"].ToString()),
                            IsOnline = bool.Parse(Data["online"].ToString())
                        };
                        Player.Status.SetData(uint.Parse(Data["status"].ToString()), PlayerId);
                        if (Player.IsOnline && !AccountManager.Accounts.ContainsKey(PlayerId))
                        {
                            Player.SetOnlineStatus(false);
                            Player.Status.ResetData(Player.PlayerId);
                        }
                        Friends.Add(Player);
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
            return Friends;
        }
        public static List<Account> GetClanPlayers(int ClanId, long Exception, bool IsOnline)
        {
            List<Account> Friends = new List<Account>();
            if (ClanId <= 0)
            {
                return Friends;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.Parameters.AddWithValue("@on", IsOnline);
                    Command.CommandText = "SELECT player_id, nickname, rank, online, status FROM accounts WHERE clan_id=@clan AND online=@on";
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
                            Rank = byte.Parse(Data["rank"].ToString()),
                            IsOnline = bool.Parse(Data["online"].ToString())
                        };
                        Player.Status.SetData(uint.Parse(Data["status"].ToString()), PlayerId);
                        if (Player.IsOnline && !AccountManager.Accounts.ContainsKey(PlayerId))
                        {
                            Player.SetOnlineStatus(false);
                            Player.Status.ResetData(Player.PlayerId);
                        }
                        Friends.Add(Player);
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
            return Friends;
        }
    }
}
