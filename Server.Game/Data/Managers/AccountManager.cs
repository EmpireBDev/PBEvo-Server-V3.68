using Plugin.Core;
using Plugin.Core.Enums;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using Plugin.Core.Utility;
using Plugin.Core.SQL;
using Plugin.Core.Models;
using Server.Game.Data.Models;

namespace Server.Game.Data.Managers
{
    public static class AccountManager
    {
        public static SortedList<long, Account> Accounts = new SortedList<long, Account>();
        public static void AddAccount(Account acc)
        {
            lock (Accounts)
            {
                if (!Accounts.ContainsKey(acc.PlayerId))
                {
                    Accounts.Add(acc.PlayerId, acc);
                }
            }
        }
        public static Account GetAccountDB(object valor, int type, int searchDBFlag)
        {
            if (type == 2 && (long)valor == 0 || (type == 0 || type == 1) && (string)valor == "")
            {

                return null;
            }
            Account Player = null;
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@value", valor);
                    command.CommandText = "SELECT * FROM accounts WHERE " + (type == 0 ? "username" : type == 1 ? "nickname" : "player_id") + "=@value";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        Player = new Account()
                        {
                            Username = Data["username"].ToString(),
                            Password = Data["password"].ToString()
                        };
                        Player.SetPlayerId(long.Parse(Data["player_id"].ToString()), searchDBFlag);
                        Player.Email = Data["email"].ToString();
                        Player.Age = int.Parse(Data["age"].ToString());
                        Player.SetPublicIP(Data.GetString(5));
                        Player.Nickname = Data["nickname"].ToString();
                        Player.NickColor = int.Parse(Data["nick_color"].ToString());
                        Player.Rank = int.Parse(Data["rank"].ToString());
                        Player.Exp = int.Parse(Data["experience"].ToString());
                        Player.Gold = int.Parse(Data["gold"].ToString());
                        Player.Cash = int.Parse(Data["cash"].ToString());
                        Player.CafePC = (CafeEnum)int.Parse(Data["pc_cafe"].ToString());
                        Player.Access = (AccessLevel)int.Parse(Data["access_level"].ToString());
                        Player.IsOnline = bool.Parse(Data["online"].ToString());
                        Player.TourneyLevel = int.Parse(Data["tourney_level"].ToString());
                        Player.ClanId = int.Parse(Data["clan_id"].ToString());
                        Player.ClanAccess = int.Parse(Data["clan_access"].ToString());
                        Player.Effects = (CouponEffects)long.Parse(Data["coupon_effect"].ToString());
                        Player.Status.SetData(uint.Parse(Data["status"].ToString()), Player.PlayerId);
                        Player.LastRankUpDate = uint.Parse(Data["last_rank_update"].ToString());
                        Player.BanObjectId = long.Parse(Data["ban_object_id"].ToString());
                        Player.Ribbon = int.Parse(Data["ribbon"].ToString());
                        Player.Ensign = int.Parse(Data["ensign"].ToString());
                        Player.Medal = int.Parse(Data["medal"].ToString());
                        Player.MasterMedal = int.Parse(Data["master_medal"].ToString());
                        Player.Mission.Mission1 = int.Parse(Data["mission_id1"].ToString());
                        Player.Mission.Mission2 = int.Parse(Data["mission_id2"].ToString());
                        Player.Mission.Mission3 = int.Parse(Data["mission_id3"].ToString());
                        Player.Tags = int.Parse(Data["tags"].ToString());
                        Player.InventoryPlus = int.Parse(Data["inventory_plus"].ToString());
                        AddAccount(Player);
                    }
                    command.Dispose();
                    Data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"was a problem loading accounts!\r\n{ex.Message}", LoggerType.Error, ex);
            }
            return Player;
        }
        public static void GetFriendlyAccounts(PlayerFriends System)
        {
            if (System == null || System.Friends.Count == 0)
            {
                return;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    string loaded = "";
                    List<string> parameters = new List<string>();
                    for (int idx = 0; idx < System.Friends.Count; idx++)
                    {
                        FriendModel friend = System.Friends[idx];
                        string param = "@valor" + idx;
                        command.Parameters.AddWithValue(param, friend.PlayerId);
                        parameters.Add(param);
                    }
                    loaded = string.Join(",", parameters.ToArray());
                    command.CommandText = "SELECT nickname, player_id, rank, online, status FROM accounts WHERE player_id in (" + loaded + ") ORDER BY player_id";
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        FriendModel Friend = System.GetFriend(long.Parse(Data["player_id"].ToString()));
                        if (Friend != null)
                        {
                            Friend.Info.Nickname = Data["nickname"].ToString();
                            Friend.Info.Rank = int.Parse(Data["rank"].ToString());
                            Friend.Info.IsOnline = bool.Parse(Data["online"].ToString());
                            Friend.Info.Status.SetData(uint.Parse(Data["status"].ToString()), Friend.PlayerId);
                        }
                    }
                    command.Dispose();
                    Data.Dispose();
                    Data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"was a problem loading (FriendlyAccounts); {ex.Message}", LoggerType.Error, ex);
            }
        }
        public static Account GetAccount(long id, int searchFlag)
        {
            if (id == 0)
            {
                return null;
            }
            try
            {
                return Accounts.TryGetValue(id, out Account Player) ? Player : GetAccountDB(id, 2, searchFlag);
            }
            catch
            {
                return null;
            }
        }
        public static Account GetAccount(long id, bool noUseDB)
        {
            if (id == 0)
            {
                return null;
            }
            try
            {
                return Accounts.TryGetValue(id, out Account Player) ? Player : (noUseDB ? null : GetAccountDB(id, 2, 287));
            }
            catch
            {
                return null;
            }
        }
        public static Account GetAccount(string text, int type, int searchFlag)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            IList<Account> List = Accounts.Values;
            foreach (Account Player in List)
            {
                if (Player != null && (type == 1 && Player.Nickname == text && Player.Nickname.Length > 0 || type == 0 && string.Compare(Player.Username, text) == 0))
                {
                    return Player;
                }
            }
            return GetAccountDB(text, type, searchFlag);
        }
        public static bool UpdatePlayerName(string name, long playerId)
        {
            return ComDiv.UpdateDB("accounts", "nickname", name, "player_id", playerId);
        }
    }
}