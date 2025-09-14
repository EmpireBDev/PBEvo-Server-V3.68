using Plugin.Core.Network;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Plugin.Core.Utility;
using Plugin.Core.SQL;
using Plugin.Core.Models;
using Plugin.Core.Enums;

namespace Plugin.Core.Managers
{
    public static class ShopManager
    {
        public static List<ItemsRepair> ItemRepairs = new List<ItemsRepair>();
        public static List<BattleBoxModel> BattleBoxes = new List<BattleBoxModel>();
        public static List<GoodsItem> ShopAllList = new List<GoodsItem>();
        public static List<GoodsItem> ShopBuyableList = new List<GoodsItem>();
        public static SortedList<int, GoodsItem> ShopUniqueList = new SortedList<int, GoodsItem>();
        public static List<ShopData> ShopDataMt1 = new List<ShopData>();
        public static List<ShopData> ShopDataMt2 = new List<ShopData>();
        public static List<ShopData> ShopDataGoods = new List<ShopData>();
        public static List<ShopData> ShopDataItems = new List<ShopData>();
        public static List<ShopData> ShopDataItemRepairs = new List<ShopData>();
        public static List<ShopData> ShopDataBattleBoxes = new List<ShopData>();
        public static byte[] ShopTagData;
        public static int TotalGoods;
        public static int TotalItems;
        public static int TotalMatching1;
        public static int TotalMatching2;
        public static int TotalRepairs;
        public static int TotalBoxes;
        public static int Set4p;
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int limit)
        {
            return list.Select((item, inx) => new { item, inx }).GroupBy(x => x.inx / limit).Select(g => g.Select(x => x.item));
        }
        public static void Load(int Type)
        {
            LoadItemRepair(Type);
            LoadItemGoods(Type);
            LoadItemGoodEffects(Type);
            LoadItemGoodSets(Type);
            LoadBattleBox(Type);
            if (Type == 1)
            {
                try
                {
                    LoadDataMatching1Goods(0);
                    LoadDataMatching2(1);
                    LoadDataItems();
                    LoadDataItemRepairs();
                    LoadDataBattleBoxes();
                    TagShopData();
                }
                catch (Exception Ex)
                {
                    CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                }
                CLogger.Print($"Plugin Loaded: {ShopBuyableList.Count} Buyable Items", LoggerType.Info);
                CLogger.Print($"Plugin Loaded: {ItemRepairs.Count} Repairable Items", LoggerType.Info);
            }
        }
        private static void LoadItemGoods(int Type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    Connection.Open();
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT * FROM system_shop";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        int ItemId = int.Parse(Data["item_id"].ToString());
                        string[] ItemCountList = Data["Item_count_list"].ToString().Contains(",") ? Data["Item_count_list"].ToString().Split(new char[] { ',' }) : new string[] { Data["Item_count_list"].ToString() };
                        string[] ItemCashList = Data["price_cash_list"].ToString().Contains(",") ? Data["price_cash_list"].ToString().Split(new char[] { ',' }) : new string[] { Data["price_cash_list"].ToString() };
                        string[] ItemGoldList = Data["price_gold_list"].ToString().Contains(",") ? Data["price_gold_list"].ToString().Split(new char[] { ',' }) : new string[] { Data["price_gold_list"].ToString() };
                        if ((ItemCountList.Length != ItemCashList.Length) || (ItemCashList.Length != ItemGoldList.Length))
                        {
                            CLogger.Print("Loading goods with invalid counts / moneys / points sizes.", LoggerType.Warning);
                        }
                        else
                        {
                            int Index = 0;
                            foreach (string ItemCount in ItemCountList)
                            {
                                Index += 1;
                                if (!uint.TryParse(ItemCount, out uint Count))
                                {
                                    CLogger.Print($"Loading goods with count != uint32 ({ItemId})", LoggerType.Warning);
                                }
                                else if (!int.TryParse(ItemCashList[Index - 1], out int Cash))
                                {
                                    CLogger.Print($"Loading goods with cash != int32 ({ItemId})", LoggerType.Warning);
                                }
                                else if (!int.TryParse(ItemGoldList[Index - 1], out int Gold))
                                {
                                    CLogger.Print($"Loading goods with gold != int32 ({ItemId})", LoggerType.Warning);
                                }
                                else
                                {
                                    string ItemName = Data["item_name"].ToString();
                                    GoodsItem Good = new GoodsItem()
                                    {
                                        Id = int.Parse($"{ItemId}0{Index}"),
                                        PriceGold = Gold,
                                        PriceCash = Cash
                                    };
                                    int DiscountPercent = int.Parse(Data["discount_percent"].ToString());
                                    if ((DiscountPercent > 0) && (Good.PriceCash > 0))
                                    {
                                        Good.PriceCash = (int)Math.Round((double)(((Good.PriceCash) / 100d) * (100 - DiscountPercent)));
                                    }
                                    if ((DiscountPercent > 0) && (Good.PriceGold > 0))
                                    {
                                        Good.PriceGold = (int)Math.Round((double)(((Good.PriceGold) / 100d) * (100 - DiscountPercent)));
                                    }
                                    Good.Tag = (DiscountPercent > 0 ? ItemTag.Sale : (ItemTag)int.Parse(Data["shop_tag"].ToString()));
                                    Good.Title = int.Parse(Data["title_requi"].ToString());
                                    Good.AuthType = int.Parse(Data["item_consume"].ToString());
                                    Good.BuyType2 = Good.AuthType == 2 ? 1 : (IsRepairableItem(ItemId) ? 2 : 1); // 1 normal 2 repairable | only for durable item | != temporary
                                    Good.BuyType3 = Good.AuthType == 1 ? 2 : 1;
                                    Good.Visibility = bool.Parse(Data["item_visible"].ToString()) ? 0 : 4;
                                    Good.Item.SetItemId(ItemId);
                                    Good.Item.Name = (Good.AuthType == 1 ? ItemName + " (" + Count + " qty)" : Good.AuthType == 2 ? ItemName + " (" + (Count / 3600) + " hours)" : ItemName);
                                    Good.Item.Count = Count;
                                    int ItemClassIDX = ComDiv.GetIdStatics(Good.Item.Id, 1);
                                    if (Type == 1 || Type == 2 && ItemClassIDX == 16)
                                    {
                                        ShopAllList.Add(Good);
                                        if (Good.Visibility != 2 && Good.Visibility != 4)
                                        {
                                            ShopBuyableList.Add(Good);
                                        }
                                        if (!ShopUniqueList.ContainsKey(Good.Item.Id) && Good.AuthType > 0)
                                        {
                                            ShopUniqueList.Add(Good.Item.Id, Good);
                                            if (Good.Visibility == 4)
                                            {
                                                Set4p++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
        private static void LoadItemGoodEffects(int Type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    Connection.Open();
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT * FROM system_shop_effects";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        int CouponId = int.Parse(Reader["coupon_id"].ToString());
                        string[] CouponCountDayList = Reader["coupon_count_day_list"].ToString().Contains(",") ? Reader["coupon_count_day_list"].ToString().Split(new char[] { ',' }) : new string[] { Reader["coupon_count_day_list"].ToString() };
                        string[] CouponCashList = Reader["price_cash_list"].ToString().Contains(",") ? Reader["price_cash_list"].ToString().Split(new char[] { ',' }) : new string[] { Reader["price_cash_list"].ToString() };
                        string[] CouponGoldList = Reader["price_gold_list"].ToString().Contains(",") ? Reader["price_gold_list"].ToString().Split(new char[] { ',' }) : new string[] { Reader["price_gold_list"].ToString() };
                        if ((CouponCountDayList.Length != CouponCashList.Length) || (CouponCashList.Length != CouponGoldList.Length))
                        {
                            CLogger.Print("Loading goods with invalid counts / moneys / points sizes.", LoggerType.Warning);
                        }
                        else
                        {
                            int Index = 0;
                            foreach (string CouponDay in CouponCountDayList)
                            {
                                Index += 1;
                                if (!int.TryParse(CouponDay, out int CountDay))
                                {
                                    CLogger.Print($"Loading effects with count != int32 ({CouponId})", LoggerType.Warning);
                                }
                                else if (!int.TryParse(CouponCashList[Index - 1], out int Cash))
                                {
                                    CLogger.Print($"Loading effects with cash != int32 ({CouponId})", LoggerType.Warning);
                                }
                                else if (!int.TryParse(CouponGoldList[Index - 1], out int Gold))
                                {
                                    CLogger.Print($"Loading effects with gold != int32 ({CouponId})", LoggerType.Warning);
                                }
                                else
                                {
                                    if (CountDay >= 100)
                                    {
                                        CountDay = 100;
                                    }
                                    int EffectId = int.Parse($"{CouponId.ToString().Substring(0, 2)}{CountDay:D2}{CouponId.ToString().Substring(4, 3)}");
                                    GoodsItem Good = new GoodsItem()
                                    {
                                        Id = int.Parse($"{CouponId}0{Index}"),
                                        PriceGold = Gold,
                                        PriceCash = Cash
                                    };
                                    int DiscountPercent = int.Parse(Reader["discount_percent"].ToString());
                                    if ((DiscountPercent > 0) && (Good.PriceCash > 0))
                                    {
                                        Good.PriceCash = (int)Math.Round((double)(((Good.PriceCash) / 100d) * (100 - DiscountPercent)));
                                    }
                                    if ((DiscountPercent > 0) && (Good.PriceGold > 0))
                                    {
                                        Good.PriceGold = (int)Math.Round((double)(((Good.PriceGold) / 100d) * (100 - DiscountPercent)));
                                    }
                                    Good.Tag = (DiscountPercent > 0 ? ItemTag.Sale : (ItemTag)int.Parse(Reader["shop_tag"].ToString()));
                                    Good.Title = 0;
                                    Good.AuthType = 1;
                                    Good.BuyType2 = 1;
                                    Good.BuyType3 = 2;
                                    Good.Visibility = bool.Parse(Reader["coupon_visible"].ToString()) ? 0 : 4;
                                    Good.Item.SetItemId(EffectId);
                                    Good.Item.Name = $"{Reader["coupon_name"]} ({CountDay} days)";
                                    Good.Item.Count = 1;
                                    int ItemClassIDX = ComDiv.GetIdStatics(Good.Item.Id, 1);
                                    if ((Type == 1) || ((Type == 2) && (ItemClassIDX == 16)))
                                    {
                                        ShopAllList.Add(Good);
                                        if ((Good.Visibility != 2) && (Good.Visibility != 4))
                                        {
                                            ShopBuyableList.Add(Good);
                                        }
                                        if (!ShopUniqueList.ContainsKey(Good.Item.Id) && (Good.AuthType > 0))
                                        {
                                            ShopUniqueList.Add(Good.Item.Id, Good);
                                            if (Good.Visibility == 4)
                                            {
                                                Set4p++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Command.Dispose();
                    Reader.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private static void LoadItemGoodSets(int Type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    Connection.Open();
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = $"SELECT * FROM system_shop_sets WHERE visible = '{true}';";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        int SetId = int.Parse(Reader["id"].ToString());
                        string Name = Reader["name"].ToString();
                        LoadSetPackage(SetId, Name, Type);
                    }
                    Command.Dispose();
                    Reader.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private static void LoadSetPackage(int SetId, string SetName, int type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    Connection.Open();
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = $"SELECT * FROM system_shop_sets_items WHERE set_id = '{SetId}' AND set_name = '{SetName}';";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        int Id = int.Parse(Reader["id"].ToString());
                        string Name = Reader["name"].ToString();
                        int Consume = int.Parse(Reader["consume"].ToString());
                        uint Count = uint.Parse(Reader["count"].ToString());
                        int PriceGold = int.Parse(Reader["price_gold"].ToString());
                        int PriceCash = int.Parse(Reader["price_cash"].ToString());
                        GoodsItem Good = new GoodsItem()
                        {
                            Id = SetId,
                            PriceGold = PriceGold,
                            PriceCash = PriceCash,
                            Tag = ItemTag.Hot,
                            Title = 0,
                            AuthType = 0,
                            BuyType2 = 1,
                            BuyType3 = Consume == 1 ? 2 : 1,
                            Visibility = 4
                        };
                        Good.Item.SetItemId(Id);
                        Good.Item.Name = Name;
                        Good.Item.Count = Count;
                        int ItemClassIDX = ComDiv.GetIdStatics(Good.Item.Id, 1);
                        if (type == 1 || type == 2 && ItemClassIDX == 16)
                        {
                            ShopAllList.Add(Good);
                            if ((Good.Visibility != 2) && (Good.Visibility != 4))
                            {
                                ShopBuyableList.Add(Good);
                            }
                            if (!ShopUniqueList.ContainsKey(Good.Item.Id) && (Good.AuthType > 0))
                            {
                                ShopUniqueList.Add(Good.Item.Id, Good);
                                if (Good.Visibility == 4)
                                {
                                    Set4p++;
                                }
                            }
                        }
                    }
                    Command.Dispose();
                    Reader.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private static void LoadItemRepair(int Type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    Connection.Open();
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT * FROM system_shop_repair";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        ItemsRepair Item = new ItemsRepair()
                        {
                            Id = int.Parse(Data["item_id"].ToString()),
                            Point = int.Parse(Data["price_gold"].ToString()),
                            Cash = int.Parse(Data["price_cash"].ToString()),
                            Quantity = uint.Parse(Data["quantity"].ToString()),
                            Enable = bool.Parse(Data["repairable"].ToString())
                        };
                        if (Item.Quantity > 100)
                        {
                            CLogger.Print($"You can't put the Item Id: {Item.Id} repair qty more than 100! Current qty: {Item.Quantity}", LoggerType.Warning);
                            return;
                        }
                        if (Type == 1 && Item.Enable)
                        {
                            ItemRepairs.Add(Item);
                        }
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
        public static void LoadBattleBox(int Type)
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    Connection.Open();
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT * FROM system_item_battlebox";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        BattleBoxModel BattleBox = new BattleBoxModel()
                        {
                            Id = int.Parse(Data["id"].ToString()),
                            Name = Data["name"].ToString(),
                            Tags = int.Parse(Data["req_tags"].ToString()),
                            Items = new List<BattleBoxItem>()
                        };
                        if (Type == 1)
                        {
                            BattleBoxes.Add(BattleBox);
                        }
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
        public static void Reset()
        {
            Set4p = 0;
            ShopAllList.Clear();
            ShopBuyableList.Clear();
            ShopUniqueList.Clear();
            ShopDataMt1.Clear();
            ShopDataMt2.Clear();
            ShopDataGoods.Clear();
            ShopDataItems.Clear();
            ShopDataItemRepairs.Clear();
            ShopDataBattleBoxes.Clear();
            ItemRepairs.Clear();
            BattleBoxes.Clear();
            TotalGoods = 0;
            TotalItems = 0;
            TotalMatching1 = 0;
            TotalMatching2 = 0;
            TotalRepairs = 0;
            TotalBoxes = 0;
        }
        private static void LoadDataMatching1Goods(int cafe)
        {
            List<GoodsItem> Matchs = new List<GoodsItem>();
            List<GoodsItem> Goods = new List<GoodsItem>();
            lock (ShopAllList)
            {
                foreach (GoodsItem Good in ShopAllList)
                {
                    if (Good.Item.Count == 0)
                    {
                        continue;
                    }
                    if (!(Good.Tag == ItemTag.PcCafe && cafe == 0) && (Good.Tag == ItemTag.PcCafe && cafe > 0 || Good.Visibility != 2))
                    {
                        Matchs.Add(Good);
                    }
                    if (Good.Visibility < 2 || Good.Visibility == 4)
                    {
                        Goods.Add(Good);
                    }
                }
            }
            TotalMatching1 = Matchs.Count;
            TotalGoods = Goods.Count;
            int Pages = (int)Math.Ceiling(Matchs.Count / 500d);
            int Count = 0;
            for (int i = 0; i < Pages; i++)
            {
                byte[] Buffer = GetMatchingData(500, i, ref Count, Matchs);
                ShopData Data = new ShopData()
                {
                    Buffer = Buffer,
                    ItemsCount = Count,
                    Offset = (i * 500)
                };
                ShopDataMt1.Add(Data);
            }
            Pages = (int)Math.Ceiling(Goods.Count / 50d);
            for (int i = 0; i < Pages; i++)
            {
                byte[] Buffer = GetGoodsData(50, i, ref Count, Goods);
                ShopData Data = new ShopData()
                {
                    Buffer = Buffer,
                    ItemsCount = Count,
                    Offset = (i * 50)
                };
                ShopDataGoods.Add(Data);
            }
        }
        private static void LoadDataMatching2(int cafe)
        {
            List<GoodsItem> Matchs = new List<GoodsItem>();
            lock (ShopAllList)
            {
                foreach (GoodsItem Good in ShopAllList)
                {
                    if (Good.Item.Count == 0)
                    {
                        continue;
                    }
                    if (!(Good.Tag == ItemTag.PcCafe && cafe == 0) && (Good.Tag == ItemTag.PcCafe && cafe > 0 || Good.Visibility != 2))
                    {
                        Matchs.Add(Good);
                    }
                }
            }
            TotalMatching2 = Matchs.Count;
            int Pages = (int)Math.Ceiling(Matchs.Count / 500d);
            int Count = 0;
            for (int i = 0; i < Pages; i++)
            {
                byte[] Buffer = GetMatchingData(500, i, ref Count, Matchs);
                ShopData Data = new ShopData()
                {
                    Buffer = Buffer,
                    ItemsCount = Count,
                    Offset = (i * 500)
                };
                ShopDataMt2.Add(Data);
            }
        }
        private static void LoadDataItems()
        {
            List<GoodsItem> Items = new List<GoodsItem>();
            lock (ShopUniqueList)
            {
                foreach (GoodsItem Good in ShopUniqueList.Values)
                {
                    if (Good.Visibility != 1 && Good.Visibility != 3)
                    {
                        Items.Add(Good);
                    }
                }
            }
            TotalItems = Items.Count;
            int ItemsPages = (int)Math.Ceiling(Items.Count / 800d);
            int Count = 0;
            for (int i = 0; i < ItemsPages; i++)
            {
                byte[] Buffer = GetItemsData(800, i, ref Count, Items);
                ShopData Data = new ShopData()
                {
                    Buffer = Buffer,
                    ItemsCount = Count,
                    Offset = (i * 800)
                };
                ShopDataItems.Add(Data);
            }
        }
        private static void LoadDataItemRepairs()
        {
            List<ItemsRepair> Items = new List<ItemsRepair>();
            lock (ItemRepairs)
            {
                foreach (ItemsRepair Model in ItemRepairs)
                {
                    Items.Add(Model);
                }
            }
            TotalRepairs = Items.Count;
            int RepairsPages = (int)Math.Ceiling(Items.Count / 100d);
            int Count = 0;
            for (int i = 0; i < RepairsPages; i++)
            {
                byte[] Buffer = GetRepairsData(100, i, ref Count, Items);
                ShopData Data = new ShopData()
                {
                    Buffer = Buffer,
                    ItemsCount = Count,
                    Offset = (i * 100)
                };
                ShopDataItemRepairs.Add(Data);
            }
        }
        private static void LoadDataBattleBoxes()
        {
            List<BattleBoxModel> Boxes = new List<BattleBoxModel>();
            lock (BattleBoxes)
            {
                foreach (BattleBoxModel Model in BattleBoxes)
                {
                    Boxes.Add(Model);
                }
            }
            TotalBoxes = Boxes.Count;
            int BattleBoxPages = (int)Math.Ceiling(Boxes.Count / 100d);
            int Count = 0;
            for (int i = 0; i < BattleBoxPages; i++)
            {
                byte[] Buffer = GetBoxesData(100, i, ref Count, Boxes);
                ShopData Data = new ShopData()
                {
                    Buffer = Buffer,
                    ItemsCount = Count,
                    Offset = (i * 100)
                };
                ShopDataBattleBoxes.Add(Data);
            }
        }
        private static byte[] GetItemsData(int Max, int Page, ref int Count, List<GoodsItem> List)
        {
            Count = 0;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (Page * Max); i < List.Count; i++)
                {
                    WriteItemsData(List[i], S);
                    if (++Count == Max)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private static byte[] GetGoodsData(int Max, int Page, ref int Count, List<GoodsItem> List)
        {
            Count = 0;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (Page * Max); i < List.Count; i++)
                {
                    WriteGoodsData(List[i], S);
                    if (++Count == Max)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private static byte[] GetRepairsData(int Max, int Page, ref int Count, List<ItemsRepair> List)
        {
            Count = 0;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (Page * Max); i < List.Count; i++)
                {
                    WriteRepairsData(List[i], S);
                    if (++Count == Max)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private static byte[] GetBoxesData(int Max, int Page, ref int Count, List<BattleBoxModel> List)
        {
            Count = 0;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (Page * Max); i < List.Count; i++)
                {
                    WriteBoxesData(List[i], S);
                    if (++Count == Max)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private static byte[] GetMatchingData(int Max, int Page, ref int Count, List<GoodsItem> List)
        {
            Count = 0;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (Page * Max); i < List.Count; i++)
                {
                    WriteMatchData(List[i], S);
                    if (++Count == Max)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private static void WriteItemsData(GoodsItem Good, SyncServerPacket S)
        {
            S.WriteD(Good.Item.Id);
            S.WriteC((byte)Good.AuthType);
            S.WriteC((byte)Good.BuyType2);
            S.WriteC((byte)Good.BuyType3);
            S.WriteC((byte)Good.Title); //req rank or archivements level
            S.WriteC((byte)(Good.Title != 0 ? 2 : 0)); // 1 archivement 2 Title
            S.WriteH(0); //archivement id
        }
        private static void WriteGoodsData(GoodsItem Good, SyncServerPacket S)
        {
            S.WriteD(Good.Id);
            S.WriteC(1); // ??
            S.WriteC((byte)(Good.Visibility == 4 ? 4 : 1)); //Flag1 = Show icon + Buy option | Flag2 = UNK | Flag4 = Show icon + No buy option
            S.WriteD(Good.PriceGold);
            S.WriteD(Good.PriceCash);
            S.WriteD(0);
            S.WriteC((byte)Good.Tag);
            S.WriteB(new byte[91]);
        }
        private static void WriteRepairsData(ItemsRepair Repair, SyncServerPacket S)
        {
            S.WriteD(Repair.Id);
            S.WriteD(Repair.Point);
            S.WriteD(Repair.Cash);
            S.WriteD(Repair.Quantity);
        }
        private static void WriteMatchData(GoodsItem Good, SyncServerPacket S)
        {
            S.WriteD(Good.Id);
            S.WriteD(Good.Item.Id);
            S.WriteD(Good.Item.Count);
            S.WriteD(0);
        }
        private static void WriteBoxesData(BattleBoxModel BattleBox, SyncServerPacket S)
        {
            S.WriteD(BattleBox.Id);
            S.WriteH((ushort)BattleBox.Tags);
            S.WriteC(0); //Unk
        }
        public static bool IsRepairableItem(int ItemId)
        {
            return GetRepairItem(ItemId) != null;
        }
        public static ItemsRepair GetRepairItem(int ItemId)
        {
            if (ItemId == 0)
            {
                return null;
            }
            lock (ItemRepairs)
            {
                foreach (ItemsRepair Item in ItemRepairs)
                {
                    if (Item.Id == ItemId)
                    {
                        return Item;
                    }
                }
            }
            return null;
        }
        public static BattleBoxModel GetBattleBox(int ItemId)
        {
            if (ItemId == 0)
            {
                return null;
            }
            lock (BattleBoxes)
            {
                foreach (BattleBoxModel BattleBox in BattleBoxes)
                {
                    if (BattleBox.Id == ItemId)
                    {
                        return BattleBox;
                    }
                }
            }
            return null;
        }
        public static bool IsBlocked(string Text, List<int> Items) //For Tournament Rules
        {
            lock (ShopUniqueList)
            {
                foreach (GoodsItem Good in ShopUniqueList.Values)
                {
                    if (!Items.Contains(Good.Item.Id) && Good.Item.Name.Contains(Text)) //Counts inside the goods the values ​​written in rules
                    {
                        Items.Add(Good.Item.Id); //Add matching ids to list items
                    }
                }
            }
            return false;
        }
        public static GoodsItem GetGood(int GoodId)
        {
            if (GoodId == 0)
            {
                return null;
            }
            lock (ShopAllList)
            {
                foreach (GoodsItem Good in ShopAllList)
                {
                    if (Good.Id == GoodId)
                    {
                        return Good;
                    }
                }
            }
            return null;
        }
        public static GoodsItem GetItemId(int ItemId)
        {
            if (ItemId == 0)
            {
                return null;
            }
            lock (ShopAllList)
            {
                foreach (GoodsItem Good in ShopAllList)
                {
                    if (Good.Item.Id == ItemId)
                    {
                        return Good;
                    }
                }
            }
            return null;
        }
        public static List<GoodsItem> GetGoods(List<CartGoods> ShopCart, out int GoldPrice, out int CashPrice, out int TagsPrice)
        {
            GoldPrice = 0;
            CashPrice = 0;
            TagsPrice = 0;
            List<GoodsItem> Items = new List<GoodsItem>();
            if (ShopCart.Count == 0)
            {
                return Items;
            }
            lock (ShopBuyableList)
            {
                foreach (GoodsItem Good in ShopBuyableList)
                {
                    foreach (CartGoods CartGood in ShopCart)
                    {
                        if (CartGood.GoodId == Good.Id)
                        {
                            Items.Add(Good);
                            if (CartGood.BuyType == 1)
                            {
                                GoldPrice += Good.PriceGold;
                            }
                            else if (CartGood.BuyType == 2)
                            {
                                CashPrice += Good.PriceCash;
                            }
                        }
                    }
                }
            }
            return Items;
        }
        private static void TagShopData()
        {
            string Tag = "zOne";
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteS(Tag, Tag.Length + 1);
                ShopTagData = S.ToArray();
            }
        }
    }
}
