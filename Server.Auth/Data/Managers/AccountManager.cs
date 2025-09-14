using Plugin.Core;
using Plugin.Core.Enums;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Plugin.Core.SQL;
using Plugin.Core.Models;
using Server.Auth.Data.Models;

namespace Server.Auth.Data.Managers
{
    public class AccountManager
    {
        public static SortedList<long, Account> Accounts = new SortedList<long, Account>();
        public static bool AddAccount(Account acc)
        {
            lock (Accounts)
            {
                if (!Accounts.ContainsKey(acc.PlayerId))
                {
                    Accounts.Add(acc.PlayerId, acc);
                    return true;
                }
            }
            return false;
        }
        public static Account GetAccountDB(object valor, object valor2, int type, int searchFlag)
        {
            if (type == 0 && (string)valor == "" || type == 1 && (long)valor == 0 || type == 2 && (string.IsNullOrEmpty((string)valor) || string.IsNullOrEmpty((string)valor2)))
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
                    command.Parameters.AddWithValue("@valor", valor);
                    if (type == 0)
                    {
                        command.CommandText = "SELECT * FROM accounts WHERE username=@valor LIMIT 1";
                    }
                    else if (type == 1)
                    {
                        command.CommandText = "SELECT * FROM accounts WHERE player_id=@valor LIMIT 1";
                    }
                    else if (type == 2)
                    {
                        command.Parameters.AddWithValue("@valor2", valor2);
                        command.CommandText = "SELECT * FROM accounts WHERE username=@valor AND password=@valor2 LIMIT 1";
                    }
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        Player = new Account()
                        {
                            Username = Data["username"].ToString(),
                            Password = Data["password"].ToString()
                        };
                        Player.SetPlayerId(long.Parse(Data["player_id"].ToString()), searchFlag);
                        Player.Email = Data["email"].ToString();
                        Player.Age = int.Parse(Data["age"].ToString());
                        Player.MacAddress = (PhysicalAddress)Data.GetValue(6);
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
                        if (AddAccount(Player) && Player.IsOnline)
                        {
                            Player.SetOnlineStatus(false);
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
                CLogger.Print($"was a problem loading accounts!\r\n{ex.Message}", LoggerType.Error, ex);
            }
            return Player;
        }
        public static void GetFriendlyAccounts(PlayerFriends system)
        {
            if (system == null || system.Friends.Count == 0)
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
                    for (int idx = 0; idx < system.Friends.Count; idx++)
                    {
                        FriendModel friend = system.Friends[idx];
                        string param = "@valor" + idx;
                        command.Parameters.AddWithValue(param, friend.PlayerId);
                        parameters.Add(param);
                    }
                    loaded = string.Join(",", parameters.ToArray());
                    command.CommandText = "SELECT nickname, player_id, rank, online, status FROM accounts WHERE player_id in (" + loaded + ") ORDER BY player_id";
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        FriendModel Friend = system.GetFriend(long.Parse(Data["player_id"].ToString()));
                        if (Friend != null)
                        {
                            Friend.Info.Nickname = Data["nickname"].ToString();
                            Friend.Info.Rank = int.Parse(Data["rank"].ToString());
                            Friend.Info.IsOnline = bool.Parse(Data["online"].ToString());
                            Friend.Info.Status.SetData(uint.Parse(Data["status"].ToString()), Friend.PlayerId);
                            if (Friend.Info.IsOnline && !Accounts.ContainsKey(Friend.PlayerId))
                            {
                                Friend.Info.SetOnlineStatus(false);
                                Friend.Info.Status.ResetData(Friend.PlayerId);
                            }
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
                CLogger.Print($"was a problem loading (FriendAccounts)!\r\n{ex.Message}", LoggerType.Error, ex);
            }
        }
        public static void GetFriendlyAccounts(PlayerFriends System, bool isOnline)
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
                    string loaded = "";
                    List<string> parameters = new List<string>();
                    for (int idx = 0; idx < System.Friends.Count; idx++)
                    {
                        FriendModel friend = System.Friends[idx];
                        if (friend.State > 0)
                        {
                            return;
                        }
                        string param = "@valor" + idx;
                        command.Parameters.AddWithValue(param, friend.PlayerId);
                        parameters.Add(param);
                    }
                    loaded = string.Join(",", parameters.ToArray());
                    if (loaded == "")
                    {
                        return;
                    }
                    connection.Open();
                    command.Parameters.AddWithValue("@on", isOnline);
                    command.CommandText = "SELECT nickname, player_id, rank, status FROM accounts WHERE player_id in (" + loaded + ") AND online=@on ORDER BY player_id";
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        FriendModel Friend = System.GetFriend(long.Parse(Data["player_id"].ToString()));
                        if (Friend != null)
                        {
                            Friend.Info.Nickname = Data["nickname"].ToString();
                            Friend.Info.Rank = int.Parse(Data["rank"].ToString());
                            Friend.Info.IsOnline = isOnline;
                            Friend.Info.Status.SetData(uint.Parse(Data["status"].ToString()), Friend.PlayerId);
                            if (isOnline && !Accounts.ContainsKey(Friend.PlayerId))
                            {
                                Friend.Info.SetOnlineStatus(false);
                                Friend.Info.Status.ResetData(Friend.PlayerId);
                            }
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
                CLogger.Print($"was a problem loading (FriendAccounts2)!\r\n{ex.Message}", LoggerType.Error, ex);
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
                return Accounts.TryGetValue(id, out Account p) ? p : GetAccountDB(id, null, 1, searchFlag);
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
                return Accounts.TryGetValue(id, out Account p) ? p : (noUseDB ? null : GetAccountDB(id, null, 1, 31));
            }
            catch
            {
                return null;
            }
        }
        public static bool CreateAccount(out Account Player, string Username, string Password)
        {
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@login", Username);
                    command.Parameters.AddWithValue("@pass", Password);
                    command.CommandText = "INSERT INTO accounts (username, password) VALUES (@login, @pass)";
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT * FROM accounts WHERE username=@login";
                    NpgsqlDataReader Data = command.ExecuteReader();
                    Account Temp = new Account();
                    while (Data.Read())
                    {
                        Temp.Username = Data["username"].ToString();
                        Temp.Password = Data["password"].ToString();
                        Temp.SetPlayerId(long.Parse(Data["player_id"].ToString()), 95);
                        Temp.Email = Data["email"].ToString();
                        Temp.Age = int.Parse(Data["age"].ToString());
                        Temp.MacAddress = (PhysicalAddress)Data.GetValue(6);
                        Temp.Nickname = Data["nickname"].ToString();
                        Temp.NickColor = int.Parse(Data["nick_color"].ToString());
                        Temp.Rank = int.Parse(Data["rank"].ToString());
                        Temp.Exp = int.Parse(Data["experience"].ToString());
                        Temp.Gold = int.Parse(Data["gold"].ToString());
                        Temp.Cash = int.Parse(Data["cash"].ToString());
                        Temp.CafePC = (CafeEnum)int.Parse(Data["pc_cafe"].ToString());
                        Temp.Access = (AccessLevel)int.Parse(Data["access_level"].ToString());
                        Temp.IsOnline = bool.Parse(Data["online"].ToString());
                        Temp.TourneyLevel = int.Parse(Data["tourney_level"].ToString());
                        Temp.ClanId = int.Parse(Data["clan_id"].ToString());
                        Temp.ClanAccess = int.Parse(Data["clan_access"].ToString());
                        Temp.Effects = (CouponEffects)long.Parse(Data["coupon_effect"].ToString());
                        Temp.Status.SetData(uint.Parse(Data["status"].ToString()), Temp.PlayerId);
                        Temp.LastRankUpDate = uint.Parse(Data["last_rank_update"].ToString());
                        Temp.BanObjectId = long.Parse(Data["ban_object_id"].ToString());
                        Temp.Ribbon = int.Parse(Data["ribbon"].ToString());
                        Temp.Ensign = int.Parse(Data["ensign"].ToString());
                        Temp.Medal = int.Parse(Data["medal"].ToString());
                        Temp.MasterMedal = int.Parse(Data["master_medal"].ToString());
                        Temp.Mission.Mission1 = int.Parse(Data["mission_id1"].ToString());
                        Temp.Mission.Mission2 = int.Parse(Data["mission_id2"].ToString());
                        Temp.Mission.Mission3 = int.Parse(Data["mission_id3"].ToString());
                        Temp.Tags = int.Parse(Data["tags"].ToString());
                        Temp.InventoryPlus = int.Parse(Data["inventory_plus"].ToString());
                    }
                    Player = Temp;
                    AddAccount(Temp);
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AccountManager.CreateAccount] {ex.Message}", LoggerType.Error, ex);
                Player = null;
                return false;
            }
        }
    }
}