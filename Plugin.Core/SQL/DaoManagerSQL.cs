using Npgsql;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace Plugin.Core.SQL
{
    public static class DaoManagerSQL
    {
        public static List<ItemsModel> GetPlayerInventoryItems(long OwnerId)
        {
            List<ItemsModel> Items = new List<ItemsModel>();
            if (OwnerId == 0)
            {
                return Items;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT * FROM player_items WHERE owner_id=@owner ORDER BY object_id ASC;";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        ItemsModel Item = new ItemsModel(int.Parse(Data["id"].ToString()))
                        {
                            ObjectId = long.Parse(Data["object_id"].ToString()),
                            Name = Data["name"].ToString(),
                            Count = uint.Parse(Data["count"].ToString()),
                            Equip = (ItemEquipType)int.Parse(Data["equip"].ToString())
                        };
                        Items.Add(Item);
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
            return Items;
        }
        public static bool CreatePlayerInventoryItem(ItemsModel Item, long OwnerId)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@itmId", Item.Id);
                    Command.Parameters.AddWithValue("@ItmNm", Item.Name);
                    Command.Parameters.AddWithValue("@count", (long)Item.Count);
                    Command.Parameters.AddWithValue("@equip", (int)Item.Equip);
                    Command.CommandText = "INSERT INTO player_items(owner_id, id, name, count, equip) VALUES(@owner, @itmId, @ItmNm, @count, @equip) RETURNING object_id";
                    object Data = Command.ExecuteScalar();
                    Item.ObjectId = Item.Equip != ItemEquipType.Permanent ? (long)Data : Item.ObjectId;
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static bool DeletePlayerInventoryItem(long ObjectId, long OwnerId)
        {
            if (ObjectId == 0 || OwnerId == 0)
            {
                return false;
            }
            return ComDiv.DeleteDB("player_items", "object_id", ObjectId, "owner_id", OwnerId);
        }
        public static BanHistory GetAccountBan(long ObjectId)
        {
            BanHistory Ban = new BanHistory();
            if (ObjectId == 0)
            {
                return Ban;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@obj", ObjectId);
                    Command.CommandText = "SELECT * FROM base_ban_history WHERE object_id=@obj";
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Ban.ObjectId = long.Parse(Data["object_id"].ToString());
                        Ban.PlayerId = long.Parse(Data["owner_id"].ToString());
                        Ban.Type = Data["type"].ToString();
                        Ban.Value = Data["value"].ToString();
                        Ban.Reason = Data["reason"].ToString();
                        Ban.StartDate = DateTime.Parse(Data["start_date"].ToString());
                        Ban.EndDate = DateTime.Parse(Data["expire_date"].ToString());
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
                return null;
            }
            return Ban;
        }
        public static List<string> GetHwIdList()
        {
            List<string> HwIds = new List<string>();
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM base_ban_hwid";
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        string HwId = Data["hardware_id"].ToString();
                        if (HwId != null || HwId.Length != 0)
                        {
                            HwIds.Add(HwId);
                        }
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return null;
            }
            return HwIds;
        }
        public static void GetBanStatus(string MAC, string IP4, out bool ValidMac, out bool ValidIp4)
        {
            ValidMac = false;
            ValidIp4 = false;
            try
            {
                DateTime Now = DateTimeUtil.Now();
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@mac", MAC);
                    Command.Parameters.AddWithValue("@ip", IP4);
                    Command.CommandText = "SELECT * FROM base_ban_history WHERE value in (@mac, @ip)";
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        string Type = Data["type"].ToString();
                        string Value = Data["value"].ToString();
                        DateTime EndDate = DateTime.Parse(Data["expire_date"].ToString());
                        if (EndDate < Now)
                        {
                            continue;
                        }
                        if (Type == "MAC" && Value == MAC)
                        {
                            ValidMac = true;
                        }
                        else if (Type == "IP4" && Value == IP4)
                        {
                            ValidIp4 = true;
                        }
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
        }
        public static BanHistory SaveBanHistory(long PlayerId, string Type, string Value, DateTime EndDate)
        {
            BanHistory Ban = new BanHistory()
            {
                PlayerId = PlayerId,
                Type = Type,
                Value = Value,
                EndDate = EndDate
            };
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@provider", Ban.PlayerId);
                    Command.Parameters.AddWithValue("@type", Ban.Type);
                    Command.Parameters.AddWithValue("@value", Ban.Value);
                    Command.Parameters.AddWithValue("@reason", Ban.Reason);
                    Command.Parameters.AddWithValue("@start", Ban.StartDate);
                    Command.Parameters.AddWithValue("@end", Ban.EndDate);
                    Command.CommandText = "INSERT INTO base_ban_history(player_id, type, value, reason, start_date, expire_date) VALUES(@provider, @type, @value, @reason, @start, @end) RETURNING object_id";
                    object data = Command.ExecuteScalar();
                    Ban.ObjectId = (long)data;
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                    return Ban;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return null;
            }
        }
        public static void SaveAutoBan(long player_id, string login, string player_name, string type, string time, string ip, string hack_type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@player_id", player_id);
                    Command.Parameters.AddWithValue("@login", login);
                    Command.Parameters.AddWithValue("@player_name", player_name);
                    Command.Parameters.AddWithValue("@type", type);
                    Command.Parameters.AddWithValue("@time", time);
                    Command.Parameters.AddWithValue("@ip", ip);
                    Command.Parameters.AddWithValue("@hack_type", hack_type);
                    Command.CommandText = "INSERT INTO base_auto_ban(player_id, username, nickname, type, time, ip4, hack_type) VALUES(@player_id, @login, @player_name, @type, @time, @ip, @hack_type)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
        }
        public static bool SaveBanReason(long ObjectId, string Reason)
        {
            return ComDiv.UpdateDB("base_ban_history", "reason", Reason, "object_id", ObjectId);
        }
        public static bool CreateClan(out int ClanId, string Name, long OwnerId, string ClanInfo, uint CreateDate)
        {
            bool Result;
            try
            {
                ClanId = -1;
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@name", Name);
                    Command.Parameters.AddWithValue("@date", (long)CreateDate);
                    Command.Parameters.AddWithValue("@info", ClanInfo);
                    Command.Parameters.AddWithValue("@best", "0-0");
                    Command.CommandText = "INSERT INTO system_clan (name, owner_id, create_date, info, best_exp, best_participants, best_wins, best_kills, best_headshots) VALUES (@name, @owner, @date, @info, @best, @best, @best, @best, @best) RETURNING id";
                    object Object = Command.ExecuteScalar();
                    ClanId = (int)Object;
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                Result = true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                ClanId = -1;
                Result = false;
            }
            return Result;
        }
        public static bool UpdateClanInfo(int ClanId, int Authority, int RankLimit, int AgeLimitStart, int AgeLimitEnd)
        {
            if (ClanId == 0)
            {
                return false;
            }
            bool result;
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.Parameters.AddWithValue("@autoridade", Authority);
                    Command.Parameters.AddWithValue("@limite_rank", RankLimit);
                    Command.Parameters.AddWithValue("@limite_idade", AgeLimitStart);
                    Command.Parameters.AddWithValue("@limite_idade2", AgeLimitEnd);
                    Command.CommandText = "UPDATE system_clan SET authority=@autoridade, rank_limit=@limite_rank, age_limit_start=@limite_idade, age_limit_end=@limite_idade2 WHERE id=@clan";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                result = true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                result = false;
            }
            return result;
        }
        public static void UpdateClanBestPlayers(ClanModel Clan)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", Clan.Id);
                    Command.Parameters.AddWithValue("@bp1", Clan.BestPlayers.Exp.GetSplit());
                    Command.Parameters.AddWithValue("@bp2", Clan.BestPlayers.Participation.GetSplit());
                    Command.Parameters.AddWithValue("@bp3", Clan.BestPlayers.Wins.GetSplit());
                    Command.Parameters.AddWithValue("@bp4", Clan.BestPlayers.Kills.GetSplit());
                    Command.Parameters.AddWithValue("@bp5", Clan.BestPlayers.Headshots.GetSplit());
                    Command.CommandType = CommandType.Text;
                    Command.CommandText = "UPDATE system_clan SET best_exp=@bp1, best_participants=@bp2, best_wins=@bp3, best_kills=@bp4, best_headshots=@bp5 WHERE id=@id";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
        }
        public static bool UpdateClanLogo(int ClanId, uint logo)
        {
            return ClanId != 0 && ComDiv.UpdateDB("system_clan", "logo", (long)logo, "id", ClanId);
        }
        public static bool UpdateClanPoints(int ClanId, float Gold)
        {
            return ClanId != 0 && ComDiv.UpdateDB("system_clan", "gold", Gold, "id", ClanId);
        }
        public static bool UpdateClanExp(int ClanId, int Exp)
        {
            return ClanId != 0 && ComDiv.UpdateDB("system_clan", "exp", Exp, "id", ClanId);
        }
        public static bool UpdateClanRank(int ClanId, int Rank)
        {
            return ClanId != 0 && ComDiv.UpdateDB("system_clan", "rank", Rank, "id", ClanId);
        }
        public static bool UpdateClanBattles(int ClanId, int Matches, int Wins, int Loses)
        {
            if (ClanId == 0)
            {
                return false;
            }
            bool result;
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.Parameters.AddWithValue("@partidas", Matches);
                    Command.Parameters.AddWithValue("@vitorias", Wins);
                    Command.Parameters.AddWithValue("@derrotas", Loses);
                    Command.CommandText = "UPDATE system_clan SET matches=@partidas, match_wins=@vitorias, match_loses=@derrotas WHERE id=@clan";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                result = true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                result = false;
            }
            return result;
        }
        public static int GetClanPlayers(int ClanId)
        {
            int Result = 0;
            if (ClanId == 0)
            {
                return Result;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.CommandText = "SELECT COUNT(*) FROM accounts WHERE clan_id=@clan";
                    Result = Convert.ToInt32(Command.ExecuteScalar());
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Result;
        }
        public static MessageModel GetMessage(long ObjectId, long PlayerId)
        {
            MessageModel Message = null;
            if (ObjectId == 0 || PlayerId == 0)
            {
                return Message;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@obj", ObjectId);
                    Command.Parameters.AddWithValue("@owner", PlayerId);
                    Command.CommandText = "SELECT * FROM player_messages WHERE object_id=@obj AND owner_id=@owner";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Message = new MessageModel(uint.Parse(Data["expire_date"].ToString()), DateTimeUtil.Now())
                        {
                            ObjectId = ObjectId,
                            SenderId = long.Parse(Data["sender_id"].ToString()),
                            SenderName = Data["sender_name"].ToString(),
                            ClanId = int.Parse(Data["clan_id"].ToString()),
                            ClanNote = (NoteMessageClan)int.Parse(Data["clan_note"].ToString()),
                            Text = Data["text"].ToString(),
                            Type = (NoteMessageType)int.Parse(Data["type"].ToString()),
                            State = (NoteMessageState)int.Parse(Data["state"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return null;
            }
            return Message;
        }
        public static List<MessageModel> GetGiftMessages(long OwnerId)
        {
            List<MessageModel> Messages = new List<MessageModel>();
            if (OwnerId == 0)
            {
                return Messages;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT * FROM player_messages WHERE owner_id=@owner";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        NoteMessageType Type = (NoteMessageType)int.Parse(Data["type"].ToString());
                        if (Type != NoteMessageType.Gift)
                        {
                            continue;
                        }
                        MessageModel msg = new MessageModel(uint.Parse(Data["expire_date"].ToString()), DateTimeUtil.Now())
                        {
                            ObjectId = long.Parse(Data["object_id"].ToString()),
                            SenderId = long.Parse(Data["sender_id"].ToString()),
                            SenderName = Data["sender_name"].ToString(),
                            ClanId = int.Parse(Data["clan_id"].ToString()),
                            ClanNote = (NoteMessageClan)int.Parse(Data["clan_note"].ToString()),
                            Text = Data["text"].ToString(),
                            Type = Type,
                            State = (NoteMessageState)int.Parse(Data["state"].ToString()),
                        };
                        Messages.Add(msg);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Messages;
        }
        public static List<MessageModel> GetMessages(long OwnerId)
        {
            List<MessageModel> Messages = new List<MessageModel>();
            if (OwnerId == 0)
            {
                return Messages;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT * FROM player_messages WHERE owner_id=@owner";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        NoteMessageType Type = (NoteMessageType)int.Parse(Data["type"].ToString());
                        if (Type == NoteMessageType.Gift)
                        {
                            continue;
                        }
                        MessageModel msg = new MessageModel(uint.Parse(Data["expire_date"].ToString()), DateTimeUtil.Now())
                        {
                            ObjectId = long.Parse(Data["object_id"].ToString()),
                            SenderId = long.Parse(Data["sender_id"].ToString()),
                            SenderName = Data["sender_name"].ToString(),
                            ClanId = int.Parse(Data["clan_id"].ToString()),
                            ClanNote = (NoteMessageClan)int.Parse(Data["clan_note"].ToString()),
                            Text = Data["text"].ToString(),
                            Type = Type,
                            State = (NoteMessageState)int.Parse(Data["state"].ToString())
                        };
                        Messages.Add(msg);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Messages;
        }
        public static bool MessageExists(long ObjectId, long OwnerId)
        {
            if (ObjectId == 0 || OwnerId == 0)
            {
                return false;
            }
            try
            {
                int MessageCount = 0;
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@obj", ObjectId);
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT COUNT(*) FROM player_messages WHERE object_id=@obj AND owner_id=@owner";
                    MessageCount = Convert.ToInt32(Command.ExecuteScalar());
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return MessageCount > 0;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return false;
        }
        public static int GetMessagesCount(long OwnerId)
        {
            int Messages = 0;
            if (OwnerId == 0)
            {
                return Messages;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT COUNT(*) FROM player_messages WHERE owner_id=@owner";
                    Messages = Convert.ToInt32(Command.ExecuteScalar());
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Messages;
        }
        public static bool CreateMessage(long OwnerId, MessageModel Message)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@sendid", Message.SenderId);
                    Command.Parameters.AddWithValue("@clan", Message.ClanId);
                    Command.Parameters.AddWithValue("@sendname", Message.SenderName);
                    Command.Parameters.AddWithValue("@text", Message.Text);
                    Command.Parameters.AddWithValue("@type", (int)Message.Type);
                    Command.Parameters.AddWithValue("@state", (int)Message.State);
                    Command.Parameters.AddWithValue("@expire", Message.ExpireDate);
                    Command.Parameters.AddWithValue("@cb", (int)Message.ClanNote);
                    Command.CommandType = CommandType.Text;
                    Command.CommandText = "INSERT INTO player_messages(owner_id, sender_id, sender_name, clan_id, clan_note, text, type, state, expire_date)VALUES(@owner, @sendid, @sendname, @clan, @cb, @text, @type, @state, @expire) RETURNING object_id";
                    object data = Command.ExecuteScalar();
                    Message.ObjectId = (long)data;
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static void UpdateState(long ObjectId, long OwnerId, int Value)
        {
            ComDiv.UpdateDB("player_messages", "state", Value, "object_id", ObjectId, "owner_id", OwnerId);
        }
        public static void UpdateExpireDate(long ObjectId, long OwnerId, uint Date)
        {
            ComDiv.UpdateDB("player_messages", "expire_date", (long)Date, "object_id", ObjectId, "owner_id", OwnerId);
        }
        public static bool DeleteMessage(long ObjectId, long OwnerId)
        {
            if (ObjectId == 0 || OwnerId == 0)
            {
                return false;
            }
            return ComDiv.DeleteDB("player_messages", "object_id", ObjectId, "owner_id", OwnerId);
        }
        public static bool DeleteMessages(List<object> ObjectIds, long OwnerId)
        {
            if (ObjectIds.Count == 0 || OwnerId == 0)
            {
                return false;
            }
            return ComDiv.DeleteDB("player_messages", "object_id", ObjectIds.ToArray(), "owner_id", OwnerId);
        }
        public static void RecycleMessages(long OwnerId, List<MessageModel> Messages)
        {
            List<object> ObjectIds = new List<object>();
            for (int i = 0; i < Messages.Count; i++)
            {
                MessageModel Message = Messages[i];
                if (Message.DaysRemaining == 0)
                {
                    ObjectIds.Add(Message.ObjectId);
                    Messages.RemoveAt(i--);
                }
            }
            DeleteMessages(ObjectIds, OwnerId);
        }
        public static PlayerEquipment GetPlayerEquipmentsDB(long OwnerId)
        {
            PlayerEquipment Equipment = null;
            if (OwnerId == 0)
            {
                return Equipment;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_equipments WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Equipment = new PlayerEquipment()
                        {
                            OwnerId = OwnerId,
                            WeaponPrimary = int.Parse(Data["weapon_primary"].ToString()),
                            WeaponSecondary = int.Parse(Data["weapon_secondary"].ToString()),
                            WeaponMelee = int.Parse(Data["weapon_melee"].ToString()),
                            WeaponExplosive = int.Parse(Data["weapon_explosive"].ToString()),
                            WeaponSpecial = int.Parse(Data["weapon_special"].ToString()),
                            CharaRedId = int.Parse(Data["chara_red_side"].ToString()),
                            CharaBlueId = int.Parse(Data["chara_blue_side"].ToString()),
                            DinoItem = int.Parse(Data["dino_item_chara"].ToString()),
                            PartHead = int.Parse(Data["part_head"].ToString()),
                            PartFace = int.Parse(Data["part_face"].ToString()),
                            PartJacket = int.Parse(Data["part_jacket"].ToString()),
                            PartPocket = int.Parse(Data["part_pocket"].ToString()),
                            PartGlove = int.Parse(Data["part_glove"].ToString()),
                            PartBelt = int.Parse(Data["part_belt"].ToString()),
                            PartHolster = int.Parse(Data["part_holster"].ToString()),
                            PartSkin = int.Parse(Data["part_skin"].ToString()),
                            BeretItem = int.Parse(Data["beret_item_part"].ToString()),
                            AccessoryId = int.Parse(Data["accesory_id"].ToString()),
                            SprayId = int.Parse(Data["spray_id"].ToString()),
                            NameCardId = int.Parse(Data["namecard_id"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Equipment;
        }
        public static bool CreatePlayerEquipmentsDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_equipments(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static List<CharacterModel> GetPlayerCharactersDB(long OwnerId)
        {
            List<CharacterModel> Characters = new List<CharacterModel>();
            if (OwnerId == 0)
            {
                return Characters;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@OwnerId", OwnerId);
                    Command.CommandText = "SELECT * FROM player_characters WHERE owner_id=@OwnerId ORDER BY slot ASC;";
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        CharacterModel Character = new CharacterModel()
                        {
                            ObjectId = long.Parse(Data["object_id"].ToString()),
                            Id = int.Parse(Data["id"].ToString()),
                            Slot = int.Parse(Data["slot"].ToString()),
                            Name = Data["name"].ToString(),
                            CreateDate = uint.Parse(Data["create_date"].ToString()),
                            PlayTime = uint.Parse(Data["playtime"].ToString())
                        };
                        Characters.Add(Character);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Characters;
        }
        public static bool CreatePlayerCharacter(CharacterModel Chara, long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner_id", OwnerId);
                    Command.Parameters.AddWithValue("@id", Chara.Id);
                    Command.Parameters.AddWithValue("@slot", Chara.Slot);
                    Command.Parameters.AddWithValue("@name", Chara.Name);
                    Command.Parameters.AddWithValue("@createdate", (long)Chara.CreateDate);
                    Command.Parameters.AddWithValue("@playtime", (long)Chara.PlayTime);
                    Command.CommandType = CommandType.Text;
                    Command.CommandText = "INSERT INTO player_characters(owner_id, id, slot, name, create_date, playtime) VALUES(@owner_id, @id, @slot, @name, @createdate, @playtime) RETURNING object_id";
                    object Data = Command.ExecuteScalar();
                    Chara.ObjectId = (long)Data;
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static PlayerStatistic GetPlayerStatisticDB(long OwnerId)
        {
            PlayerStatistic Statistic = null;
            if (OwnerId == 0)
            {
                return Statistic;
            }
            try
            {

            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Statistic;
        }
        public static StatBasic GetPlayerStatBasicDB(long OwnerId)
        {
            StatBasic Total = null;
            if (OwnerId == 0)
            {
                return Total;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_basics WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Total = new StatBasic()
                        {
                            OwnerId = OwnerId,
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            MatchDraws = int.Parse(Data["match_draws"].ToString()),
                            KillsCount = int.Parse(Data["kills_count"].ToString()),
                            DeathsCount = int.Parse(Data["deaths_count"].ToString()),
                            HeadshotsCount = int.Parse(Data["headshots_count"].ToString()),
                            AssistsCount = int.Parse(Data["assists_count"].ToString()),
                            EscapesCount = int.Parse(Data["escapes_count"].ToString()),
                            MvpCount = int.Parse(Data["mvp_count"].ToString()),
                            TotalMatchesCount = int.Parse(Data["total_matches"].ToString()),
                            TotalKillsCount = int.Parse(Data["total_kills"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Total;
        }
        public static bool CreatePlayerStatBasicDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_basics(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static StatSeason GetPlayerStatSeasonDB(long OwnerId)
        {
            StatSeason Season = null;
            if (OwnerId == 0)
            {
                return Season;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_seasons WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Season = new StatSeason()
                        {
                            OwnerId = OwnerId,
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            MatchDraws = int.Parse(Data["match_draws"].ToString()),
                            KillsCount = int.Parse(Data["kills_count"].ToString()),
                            DeathsCount = int.Parse(Data["deaths_count"].ToString()),
                            HeadshotsCount = int.Parse(Data["headshots_count"].ToString()),
                            AssistsCount = int.Parse(Data["assists_count"].ToString()),
                            EscapesCount = int.Parse(Data["escapes_count"].ToString()),
                            MvpCount = int.Parse(Data["mvp_count"].ToString()),
                            TotalMatchesCount = int.Parse(Data["total_matches"].ToString()),
                            TotalKillsCount = int.Parse(Data["total_kills"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Season;
        }
        public static bool CreatePlayerStatSeasonDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_seasons(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static StatClan GetPlayerStatClanDB(long OwnerId)
        {
            StatClan Clan = null;
            if (OwnerId == 0)
            {
                return Clan;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_clans WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Clan = new StatClan()
                        {
                            OwnerId = OwnerId,
                            Matches = int.Parse(Data["clan_matches"].ToString()),
                            MatchWins = int.Parse(Data["clan_match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["clan_match_loses"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Clan;
        }
        public static bool CreatePlayerStatClanDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_clans(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static StatDaily GetPlayerStatDailiesDB(long OwnerId)
        {
            StatDaily Dailies = null;
            if (OwnerId == 0)
            {
                return Dailies;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_dailies WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Dailies = new StatDaily()
                        {
                            OwnerId = OwnerId,
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            MatchDraws = int.Parse(Data["match_draws"].ToString()),
                            KillsCount = int.Parse(Data["kills_count"].ToString()),
                            DeathsCount = int.Parse(Data["deaths_count"].ToString()),
                            HeadshotsCount = int.Parse(Data["headshots_count"].ToString()),
                            ExpGained = int.Parse(Data["exp_gained"].ToString()),
                            PointGained = int.Parse(Data["point_gained"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Dailies;
        }
        public static bool CreatePlayerStatDailiesDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_dailies(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static StatWeapon GetPlayerStatWeaponsDB(long OwnerId)
        {
            StatWeapon Weapons = null;
            if (OwnerId == 0)
            {
                return Weapons;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_weapons WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Weapons = new StatWeapon()
                        {
                            OwnerId = OwnerId,
                            AssaultKills = int.Parse(Data["assault_rifle_kills"].ToString()),
                            AssaultDeaths = int.Parse(Data["assault_rifle_deaths"].ToString()),
                            SmgKills = int.Parse(Data["sub_machine_gun_kills"].ToString()),
                            SmgDeaths = int.Parse(Data["sub_machine_gun_deaths"].ToString()),
                            SniperKills = int.Parse(Data["sniper_rifle_kills"].ToString()),
                            SniperDeaths = int.Parse(Data["sniper_rifle_deaths"].ToString()),
                            MachinegunKills = int.Parse(Data["machine_gun_kills"].ToString()),
                            MachinegunDeaths = int.Parse(Data["machine_gun_deaths"].ToString()),
                            ShotgunKills = int.Parse(Data["shot_gun_kills"].ToString()),
                            ShotgunDeaths = int.Parse(Data["shot_gun_deaths"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Weapons;
        }
        public static bool CreatePlayerStatWeaponsDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_weapons(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static StatAcemode GetPlayerStatAcemodesDB(long OwnerId)
        {
            StatAcemode Acemode = null;
            if (OwnerId == 0)
            {
                return Acemode;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_acemodes WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Acemode = new StatAcemode()
                        {
                            OwnerId = OwnerId,
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            Kills = int.Parse(Data["kills_count"].ToString()),
                            Deaths = int.Parse(Data["deaths_count"].ToString()),
                            Headshots = int.Parse(Data["headshots_count"].ToString()),
                            Assists = int.Parse(Data["assists_count"].ToString()),
                            Escapes = int.Parse(Data["escapes_count"].ToString()),
                            Winstreaks = int.Parse(Data["winstreaks_count"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Acemode;
        }
        public static bool CreatePlayerStatAcemodesDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_acemodes(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static StatBattleroyale GetPlayerStatBattleroyaleDB(long OwnerId)
        {
            StatBattleroyale Battleroyale = null;
            if (OwnerId == 0)
            {
                return Battleroyale;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_stat_battleroyales WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Battleroyale = new StatBattleroyale()
                        {
                            OwnerId = OwnerId,
                            Matches = int.Parse(Data["matches"].ToString()),
                            MatchWins = int.Parse(Data["match_wins"].ToString()),
                            MatchLoses = int.Parse(Data["match_loses"].ToString()),
                            KillsCount = int.Parse(Data["kills_count"].ToString()),
                            DeathsCount = int.Parse(Data["deaths_count"].ToString()),
                            HeadshotsCount = int.Parse(Data["headshots_count"].ToString()),
                            AssistsCount = int.Parse(Data["assists_count"].ToString()),
                            EscapesCount = int.Parse(Data["escapes_count"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Battleroyale;
        }
        public static bool CreatePlayerStatBattleroyaleDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_stat_battleroyales(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static PlayerTitles GetPlayerTitlesDB(long OwnerId)
        {
            PlayerTitles Title = null;
            if (OwnerId == 0)
            {
                return Title;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_titles WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Title = new PlayerTitles()
                        {
                            OwnerId = OwnerId,
                            Equiped1 = int.Parse(Data["equip_slot1"].ToString()),
                            Equiped2 = int.Parse(Data["equip_slot2"].ToString()),
                            Equiped3 = int.Parse(Data["equip_slot3"].ToString()),
                            Flags = long.Parse(Data["flags"].ToString()),
                            Slots = int.Parse(Data["slots"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Title;
        }
        public static bool CreatePlayerTitlesDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_titles(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static PlayerBonus GetPlayerBonusDB(long OwnerId)
        {
            PlayerBonus Bonus = null;
            if (OwnerId == 0)
            {
                return Bonus;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_bonus WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Bonus = new PlayerBonus()
                        {
                            OwnerId = OwnerId,
                            Bonuses = int.Parse(Data["bonuses"].ToString()),
                            CrosshairColor = int.Parse(Data["crosshair_color"].ToString()),
                            FreePass = int.Parse(Data["free_pass"].ToString()),
                            FakeRank = int.Parse(Data["fake_rank"].ToString()),
                            FakeNick = Data["fake_nick"].ToString(),
                            MuzzleColor = int.Parse(Data["muzzle_color"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Bonus;
        }
        public static bool CreatePlayerBonusDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_bonus(owner_id) VALUES(@id)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static PlayerConfig GetPlayerConfigDB(long OwnerId)
        {
            PlayerConfig Config = null;
            if (OwnerId == 0)
            {
                return Config;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT * FROM player_configs WHERE owner_id=@owner";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Config = new PlayerConfig()
                        {
                            OwnerId = OwnerId,
                            Config = int.Parse(Data["configs"].ToString()),
                            ShowBlood = int.Parse(Data["show_blood"].ToString()),
                            Crosshair = int.Parse(Data["crosshair"].ToString()),
                            HandPosition = int.Parse(Data["hand_pos"].ToString()),
                            AudioSFX = int.Parse(Data["audio_sfx"].ToString()),
                            AudioBGM = int.Parse(Data["audio_bgm"].ToString()),
                            AudioEnable = int.Parse(Data["audio_enable"].ToString()),
                            Sensitivity = int.Parse(Data["sensitivity"].ToString()),
                            PointOfView = int.Parse(Data["pov_size"].ToString()),
                            InvertMouse = int.Parse(Data["invert_mouse"].ToString()),
                            EnableInviteMsg = int.Parse(Data["enable_invite"].ToString()),
                            EnableWhisperMsg = int.Parse(Data["enable_whisper"].ToString()),
                            Macro = int.Parse(Data["macro_enable"].ToString()),
                            Macro1 = Data["macro1"].ToString(),
                            Macro2 = Data["macro2"].ToString(),
                            Macro3 = Data["macro3"].ToString(),
                            Macro4 = Data["macro4"].ToString(),
                            Macro5 = Data["macro5"].ToString(),
                            Nations = int.Parse(Data["nations"].ToString())
                        };
                        Data.GetBytes(19, 0, Config.KeyboardKeys, 0, 235);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Config;
        }
        public static bool CreatePlayerConfigDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand cmd = connection.CreateCommand();
                    connection.Open();
                    cmd.Parameters.AddWithValue("@owner", OwnerId);
                    cmd.CommandText = "INSERT INTO player_configs(owner_id) VALUES(@owner)";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static PlayerEvent GetPlayerEventDB(long OwnerId)
        {
            PlayerEvent Event = null;
            if (OwnerId == 0)
            {
                return Event;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "SELECT * FROM player_events WHERE owner_id=@id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        Event = new PlayerEvent()
                        {
                            OwnerId = OwnerId,
                            LastVisitEventId = int.Parse(Data["last_visit_event_id"].ToString()),
                            LastVisitSequence1 = int.Parse(Data["last_visit_sequence1"].ToString()),
                            LastVisitSequence2 = int.Parse(Data["last_visit_sequence2"].ToString()),
                            NextVisitDate = int.Parse(Data["next_visit_date"].ToString()),
                            LastXmasRewardDate = uint.Parse(Data["last_xmas_reward_date"].ToString()),
                            LastPlaytimeDate = uint.Parse(Data["last_playtime_date"].ToString()),
                            LastPlaytimeValue = int.Parse(Data["last_playtime_value"].ToString()),
                            LastPlaytimeFinish = int.Parse(Data["last_playtime_finish"].ToString()),
                            LastLoginDate = uint.Parse(Data["last_login_date"].ToString()),
                            LastQuestDate = uint.Parse(Data["last_quest_date"].ToString()),
                            LastQuestFinish = int.Parse(Data["last_quest_finish"].ToString())
                        };
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Event;
        }
        public static bool CreatePlayerEventDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@id", OwnerId);
                    Command.CommandText = "INSERT INTO player_events (owner_id) VALUES (@id)";
                    Command.CommandType = CommandType.Text;
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static List<FriendModel> GetPlayerFriendsDB(long OwnerId)
        {
            List<FriendModel> Friends = new List<FriendModel>();
            if (OwnerId == 0)
            {
                return Friends;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT * FROM player_friends WHERE owner_id=@owner ORDER BY id";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        FriendModel Friend = new FriendModel(long.Parse(Data["id"].ToString()))
                        {
                            OwnerId = OwnerId,
                            ObjectId = long.Parse(Data["object_id"].ToString()),
                            State = int.Parse(Data["state"].ToString()),
                            Removed = bool.Parse(Data["removed"].ToString())
                        };
                        Friends.Add(Friend);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Friends;
        }
        public static void UpdatePlayerBonus(long PlayerId, int Bonuses, int FreePass)
        {
            if (PlayerId == 0)
            {
                return;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@id", PlayerId);
                    Command.Parameters.AddWithValue("@bonuses", Bonuses);
                    Command.Parameters.AddWithValue("@freepass", FreePass);
                    Command.CommandText = "UPDATE player_bonus SET bonuses=@bonuses, free_pass=@freepass WHERE owner_id=@id";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
        }
        public static List<QuickstartModel> GetPlayerQuickstartsDB(long OwnerId)
        {
            List<QuickstartModel> Quickjoins = new List<QuickstartModel>();
            if (OwnerId == 0)
            {
                return Quickjoins;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT * FROM player_quickstarts WHERE owner_id=@owner;";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        QuickstartModel Setting1 = new QuickstartModel()
                        {
                            MapId = byte.Parse(Reader["list0_map_id"].ToString()),
                            Rule = byte.Parse(Reader["list0_map_rule"].ToString()),
                            StageOptions = byte.Parse(Reader["list0_map_stage"].ToString()),
                            Type = byte.Parse(Reader["list0_map_type"].ToString())
                        };
                        Quickjoins.Add(Setting1);
                        QuickstartModel Setting2 = new QuickstartModel()
                        {
                            MapId = byte.Parse(Reader["list1_map_id"].ToString()),
                            Rule = byte.Parse(Reader["list1_map_rule"].ToString()),
                            StageOptions = byte.Parse(Reader["list1_map_stage"].ToString()),
                            Type = byte.Parse(Reader["list1_map_type"].ToString())
                        };
                        Quickjoins.Add(Setting2);
                        QuickstartModel Setting3 = new QuickstartModel()
                        {
                            MapId = byte.Parse(Reader["list2_map_id"].ToString()),
                            Rule = byte.Parse(Reader["list2_map_rule"].ToString()),
                            StageOptions = byte.Parse(Reader["list2_map_stage"].ToString()),
                            Type = byte.Parse(Reader["list2_map_type"].ToString())
                        };
                        Quickjoins.Add(Setting3);
                    }
                    Command.Dispose();
                    Reader.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Quickjoins;
        }
        public static bool CreatePlayerQuickstartsDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command2 = Connection.CreateCommand();
                    Connection.Open();
                    command2.Parameters.AddWithValue("@owner", OwnerId);
                    command2.CommandText = "INSERT INTO player_quickstarts(owner_id) VALUES(@owner);";
                    command2.CommandType = CommandType.Text;
                    command2.ExecuteNonQuery();
                    command2.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool IsPlayerNameExist(string Nickname)
        {
            if (string.IsNullOrEmpty(Nickname))
            {
                return true;
            }
            try
            {
                int value = 0;
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@name", Nickname);
                    Command.CommandText = "SELECT COUNT(*) FROM accounts WHERE nickname=@name";
                    value = Convert.ToInt32(Command.ExecuteScalar());
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return value > 0;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static List<NHistoryModel> GetPlayerNickHistory(object Value, int Type)
        {
            List<NHistoryModel> Nicks = new List<NHistoryModel>();
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    string moreCmd = Type == 0 ? "WHERE new_nick=@valor" : "WHERE owner_id=@valor";
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@valor", Value);
                    Command.CommandText = "SELECT * FROM base_nick_history " + moreCmd + " ORDER BY change_date LIMIT 30";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        NHistoryModel Nick = new NHistoryModel()
                        {
                            ObjectId = long.Parse(Data["object_id"].ToString()),
                            OwnerId = long.Parse(Data["owner_id"].ToString()),
                            OldNick = Data["old_nick"].ToString(),
                            NewNick = Data["new_nick"].ToString(),
                            ChangeDate = uint.Parse(Data["change_date"].ToString()),
                            Motive = Data["motive"].ToString()
                        };
                        Nicks.Add(Nick);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Nicks;
        }
        public static bool CreatePlayerNickHistory(long OwnerId, string OldNick, string NewNick, string Motive)
        {
            NHistoryModel history = new NHistoryModel()
            {
                OwnerId = OwnerId,
                OldNick = OldNick,
                NewNick = NewNick,
                ChangeDate = uint.Parse(DateTimeUtil.Now("yyMMddHHmm")),
                Motive = Motive
            };
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", history.OwnerId);
                    Command.Parameters.AddWithValue("@oldnick", history.OldNick);
                    Command.Parameters.AddWithValue("@newnick", history.NewNick);
                    Command.Parameters.AddWithValue("@date", (long)history.ChangeDate);
                    Command.Parameters.AddWithValue("@motive", history.Motive);
                    Command.CommandType = CommandType.Text;
                    Command.CommandText = "INSERT INTO base_nick_history(owner_id, old_nick, new_nick, change_date, motive) VALUES(@owner, @oldnick, @newnick, @date, @motive)";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateAccountValuable(long PlayerId, int Gold, int Cash, int Tags)
        {
            if (PlayerId == 0 || Gold == -1 && Cash == -1 && Tags == -1)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", PlayerId);
                    string cmd = "";
                    if (Gold > -1)
                    {
                        Command.Parameters.AddWithValue("@gold", Gold);
                        cmd += "gold=@gold";
                    }
                    if (Cash > -1)
                    {
                        Command.Parameters.AddWithValue("@cash", Cash);
                        cmd += (cmd != "" ? ", " : "") + "cash=@cash";
                    }
                    if (Tags > -1)
                    {
                        Command.Parameters.AddWithValue("@tags", Tags);
                        cmd += (cmd != "" ? ", " : "") + "tags=@tags";
                    }
                    Command.CommandText = "UPDATE accounts SET " + cmd + " WHERE player_id=@owner";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdatePlayerKD(long OwnerId, int Kills, int Deaths, int Headshots, int Totals)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@deaths", Deaths);
                    Command.Parameters.AddWithValue("@kills", Kills);
                    Command.Parameters.AddWithValue("@hs", Headshots);
                    Command.Parameters.AddWithValue("@total", Totals);
                    Command.CommandText = "UPDATE player_stat_seasons SET kills_count=@kills, deaths_count=@deaths, headshots_count=@hs, total_kills=@total WHERE owner_id=@owner";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdatePlayerMatches(int Matches, int MatchWins, int MatchLoses, int MatchDraws, int Totals, long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@owner", OwnerId);
                    command.Parameters.AddWithValue("@partidas", Matches);
                    command.Parameters.AddWithValue("@ganhas", MatchWins);
                    command.Parameters.AddWithValue("@perdidas", MatchLoses);
                    command.Parameters.AddWithValue("@empates", MatchDraws);
                    command.Parameters.AddWithValue("@todaspartidas", Totals);
                    command.CommandText = "UPDATE player_stat_seasons SET matches=@partidas, match_wins=@ganhas, match_loses=@perdidas, match_draws=@empates, total_matches=@todaspartidas WHERE owner_id=@owner";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateAccountCash(long OwnerId, int Cash)
        {
            if (OwnerId == 0 || Cash == -1)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@cash", Cash);
                    Command.CommandText = "UPDATE accounts SET cash=@cash WHERE player_id=@owner";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateAccountGold(long OwnerId, int Gold)
        {
            if (OwnerId == 0 || Gold == -1)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@gold", Gold);
                    Command.CommandText = "UPDATE accounts SET gold=@gold WHERE player_id=@owner";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateAccountTags(long OwnerId, int Tags)
        {
            if (OwnerId == 0 || Tags == -1)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@tag", Tags);
                    Command.CommandText = "UPDATE accounts SET tags=@tag WHERE player_id=@owner";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static void UpdateCouponEffect(long PlayerId, CouponEffects Effects)
        {
            if (PlayerId == 0)
            {
                return;
            }
            ComDiv.UpdateDB("accounts", "coupon_effect", (long)Effects, "player_id", PlayerId);
        }
        public static int GetRequestClanId(long OwnerId)
        {
            int Results = 0;
            if (OwnerId == 0)
            {
                return Results;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "SELECT clan_id FROM system_clan_invites WHERE player_id=@owner";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    if (Data.Read())
                    {
                        Results = int.Parse(Data["clan_id"].ToString());
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Results;
        }
        public static int GetRequestClanCount(int ClanId)
        {
            int Count = 0;
            if (ClanId == 0)
            {
                return Count;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.CommandText = "SELECT COUNT(*) FROM system_clan_invites WHERE clan_id=@clan";
                    Count = Convert.ToInt32(Command.ExecuteScalar());
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Count;
        }
        public static List<ClanInvite> GetClanRequestList(int ClanId)
        {
            List<ClanInvite> Invites = new List<ClanInvite>();
            if (ClanId == 0)
            {
                return Invites;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@clan", ClanId);
                    Command.CommandText = "SELECT * FROM system_clan_invites WHERE clan_id=@clan";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        ClanInvite Invite = new ClanInvite()
                        {
                            Id = ClanId,
                            PlayerId = long.Parse(Data["player_id"].ToString()),
                            InviteDate = uint.Parse(Data["invite_date"].ToString()),
                            Text = Data["text"].ToString()
                        };
                        Invites.Add(Invite);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Invites;
        }
        public static int GetPlayerMessagesCount(long OwnerId)
        {
            int Messages = 0;
            if (OwnerId == 0)
            {
                return Messages;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = Connection.CreateCommand();
                    Connection.Open();
                    command.Parameters.AddWithValue("@owner", OwnerId);
                    command.CommandText = "SELECT COUNT(*) FROM player_messages WHERE owner_id=@owner";
                    Messages = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Messages;
        }
        public static bool CreatePlayerMessage(long OwnerId, MessageModel Message)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = Connection.CreateCommand();
                    Connection.Open();
                    command.Parameters.AddWithValue("@owner", OwnerId);
                    command.Parameters.AddWithValue("@sendid", Message.SenderId);
                    command.Parameters.AddWithValue("@clan", Message.ClanId);
                    command.Parameters.AddWithValue("@sendname", Message.SenderName);
                    command.Parameters.AddWithValue("@text", Message.Text);
                    command.Parameters.AddWithValue("@type", Message.Type);
                    command.Parameters.AddWithValue("@state", Message.State);
                    command.Parameters.AddWithValue("@expire", (long)Message.ExpireDate);
                    command.Parameters.AddWithValue("@cb", (int)Message.ClanNote);
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO player_messages(owner_id, sender_id, sender_name, clan_id, clan_note, text, type, state, expire)VALUES(@owner, @sendid, @sendname, @clan, @cb, @text, @type, @state, @expire) RETURNING object_id";
                    object Data = command.ExecuteScalar();
                    Message.ObjectId = (long)Data;
                    command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static bool DeletePlayerFriend(long friendId, long pId)
        {
            return ComDiv.DeleteDB("player_friends", "id", friendId, "owner_id", pId);
        }
        public static void UpdatePlayerFriendState(long ownerId, FriendModel friend)
        {
            ComDiv.UpdateDB("player_friends", "state", friend.State, "owner_id", ownerId, "id", friend.PlayerId);
        }
        public static void UpdatePlayerFriendBlock(long OwnerId, FriendModel Friend)
        {
            ComDiv.UpdateDB("player_friends", "removed", Friend.Removed, "owner_id", OwnerId, "id", Friend.PlayerId);
        }
        public static bool DeleteClanInviteDB(int ClanId, long PlayerId)
        {
            if (PlayerId == 0 || ClanId == 0)
            {
                return false;
            }
            return ComDiv.DeleteDB("system_clan_invites", "clan_id", ClanId, "player_id", PlayerId);
        }
        public static bool DeleteClanInviteDB(long PlayerId)
        {
            if (PlayerId == 0)
            {
                return false;
            }
            return ComDiv.DeleteDB("system_clan_invites", "player_id", PlayerId);
        }
        public static bool CreateClanInviteInDB(ClanInvite invite)
        {
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@clan", invite.Id);
                    command.Parameters.AddWithValue("@player", invite.PlayerId);
                    command.Parameters.AddWithValue("@date", (long)invite.InviteDate);
                    command.Parameters.AddWithValue("@text", invite.Text);
                    command.CommandText = "INSERT INTO system_clan_invites(clan_id, player_id, invite_date, text)VALUES(@clan,@player,@date,@text)";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static int GetRequestClanInviteCount(int clanId)
        {
            int count = 0;
            if (clanId == 0)
            {
                return count;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@clan", clanId);
                    command.CommandText = "SELECT COUNT(*) FROM system_clan_invites WHERE clan_id=@clan";
                    count = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return count;
        }
        public static string GetRequestClanInviteText(int ClanId, long PlayerId)
        {
            string result = null;
            if (ClanId == 0 || PlayerId == 0)
            {
                return result;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@clan", ClanId);
                    command.Parameters.AddWithValue("@player", PlayerId);
                    command.CommandText = "SELECT text FROM system_clan_invites WHERE clan_id=@clan AND player_id=@player";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    if (data.Read())
                    {
                        result = data["text"].ToString();
                    }
                    command.Dispose();
                    data.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return result;
        }
        public static string GetPlayerIP4Address(long PlayerId)
        {
            string result = "";
            if (PlayerId == 0)
            {
                return result;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@player", PlayerId);
                    command.CommandText = "SELECT ip4_address FROM accounts WHERE player_id=@player";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    if (data.Read())
                    {
                        result = data["ip4_address"].ToString();
                    }
                    command.Dispose();
                    data.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return result;
        }
        public static PlayerMissions GetPlayerMissionsDB(long OwnerId, int Mission1, int Mission2, int Mission3, int Mission4)
        {
            PlayerMissions Mission = null;
            if (OwnerId == 0)
            {
                return Mission;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@owner", OwnerId);
                    command.CommandText = "SELECT * FROM player_missions WHERE owner_id=@owner";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        Mission = new PlayerMissions()
                        {
                            OwnerId = OwnerId,
                            ActualMission = int.Parse(Data["current_mission"].ToString()),
                            Card1 = int.Parse(Data["card1"].ToString()),
                            Card2 = int.Parse(Data["card2"].ToString()),
                            Card3 = int.Parse(Data["card3"].ToString()),
                            Card4 = int.Parse(Data["card4"].ToString()),
                            Mission1 = Mission1,
                            Mission2 = Mission2,
                            Mission3 = Mission3,
                            Mission4 = Mission4,
                        };
                        Data.GetBytes(6, 0, Mission.List1, 0, 40);
                        Data.GetBytes(7, 0, Mission.List2, 0, 40);
                        Data.GetBytes(8, 0, Mission.List3, 0, 40);
                        Data.GetBytes(9, 0, Mission.List4, 0, 40);
                        Mission.UpdateSelectedCard();
                    }
                    command.Dispose();
                    Data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Mission;
        }
        public static bool CreatePlayerMissionsDB(long OwnerId)
        {
            if (OwnerId == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.CommandText = "INSERT INTO player_missions(owner_id) VALUES(@owner)";
                    Command.CommandType = CommandType.Text;
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static void UpdateCurrentPlayerMissionList(long player_id, PlayerMissions mission)
        {
            byte[] list = mission.GetCurrentMissionList();
            ComDiv.UpdateDB("player_missions", $"mission{(mission.ActualMission + 1)}_raw", list, "owner_id", player_id);
        }
        public static bool DeletePlayerCharacter(long ObjectId, long OwnerId)
        {
            if (ObjectId == 0 || OwnerId == 0)
            {
                return false;
            }
            return ComDiv.DeleteDB("player_characters", "object_id", ObjectId, "owner_id", OwnerId);
        }
        public static bool UpdatePlayerCharacter(int Slot, long ObjectId, long OwnerId)
        {
            return ComDiv.UpdateDB("player_characters", "slot", Slot, "object_id", ObjectId, "owner_id", OwnerId);
        }
        public static bool UpdateEquipedPlayerTitle(long player_id, int index, int titleId)
        {
            return ComDiv.UpdateDB("player_titles", $"equip_slot{(index + 1)}", titleId, "owner_id", player_id);
        }
        public static void UpdatePlayerTitlesFlags(long player_id, long flags)
        {
            ComDiv.UpdateDB("player_titles", "flags", flags, "owner_id", player_id);
        }
        public static void UpdatePlayerTitleRequi(long player_id, int medalhas, int insignias, int ordens_azuis, int broche)
        {
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@pid", player_id);
                    command.Parameters.AddWithValue("@broche", broche);
                    command.Parameters.AddWithValue("@insignias", insignias);
                    command.Parameters.AddWithValue("@medalhas", medalhas);
                    command.Parameters.AddWithValue("@ordensazuis", ordens_azuis);
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE accounts SET ribbon=@broche, ensign=@insignias, medal=@medalhas, master_medal=@ordensazuis WHERE player_id=@pid";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
        }
        public static bool UpdatePlayerMissionId(long player_id, int value, int index)
        {
            return ComDiv.UpdateDB("accounts", $"mission_id{(index + 1)}", value, "player_id", player_id);
        }
        public static int GetUsedTicket(long OwnerId, string Token)
        {
            int result = 0;
            if (OwnerId == 0 || string.IsNullOrEmpty(Token))
            {
                return result;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@player", OwnerId);
                    command.Parameters.AddWithValue("@token", Token);
                    command.CommandText = "SELECT used_count FROM base_redeem_history WHERE used_token=@token AND owner_id=@player";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    if (data.Read())
                    {
                        result = int.Parse(data["used_count"].ToString());
                    }
                    command.Dispose();
                    data.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return result;
        }
        public static bool IsTicketUsedByPlayer(long OwnerId, string Token)
        {
            bool Result = false;
            if (OwnerId == 0)
            {
                return Result;
            }
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@player", OwnerId);
                    command.Parameters.AddWithValue("@token", Token);
                    command.CommandText = "SELECT * FROM base_redeem_history WHERE used_token=@token AND owner_id=@player";
                    command.CommandType = CommandType.Text;
                    Result = Convert.ToBoolean(command.ExecuteScalar());
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Result;
        }
        public static bool CreatePlayerRedeemHistory(long OwnerId, string Token, int Used)
        {
            if (OwnerId == 0 || string.IsNullOrEmpty(Token) || Used == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@owner", OwnerId);
                    Command.Parameters.AddWithValue("@token", Token);
                    Command.Parameters.AddWithValue("@used", Used);
                    Command.CommandText = "INSERT INTO base_redeem_history(owner_id, used_token, used_count) VALUES(@owner, @token, @used)";
                    Command.CommandType = CommandType.Text;
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
                return false;
            }
        }
        public static PlayerVip GetPlayerVIP(long OwnerId)
        {
            PlayerVip Vip = null;
            if (OwnerId == 0)
            {
                return Vip;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.Parameters.AddWithValue("@ownerId", OwnerId);
                    Command.CommandText = "SELECT * FROM player_vip WHERE owner_id=@ownerId";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        Vip = new PlayerVip()
                        {
                            OwnerId = OwnerId,
                            Address = Reader["registered_ip"].ToString(),
                            Benefit = Reader["last_benefit"].ToString(),
                            Expirate = uint.Parse(Reader["expirate"].ToString())
                        };
                    }
                    Command.Dispose();
                    Reader.Close();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
            return Vip;
        }
    }
}
