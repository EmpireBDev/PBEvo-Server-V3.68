using Npgsql;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.RAW;
using Plugin.Core.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;

namespace Plugin.Core.Utility
{
    public static class ComDiv
    {
        public static int CheckEquipedItems(PlayerEquipment Equip, List<ItemsModel> Inventory, bool BattleRules)
        {
            int Type = 0;
            bool Primary = false, Secondary = false, Melee = false, Explosive = false, Special = false, RedSide = false, BlueSide = false, Head = false, Face = false, Jacket = false, Pocket = false, Glove = false, Belt = false, Holster = false, Skin = false, Beret = false, Dino = false, Accesory = false, Spray = false, Namecard = false;
            if (Equip.WeaponPrimary == 103004) //Always K-2 for default weapon
            {
                Primary = true;
            }
            if (BattleRules)
            {
                if (!Primary)
                {
                    if (Equip.WeaponPrimary == 105025 || Equip.WeaponPrimary == 106007) //SSG-69 For Sniper Mode or 870MCS For Shotgun Mode
                    {
                        Primary = true;
                    }
                }
                if (!Melee)
                {
                    if (Equip.WeaponMelee == 323001) //Bare Fist for Knuckle Mode
                    {
                        Melee = true;
                    }
                }
            }
            if (Equip.BeretItem == 0)
            {
                Beret = true;
            }
            if (Equip.AccessoryId == 0)
            {
                Accesory = true;
            }
            if (Equip.SprayId == 0)
            {
                Spray = true;
            }
            if (Equip.NameCardId == 0)
            {
                Namecard = true;
            }
            lock (Inventory)
            {
                foreach (ItemsModel Item in Inventory)
                {
                    if (Item.Count > 0)
                    {
                        if (Item.Id == Equip.WeaponPrimary)
                        {
                            Primary = true;
                        }
                        else if (Item.Id == Equip.WeaponSecondary)
                        {
                            Secondary = true;
                        }
                        else if (Item.Id == Equip.WeaponMelee)
                        {
                            Melee = true;
                        }
                        else if (Item.Id == Equip.WeaponExplosive)
                        {
                            Explosive = true;
                        }
                        else if (Item.Id == Equip.WeaponSpecial)
                        {
                            Special = true;
                        }
                        else if (Item.Id == Equip.CharaRedId)
                        {
                            RedSide = true;
                        }
                        else if (Item.Id == Equip.CharaBlueId)
                        {
                            BlueSide = true;
                        }
                        else if (Item.Id == Equip.PartHead)
                        {
                            Head = true;
                        }
                        else if (Item.Id == Equip.PartFace)
                        {
                            Face = true;
                        }
                        else if (Item.Id == Equip.PartJacket)
                        {
                            Jacket = true;
                        }
                        else if (Item.Id == Equip.PartPocket)
                        {
                            Pocket = true;
                        }
                        else if (Item.Id == Equip.PartGlove)
                        {
                            Glove = true;
                        }
                        else if (Item.Id == Equip.PartBelt)
                        {
                            Belt = true;
                        }
                        else if (Item.Id == Equip.PartHolster)
                        {
                            Holster = true;
                        }
                        else if (Item.Id == Equip.PartSkin)
                        {
                            Skin = true;
                        }
                        else if (Item.Id == Equip.BeretItem)
                        {
                            Beret = true;
                        }
                        else if (Item.Id == Equip.DinoItem)
                        {
                            Dino = true;
                        }
                        else if (Item.Id == Equip.AccessoryId)
                        {
                            Accesory = true;
                        }
                        else if (Item.Id == Equip.SprayId)
                        {
                            Spray = true;
                        }
                        else if (Item.Id == Equip.NameCardId)
                        {
                            Namecard = true;
                        }
                        if (Primary && Secondary && Melee && Explosive && Special && RedSide && BlueSide && Head && Face && Jacket && Pocket && Glove && Belt && Holster && Skin && Beret && Dino && Accesory && Spray && Namecard)
                        {
                            break;
                        }
                    }
                }
            }
            if (!Primary || !Secondary || !Melee || !Explosive || !Special)
            {
                Type += 2;
            }
            if (!RedSide || !BlueSide || !Head || !Face || !Jacket || !Pocket || !Glove || !Belt || !Holster || !Skin || !Beret || !Dino)
            {
                Type += 1;
            }
            if (!Accesory || !Spray || !Namecard)
            {
                Type += 3;
            }
            if (!Primary)
            {
                Equip.WeaponPrimary = 103004;
            }
            if (!Secondary)
            {
                Equip.WeaponSecondary = 202003;
            }
            if (!Melee)
            {
                Equip.WeaponMelee = 301001;
            }
            if (!Explosive)
            {
                Equip.WeaponExplosive = 407001;
            }
            if (!Special)
            {
                Equip.WeaponSpecial = 508001;
            }
            if (!RedSide)
            {
                Equip.CharaRedId = 601001;
            }
            if (!BlueSide)
            {
                Equip.CharaBlueId = 602002;
            }
            if (!Head)
            {
                Equip.PartHead = 1000700000;
            }
            if (!Face)
            {
                Equip.PartFace = 1000800000;
            }
            if (!Jacket)
            {
                Equip.PartJacket = 1000900000;
            }
            if (!Pocket)
            {
                Equip.PartPocket = 1001000000;
            }
            if (!Glove)
            {
                Equip.PartGlove = 1001100000;
            }
            if (!Belt)
            {
                Equip.PartBelt = 1001200000;
            }
            if (!Holster)
            {
                Equip.PartHolster = 1001300000;
            }
            if (!Skin)
            {
                Equip.PartSkin = 1001400000;
            }
            if (!Beret)
            {
                Equip.BeretItem = 0;
            }
            if (!Dino)
            {
                Equip.DinoItem = 1500511;
            }
            if (!Accesory)
            {
                Equip.AccessoryId = 0;
            }
            if (!Spray)
            {
                Equip.SprayId = 0;
            }
            if (!Namecard)
            {
                Equip.NameCardId = 0;
            }
            return Type;
        }
        public static void UpdateWeapons(PlayerEquipment Source, PlayerEquipment Equip, DBQuery Query)
        {
            if (Equip.WeaponPrimary != Source.WeaponPrimary)
            {
                Query.AddQuery("weapon_primary", Source.WeaponPrimary);
            }
            if (Equip.WeaponSecondary != Source.WeaponSecondary)
            {
                Query.AddQuery("weapon_secondary", Source.WeaponSecondary);
            }
            if (Equip.WeaponMelee != Source.WeaponMelee)
            {
                Query.AddQuery("weapon_melee", Source.WeaponMelee);
            }
            if (Equip.WeaponExplosive != Source.WeaponExplosive)
            {
                Query.AddQuery("weapon_explosive", Source.WeaponExplosive);
            }
            if (Equip.WeaponSpecial != Source.WeaponSpecial)
            {
                Query.AddQuery("weapon_special", Source.WeaponSpecial);
            }
        }
        public static void UpdateChars(PlayerEquipment Source, PlayerEquipment Equip, DBQuery Query)
        {
            if (Equip.CharaRedId != Source.CharaRedId)
            {
                Query.AddQuery("chara_red_side", Source.CharaRedId);
            }
            if (Equip.CharaBlueId != Source.CharaBlueId)
            {
                Query.AddQuery("chara_blue_side", Source.CharaBlueId);
            }
            if (Equip.PartHead != Source.PartHead)
            {
                Query.AddQuery("part_head", Source.PartHead);
            }
            if (Equip.PartFace != Source.PartFace)
            {
                Query.AddQuery("part_face", Source.PartFace);
            }
            if (Equip.PartJacket != Source.PartJacket)
            {
                Query.AddQuery("part_jacket", Source.PartJacket);
            }
            if (Equip.PartPocket != Source.PartPocket)
            {
                Query.AddQuery("part_pocket", Source.PartPocket);
            }
            if (Equip.PartPocket != Source.PartPocket)
            {
                Query.AddQuery("part_glove", Source.PartPocket);
            }
            if (Equip.PartBelt != Source.PartBelt)
            {
                Query.AddQuery("part_belt", Source.PartBelt);
            }
            if (Equip.PartHolster != Source.PartHolster)
            {
                Query.AddQuery("part_holster", Source.PartHolster);
            }
            if (Equip.PartSkin != Source.PartSkin)
            {
                Query.AddQuery("part_skin", Source.PartSkin);
            }
            if (Equip.BeretItem != Source.BeretItem)
            {
                Query.AddQuery("beret_item_part", Source.BeretItem);
            }
            if (Equip.DinoItem != Source.DinoItem)
            {
                Query.AddQuery("dino_item_chara", Source.DinoItem);
            }
        }
        public static void UpdateCharSlots(PlayerEquipment Source, PlayerEquipment Equip, DBQuery Query)
        {
            if (Equip.CharaRedId != Source.CharaRedId)
            {
                Query.AddQuery("chara_red_side", Source.CharaRedId);
            }
            if (Equip.CharaBlueId != Source.CharaBlueId)
            {
                Query.AddQuery("chara_blue_side", Source.CharaBlueId);
            }
            if (Equip.DinoItem != Source.DinoItem)
            {
                Query.AddQuery("dino_item_chara", Source.DinoItem);
            }
        }
        public static void UpdateItems(PlayerEquipment Source, PlayerEquipment Equip, DBQuery Query)
        {
            if (Equip.AccessoryId != Source.AccessoryId)
            {
                Query.AddQuery("accesory_id", Source.AccessoryId);
            }
            if (Equip.SprayId != Source.SprayId)
            {
                Query.AddQuery("spray_id", Source.SprayId);
            }
            if (Equip.NameCardId != Source.NameCardId)
            {
                Query.AddQuery("namecard_id", Source.NameCardId);
            }
        }
        public static void UpdateWeapons(PlayerEquipment Equip, DBQuery Query)
        {
            Query.AddQuery("weapon_primary", Equip.WeaponPrimary);
            Query.AddQuery("weapon_secondary", Equip.WeaponSecondary);
            Query.AddQuery("weapon_melee", Equip.WeaponMelee);
            Query.AddQuery("weapon_explosive", Equip.WeaponExplosive);
            Query.AddQuery("weapon_special", Equip.WeaponSpecial);
        }
        public static void UpdateChars(PlayerEquipment Equip, DBQuery Query)
        {
            Query.AddQuery("chara_red_side", Equip.CharaRedId);
            Query.AddQuery("chara_blue_side", Equip.CharaBlueId);
            Query.AddQuery("part_head", Equip.PartHead);
            Query.AddQuery("part_face", Equip.PartFace);
            Query.AddQuery("part_jacket", Equip.PartJacket);
            Query.AddQuery("part_pocket", Equip.PartPocket);
            Query.AddQuery("part_glove", Equip.PartGlove);
            Query.AddQuery("part_belt", Equip.PartBelt);
            Query.AddQuery("part_holster", Equip.PartHolster);
            Query.AddQuery("part_skin", Equip.PartSkin);
            Query.AddQuery("beret_item_part", Equip.BeretItem);
            Query.AddQuery("dino_item_chara", Equip.DinoItem);
        }
        public static void UpdateItems(PlayerEquipment Equip, DBQuery Query)
        {
            Query.AddQuery("accesory_id", Equip.AccessoryId);
            Query.AddQuery("spray_id", Equip.SprayId);
            Query.AddQuery("namecard_id", Equip.NameCardId);
        }
        public static void TryCreateItem(ItemsModel Model, PlayerInventory Inventory, long OwnerId)
        {
            try
            {
                ItemsModel Item = Inventory.GetItem(Model.Id);
                if (Item == null)
                {
                    if (DaoManagerSQL.CreatePlayerInventoryItem(Model, OwnerId))
                    {
                        Inventory.AddItem(Model);
                    }
                }
                else
                {
                    Model.ObjectId = Item.ObjectId;
                    if (Item.Equip == ItemEquipType.Durable)
                    {
                        if (ShopManager.IsRepairableItem(Model.Id))
                        {
                            Model.Count = 100;
                            ComDiv.UpdateDB("player_items", "count", (long)Model.Count, "owner_id", OwnerId, "id", Model.Id);
                        }
                        else
                        {
                            Model.Count += Item.Count;
                            ComDiv.UpdateDB("player_items", "count", (long)Model.Count, "owner_id", OwnerId, "id", Model.Id);
                        }
                    }
                    else if (Item.Equip == ItemEquipType.Temporary)
                    {
                        DateTime Data = DateTime.ParseExact(Item.Count.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
                        if (Model.Category != ItemCategory.Coupon)
                        {
                            Model.Equip = ItemEquipType.Temporary;
                            Model.Count = Convert.ToUInt32(Data.AddSeconds(Model.Count).ToString("yyMMddHHmm"));
                        }
                        else
                        {
                            TimeSpan Time = DateTime.ParseExact(Model.Count.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture) - DateTimeUtil.Now();
                            Model.Equip = ItemEquipType.Temporary;
                            Model.Count = Convert.ToUInt32(Data.AddDays(Time.TotalDays).ToString("yyMMddHHmm"));
                        }
                        ComDiv.UpdateDB("player_items", "count", (long)Model.Count, "owner_id", OwnerId, "id", Model.Id);
                    }
                    Item.Equip = Model.Equip;
                    Item.Count = Model.Count;
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static ItemCategory GetItemCategory(int ItemId)
        {
            int BasicValue = GetIdStatics(ItemId, 1), PartValue = GetIdStatics(ItemId, 4);
            if (BasicValue >= 1 && BasicValue <= 5)
            {
                return ItemCategory.Weapon;
            }
            else if ((BasicValue >= 6 && BasicValue <= 14) || BasicValue == 15 || (BasicValue >= 26 && BasicValue <= 27) || (PartValue >= 7 && PartValue <= 14))
            {
                return ItemCategory.Character;
            }
            else if ((BasicValue >= 16 && BasicValue <= 20) || BasicValue == 28 || (BasicValue >= 30 && BasicValue <= 37))
            {
                return ItemCategory.Coupon;
            }
            else
            {
                CLogger.Print($"Invalid Category [{BasicValue}]: {ItemId}", LoggerType.Warning);
            }
            return ItemCategory.None;
        }
        public static uint ValidateStockId(int ItemId)
        {
            int CharaPart = GetIdStatics(ItemId, 4);
            return GenStockId((CharaPart >= 7 && CharaPart <= 14) ? CharaPart : ItemId);
        }
        public static int GetIdStatics(int id, int type)
        {
            switch (type)
            {
                case 1: return id / 100000; // Item Class
                case 2: return id % 100000 / 1000; // Class Type
                case 3: return id % 1000; // Number
                case 4: return id % 10000000 / 100000; //Partial
                default: return 0;
            }
        }
        public static double GetDuration(DateTime Date)
        {
            return (double)(DateTimeUtil.Now() - Date).TotalSeconds;
        }
        public static byte[] AddressBytes(string Host)
        {
            return IPAddress.Parse(Host).GetAddressBytes();
        }
        public static int CreateItemId(int ItemClass, int ClassType, int Number)
        {
            return ItemClass * 100000 + ClassType * 1000 + Number;
        }
        public static int Percentage(int Total, int Percent)
        {
            return Total * Percent / 100;
        }
        public static float Percentage(float Total, int Percent)
        {
            return Total * Percent / 100;
        }
        public static ClassType GetIdClassType(int id)
        {
            return (ClassType)(id % 100000 / 1000);
        }
        public static EnumType ConvertToEnum<EnumType>(string EnumValue)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), EnumValue);
        }
        public static char[] SubArray(this char[] Input, int StartIndex, int Length)
        {
            List<char> Result = new List<char>();
            for (int i = StartIndex; i < Length; i++)
            {
                Result.Add(Input[i]);
            }
            return Result.ToArray();
        }
        public static bool UpdateDB(string TABEL, string COLUMN, object VALUE)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@Value", VALUE);
                    Command.CommandText = $"UPDATE {TABEL} SET {COLUMN}=@Value";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.UpdateDB1] {ex.Message}", LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateDB(string TABEL, string Req1, object ValueReq1, string[] COLUMNS, params object[] VALUES)
        {
            if (COLUMNS.Length > 0 && VALUES.Length > 0 && COLUMNS.Length != VALUES.Length)
            {
                CLogger.Print("[Update Database] Wrong values: " + string.Join(",", COLUMNS) + "/" + string.Join(",", VALUES), LoggerType.Warning);
                return false;
            }
            else if (COLUMNS.Length == 0 || VALUES.Length == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    string Loaded = "";
                    List<string> Parameters = new List<string>();
                    for (int i = 0; i < VALUES.Length; i++)
                    {
                        object Obj = VALUES[i];
                        string Column = COLUMNS[i];
                        string Param = "@Value" + i;
                        Command.Parameters.AddWithValue(Param, Obj);
                        Parameters.Add(Column + "=" + Param);
                    }
                    Loaded = string.Join(",", Parameters.ToArray());
                    Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req1}=@Req1";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.UpdateDB2] {ex.Message}", LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateDB(string TABEL, string COLUMN, object VALUE, string Req1, object ValueReq1)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@Value", VALUE);
                    Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    Command.CommandText = $"UPDATE {TABEL} SET {COLUMN}=@Value WHERE {Req1}=@Req1";
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.UpdateDB3] {ex.Message}", LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateDB(string TABEL, string Req1, object ValueReq1, string Req2, object valueReq2, string[] COLUMNS, params object[] VALUES)
        {
            if (COLUMNS.Length > 0 && VALUES.Length > 0 && COLUMNS.Length != VALUES.Length)
            {
                CLogger.Print("[Update Database] Wrong values: " + string.Join(",", COLUMNS) + "/" + string.Join(",", VALUES), LoggerType.Warning);
                return false;
            }
            else if (COLUMNS.Length == 0 || VALUES.Length == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    string Loaded = "";
                    List<string> Parameters = new List<string>();
                    for (int i = 0; i < VALUES.Length; i++)
                    {
                        object Obj = VALUES[i];
                        string Column = COLUMNS[i];
                        string Param = "@Value" + i;
                        Command.Parameters.AddWithValue(Param, Obj);
                        Parameters.Add(Column + "=" + Param);
                    }
                    Loaded = string.Join(",", Parameters.ToArray());
                    if (Req1 != null)
                    {
                        Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    }
                    if (Req2 != null)
                    {
                        Command.Parameters.AddWithValue("@Req2", valueReq2);
                    }
                    if (Req1 != null && Req2 == null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req1}=@Req1";
                    }
                    else if (Req2 != null && Req1 == null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req2}=@Req2";
                    }
                    else if (Req2 != null && Req1 != null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req1}=@Req1 AND {Req2}=@Req2";
                    }
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.UpdateDB4] {ex.Message}", LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateDB(string TABEL, string Req1, int[] ValueReq1, string Req2, object ValueReq2, string[] COLUMNS, params object[] VALUES)
        {
            if (COLUMNS.Length > 0 && VALUES.Length > 0 && COLUMNS.Length != VALUES.Length)
            {
                CLogger.Print("[updateDB5] Wrong values: " + string.Join(",", COLUMNS) + "/" + string.Join(",", VALUES), LoggerType.Warning);
                return false;
            }
            else if (COLUMNS.Length == 0 || VALUES.Length == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    string Loaded = "";
                    List<string> Parameters = new List<string>();
                    for (int i = 0; i < VALUES.Length; i++)
                    {
                        object Obj = VALUES[i];
                        string Column = COLUMNS[i];
                        string Param = "@Value" + i;
                        Command.Parameters.AddWithValue(Param, Obj);
                        Parameters.Add(Column + "=" + Param);
                    }
                    Loaded = string.Join(",", Parameters.ToArray());
                    if (Req1 != null)
                    {
                        Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    }
                    if (Req2 != null)
                    {
                        Command.Parameters.AddWithValue("@Req2", ValueReq2);
                    }
                    if (Req1 != null && Req2 == null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req1} = ANY (@Req1)";
                    }
                    else if (Req2 != null && Req1 == null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req2}=@Req2";
                    }
                    else if (Req2 != null && Req1 != null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {Loaded} WHERE {Req1} = ANY (@Req1) AND {Req2}=@Req2";
                    }
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.UpdateDB5] {ex.Message}", LoggerType.Error, ex);
                return false;
            }
        }
        public static bool UpdateDB(string TABEL, string COLUMN, object VALUE, string Req1, object ValueReq1, string Req2, object ValueReq2)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@Value", VALUE);
                    if (Req1 != null)
                    {
                        Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    }
                    if (Req2 != null)
                    {
                        Command.Parameters.AddWithValue("@Req2", ValueReq2);
                    }
                    if (Req1 != null && Req2 == null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {COLUMN}=@Value WHERE {Req1}=@Req1";
                    }
                    else if (Req2 != null && Req1 == null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {COLUMN}=@Value WHERE {Req2}=@Req2";
                    }
                    else if (Req2 != null && Req1 != null)
                    {
                        Command.CommandText = $"UPDATE {TABEL} SET {COLUMN}=@Value WHERE {Req1}=@Req1 AND {Req2}=@Req2";
                    }
                    Command.ExecuteNonQuery();
                    Command.Dispose();
                    Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.UpdateDB6] {ex.Message}", LoggerType.Error, ex);
                return false;
            }
        }
        public static bool DeleteDB(string TABEL, string Req1, object ValueReq1)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    Command.CommandText = $"DELETE FROM {TABEL} WHERE {Req1}=@Req1";
                    Command.ExecuteNonQuery();
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
        public static bool DeleteDB(string TABEL, string Req1, object ValueReq1, string Req2, object ValueReq2)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    if (Req1 != null)
                    {
                        Command.Parameters.AddWithValue("@Req1", ValueReq1);
                    }
                    if (Req2 != null)
                    {
                        Command.Parameters.AddWithValue("@Req2", ValueReq2);
                    }
                    if (Req1 != null && Req2 == null)
                    {
                        Command.CommandText = $"DELETE FROM {TABEL} WHERE {Req1}=@Req1";
                    }
                    else if (Req2 != null && Req1 == null)
                    {
                        Command.CommandText = $"DELETE FROM {TABEL} WHERE {Req2}=@Req2";
                    }
                    else if (Req2 != null && Req1 != null)
                    {
                        Command.CommandText = $"DELETE FROM {TABEL} WHERE {Req1}=@Req1 AND {Req2}=@Req2";
                    }
                    Command.ExecuteNonQuery();
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
        public static bool DeleteDB(string TABEL, string Req1, object[] ValueReq1, string Req2, object ValueReq2)
        {
            if (ValueReq1.Length == 0)
            {
                return false;
            }
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                using (NpgsqlCommand Command = Connection.CreateCommand())
                {
                    Connection.Open();
                    Command.CommandType = CommandType.Text;
                    string Loaded = "";
                    List<string> Parameters = new List<string>();
                    for (int i = 0; i < ValueReq1.Length; i++)
                    {
                        object Obj = ValueReq1[i];
                        string Param = "@Value" + i;
                        Command.Parameters.AddWithValue(Param, Obj);
                        Parameters.Add(Param);
                    }
                    Loaded = string.Join(",", Parameters.ToArray());
                    Command.Parameters.AddWithValue("@Req2", ValueReq2);
                    Command.CommandText = $"DELETE FROM {TABEL} WHERE {Req1} in ({Loaded}) AND {Req2}=@Req2";
                    Command.ExecuteNonQuery();
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
        public static uint GetPlayerStatus(AccountStatus status, bool isOnline)
        {
            GetPlayerLocation(status, isOnline, out FriendState state, out int roomId, out int channelId, out int serverId);
            return GetPlayerStatus(roomId, channelId, serverId, (int)state);
        }
        public static uint GetPlayerStatus(int roomId, int channelId, int serverId, int stateId)
        {
            int p1 = (stateId & 0xFF) << 28, p2 = (serverId & 0xFF) << 20, p3 = (channelId & 0xFF) << 12, p4 = roomId & 0xFFF;
            return (uint)(p1 | p2 | p3 | p4);
        }
        public static ulong GetPlayerStatus(int clanFId, int roomId, int channelId, int serverId, int stateId)
        {
            long p1 = (clanFId & 0xFFFFFFFF) << 32, p2 = (stateId & 0xFF) << 28, p3 = (serverId & 0xFF) << 20, p4 = (channelId & 0xFF) << 12, p5 = roomId & 0xFFF;
            return (ulong)(p1 | p2 | p3 | p4 | p5);
        }
        public static ulong GetClanStatus(AccountStatus status, bool isOnline)
        {
            GetPlayerLocation(status, isOnline, out FriendState state, out int roomId, out int channelId, out int serverId, out int clanFId);
            return GetPlayerStatus(clanFId, roomId, channelId, serverId, (int)state);
        }
        public static ulong GetClanStatus(FriendState state)
        {
            return GetPlayerStatus(0, 0, 0, 0, (int)state);
        }
        public static uint GetFriendStatus(FriendModel f)
        {
            PlayerInfo info = f.Info;
            if (info == null)
            {
                return 0;
            }
            FriendState state = 0;
            int serverId = 0, channelId = 0, roomId = 0;
            if (f.Removed)
            {
                state = FriendState.Offline;
            }
            else if (f.State > 0)
            {
                state = (FriendState)f.State;
            }
            else
            {
                GetPlayerLocation(info.Status, info.IsOnline, out state, out roomId, out channelId, out serverId);
            }
            return GetPlayerStatus(roomId, channelId, serverId, (int)state);
        }
        public static uint GetFriendStatus(FriendModel f, FriendState stateN)
        {
            PlayerInfo info = f.Info;
            if (info == null)
            {
                return 0;
            }
            FriendState state = stateN;
            int serverId = 0, channelId = 0, roomId = 0;
            if (f.Removed)
            {
                state = FriendState.Offline;
            }
            else if (f.State > 0)
            {
                state = (FriendState)f.State;
            }
            else if (stateN == 0)
            {
                GetPlayerLocation(info.Status, info.IsOnline, out state, out roomId, out channelId, out serverId);
            }
            return GetPlayerStatus(roomId, channelId, serverId, (int)state);
        }
        public static void GetPlayerLocation(AccountStatus status, bool isOnline, out FriendState state, out int roomId, out int channelId, out int serverId)
        {
            roomId = 0;
            channelId = 0;
            serverId = 0;
            if (isOnline)
            {
                if (status.RoomId != 255)
                {
                    roomId = status.RoomId;
                    channelId = status.ChannelId;
                    state = FriendState.Room;
                }
                else if (status.RoomId == 255 && status.ChannelId != 255)
                {
                    channelId = status.ChannelId;
                    state = FriendState.Lobby;
                }
                else if (status.RoomId == 255 && status.ChannelId == 255)
                {
                    state = FriendState.Online;
                }
                else
                {
                    state = FriendState.Offline;
                }
                if (status.ServerId != 255)
                {
                    serverId = status.ServerId;
                }
            }
            else
            {
                state = FriendState.Offline;
            }
        }
        public static void GetPlayerLocation(AccountStatus status, bool isOnline, out FriendState state, out int roomId, out int channelId, out int serverId, out int clanFId)
        {
            roomId = 0;
            channelId = 0;
            serverId = 0;
            clanFId = 0;
            if (isOnline)
            {
                if (status.RoomId != 255)
                {
                    roomId = status.RoomId;
                    channelId = status.ChannelId;
                    state = FriendState.Room;
                }
                else if ((status.ClanMatchId != 255 || status.RoomId == 255) && status.ChannelId != 255)
                {
                    channelId = status.ChannelId;
                    state = FriendState.Lobby;
                }
                else if (status.RoomId == 255 && status.ChannelId == 255)
                {
                    state = FriendState.Online;
                }
                else
                {
                    state = FriendState.Offline;
                }
                if (status.ServerId != 255)
                {
                    serverId = status.ServerId;
                }
                if (status.ClanMatchId != 255)
                {
                    clanFId = status.ClanMatchId + 1;
                }
            }
            else
            {
                state = FriendState.Offline;
            }
        }
        public static ushort GetMissionCardFlags(int missionId, int cardIdx, byte[] arrayList)
        {
            if (missionId == 0)
            {
                return 0;
            }
            int result = 0;
            List<MissionCardModel> List = MissionCardRAW.GetCards(missionId, cardIdx);
            foreach (MissionCardModel Card in List)
            {
                if (arrayList[Card.ArrayIdx] >= Card.MissionLimit)
                {
                    result |= Card.Flag;
                }
            }
            return (ushort)result;
        }
        public static byte[] GetMissionCardFlags(int missionId, byte[] arrayList)
        {
            if (missionId == 0)
            {
                return new byte[20];
            }
            List<MissionCardModel> List = MissionCardRAW.GetCards(missionId);
            if (List.Count == 0)
            {
                return new byte[20];
            }
            using (SyncServerPacket S = new SyncServerPacket(20))
            {
                int result = 0;
                for (int i = 0; i < 10; i++)
                {
                    List<MissionCardModel> Result = MissionCardRAW.GetCards(List, i);
                    foreach (MissionCardModel Card in Result)
                    {
                        if (arrayList[Card.ArrayIdx] >= Card.MissionLimit)
                        {
                            result |= Card.Flag;
                        }
                    }
                    S.WriteH((ushort)result);
                    result = 0;
                }
                return S.ToArray();
            }
        }
        public static int CountDB(string CommandArgument)
        {
            int Result = 0;
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = CommandArgument;
                    Result = Convert.ToInt32(Command.ExecuteScalar());
                    Command.Dispose();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"[QuerySQL.CountDB] {ex.Message}", LoggerType.Error, ex);
            }
            return Result;
        }
        public static bool ValidateAllPlayersAccount()
        {
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"UPDATE accounts SET online = {false} WHERE online = {true}";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static uint GenStockId(int ItemId)
        {
            return BitConverter.ToUInt32(ReplaceBytes(ItemId), 0);
        }
        private static byte[] ReplaceBytes(int Value)
        {
            byte[] VALUES = BitConverter.GetBytes(Value);
            VALUES[3] = 0x40;
            return VALUES;
        }
        public static T NextOf<T>(IList<T> List, T Item)
        {
            int IndexOf = List.IndexOf(Item);
            return List[IndexOf == List.Count - 1 ? 0 : IndexOf + 1];
        }
        public static T ParseEnum<T>(string Value)
        {
            return (T)Enum.Parse(typeof(T), Value, true);
        }
        public static string ToTitleCase(string Text)
        {
            string FirstWord = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Text.Split(' ')[0].ToLower());
            Text = Text.Replace(Text.Split(' ')[0], FirstWord);
            return Text;
        }
    }
}