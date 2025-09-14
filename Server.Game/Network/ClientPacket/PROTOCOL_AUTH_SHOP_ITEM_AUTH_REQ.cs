using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_ITEM_AUTH_REQ : GameClientPacket
    {
        private long ObjectId;
        private int ItemId;
        private long OldCount;
        private uint Error = 1;
        private readonly Random GetRandom = new Random();
        private readonly object SyncLock = new object();
        public PROTOCOL_AUTH_SHOP_ITEM_AUTH_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ObjectId = ReadUD();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                ItemsModel ItemObject = Player.Inventory.GetItem(ObjectId);
                if (ItemObject != null)
                {
                    ItemId = ItemObject.Id;
                    OldCount = ItemObject.Count;
                    if (ItemObject.Category == ItemCategory.Coupon && Player.Inventory.Items.Count >= 500)
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(0x80001029));
                        return;
                    }
                    if (ItemId == 1800049)
                    {
                        if (DaoManagerSQL.UpdatePlayerKD(Player.PlayerId, 0, 0, Player.Statistic.Season.HeadshotsCount, Player.Statistic.Season.TotalKillsCount))
                        {
                            Player.Statistic.Season.KillsCount = 0;
                            Player.Statistic.Season.DeathsCount = 0;
                            Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_RECORD_ACK(Player.Statistic));
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else if (ItemId == 1800048)
                    {
                        if (DaoManagerSQL.UpdatePlayerMatches(0, 0, 0, 0, Player.Statistic.Season.TotalMatchesCount, Player.PlayerId))
                        {
                            Player.Statistic.Season.Matches = 0;
                            Player.Statistic.Season.MatchWins = 0;
                            Player.Statistic.Season.MatchLoses = 0;
                            Player.Statistic.Season.MatchDraws = 0;
                            Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_RECORD_ACK(Player.Statistic));
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else if (ItemId == 1800050)
                    {
                        if (ComDiv.UpdateDB("player_stat_seasons", "escapes_count", 0, "owner_id", Player.PlayerId))
                        {
                            Player.Statistic.Season.EscapesCount = 0;
                            Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_RECORD_ACK(Player.Statistic));
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else if (ItemId == 1800053)
                    {
                        if (DaoManagerSQL.UpdateClanBattles(Player.ClanId, 0, 0, 0))
                        {
                            ClanModel c = ClanManager.GetClan(Player.ClanId);
                            if (c.Id > 0 && c.OwnerId == Client.PlayerId)
                            {
                                c.Matches = 0;
                                c.MatchWins = 0;
                                c.MatchLoses = 0;
                                Client.SendPacket(new PROTOCOL_CS_RECORD_RESET_RESULT_ACK());
                            }
                            else
                            {
                                Error = 0x80000000;
                            }
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else if (ItemId == 1800055)
                    {
                        ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                        if (Clan.Id > 0 && Clan.OwnerId == Client.PlayerId)
                        {
                            if (Clan.MaxPlayers + 50 <= 250 && ComDiv.UpdateDB("system_clan", "max_players", Clan.MaxPlayers + 50, "id", Player.ClanId))
                            {
                                Clan.MaxPlayers += 50;
                                Client.SendPacket(new PROTOCOL_CS_REPLACE_PERSONMAX_ACK(Clan.MaxPlayers));
                            }
                            else
                            {
                                Error = 0x80001056;
                            }
                        }
                        else
                        {
                            Error = 0x80001056;
                        }
                    }
                    else if (ItemId == 1800056)
                    {
                        ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                        if (Clan.Id > 0 && Clan.Points != 1000)
                        {
                            if (ComDiv.UpdateDB("system_clan", "points", 1000.0F, "id", Player.ClanId))
                            {
                                Clan.Points = 1000;
                                Client.SendPacket(new PROTOCOL_CS_POINT_RESET_RESULT_ACK());
                            }
                            else
                            {
                                Error = 0x80001056;
                            }
                        }
                        else
                        {
                            Error = 0x80001056;
                        }
                    }
                    else if (ItemId > 1800113 && ItemId < 1800119)
                    {
                        int GoldReceive = ItemId == 1800114 ? 500 : (ItemId == 1800115 ? 1000 : (ItemId == 1800116 ? 5000 : (ItemId == 1800117 ? 10000 : 30000)));
                        if (ComDiv.UpdateDB("accounts", "gold", Player.Gold + GoldReceive, "player_id", Player.PlayerId))
                        {
                            Player.Gold += GoldReceive;
                            Client.SendPacket(new PROTOCOL_SHOP_PLUS_POINT_ACK(GoldReceive, Player.Gold, 0));
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    /*else if (itemId == 1800999)
                    {
                        if (ComDiv.updateDB("accounts", "exp", p._exp + 515999, "player_id", p.player_id))
                        {
                            p._exp += 515999;
                            _client.SendPacket(new PROTOCOL_SHOP_PLUS_TAG_ACK(515999, 0));
                        }
                        else
                        {
                            erro = 0x80000000;
                        }
                    }*/
                    else if (ItemId == 1801145)
                    {
                        int tags = 0;

                        int random = new Random().Next(0, 9);
                        switch (random)
                        {
                            case 0: tags = 1; break;
                            case 1: tags = 2; break;
                            case 2: tags = 3; break;
                            case 3: tags = 4; break;
                            case 4: tags = 5; break;
                            case 5: tags = 10; break;
                            case 6: tags = 15; break;
                            case 7: tags = 25; break;
                            case 8: tags = 30; break;
                            case 9: tags = 50; break;
                        }

                        if (tags > 0)
                        {
                            if (DaoManagerSQL.UpdateAccountTags(Player.PlayerId, Player.Tags + tags))
                            {
                                Player.Tags += tags;
                                Client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, Player));
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_CAPSULE_ACK(new List<ItemsModel>(), ItemId, random));
                            }
                            else
                            {
                                Error = 0x80000000;
                            }
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else if (ItemObject.Category == ItemCategory.Coupon && RandomBoxXML.ContainsBox(ItemId))
                    {
                        RandomBoxModel RBox = RandomBoxXML.GetBox(ItemId);
                        if (RBox != null)
                        {
                            List<RandomBoxItem> SortedLists = RBox.GetSortedList(GetRandomNumber(1, 100));
                            List<RandomBoxItem> RewardLists = RBox.GetRewardList(SortedLists, GetRandomNumber(0, SortedLists.Count));
                            if (RewardLists.Count > 0)
                            {
                                int ItemIdx = RewardLists[0].Index;
                                List<ItemsModel> Rewards = new List<ItemsModel>();
                                foreach (RandomBoxItem Coupon in RewardLists)
                                {
                                    if (Coupon.Item != null)
                                    {
                                        Rewards.Add(Coupon.Item);
                                    }
                                    else if (DaoManagerSQL.UpdateAccountGold(Player.PlayerId, Player.Gold + (int)Coupon.Count))
                                    {
                                        Player.Gold += (int)Coupon.Count;
                                        Client.SendPacket(new PROTOCOL_SHOP_PLUS_POINT_ACK((int)Coupon.Count, Player.Gold, 0));
                                    }
                                    else
                                    {
                                        Error = 0x80000000;
                                        break;
                                    }
                                    if (Coupon.Special)
                                    {
                                        using (PROTOCOL_AUTH_SHOP_JACKPOT_ACK packet = new PROTOCOL_AUTH_SHOP_JACKPOT_ACK(Player.Nickname, ItemId, ItemIdx))
                                        {
                                            GameXender.Client.SendPacketToAllClients(packet);
                                        }
                                    }
                                }
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_CAPSULE_ACK(Rewards, ItemId, ItemIdx));
                                if (Rewards.Count > 0)
                                {
                                    foreach (ItemsModel Item in Rewards)
                                    {
                                        Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Item));
                                    }
                                }
                            }
                            else
                            {
                                Error = 0x80000000;
                            }
                            SortedLists = null;
                            RewardLists = null;
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else
                    {
                        int ItemClass = ComDiv.GetIdStatics(ItemObject.Id, 1);
                        if ((ItemClass >= 1 && ItemClass <= 8) || ItemClass == 15 || ItemClass == 27 || ItemClass == 30 || ItemClass == 32 || ItemClass == 34)
                        {
                            if (ItemObject.Equip == ItemEquipType.Durable)
                            {
                                ItemObject.Equip = ItemEquipType.Temporary;
                                ItemObject.Count = Convert.ToUInt32(DateTimeUtil.Now().AddSeconds(ItemObject.Count).ToString("yyMMddHHmm"));
                                ComDiv.UpdateDB("player_items", "object_id", ObjectId, "owner_id", Player.PlayerId, new string[] { "count", "equip" }, (long)ItemObject.Count, (int)ItemObject.Equip);
                                if (ItemClass == 6)
                                {
                                    CharacterModel Chara = Player.Character.GetCharacter(ItemObject.Id);
                                    if (Chara != null)
                                    {
                                        Client.SendPacket(new PROTOCOL_CHAR_CHANGE_STATE_ACK(Chara));
                                    }
                                }
                            }
                            else
                            {
                                Error = 0x80000000;
                            }
                        }
                        else if (ItemClass == 17)
                        {
                            CouponIncreaseDays(Player, ItemObject.Name);
                        }
                        else if (ItemClass == 20)
                        {
                            CouponIncreaseGold(Player, ItemObject.Id);
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(Error, ItemObject, Player));
            }
            catch (OverflowException ex)
            {
                CLogger.Print($"Obj: {ObjectId} ItemId: {ItemId} Count: {OldCount} PlayerId: {Client.Player} Name: '{Client.Player.Nickname}' {ex.Message}", LoggerType.Error, ex);
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_SHOP_ITEM_AUTH_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private int GetRandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                return GetRandom.Next(min, max);
            }
        }
        private void CouponIncreaseDays(Account Player, string originalName)
        {
            int CouponId = ComDiv.CreateItemId(16, 0, ComDiv.GetIdStatics(ItemId, 3)), days = ComDiv.GetIdStatics(ItemId, 2);
            CouponEffects eff = Player.Effects;
            if (CouponId == 1600065 && ((eff & (CouponEffects.Defense5 | CouponEffects.Defense10 | CouponEffects.Defense20)) > 0) || CouponId == 1600079 && ((eff & (CouponEffects.Defense5 | CouponEffects.Defense10 | CouponEffects.Defense90)) > 0) || CouponId == 1600044 && ((eff & (CouponEffects.Defense5 | CouponEffects.Defense20 | CouponEffects.Defense90)) > 0) || CouponId == 1600030 && ((eff & (CouponEffects.Defense20 | CouponEffects.Defense10 | CouponEffects.Defense90)) > 0) || CouponId == 1600078 && ((eff & (CouponEffects.FullMetalJack | CouponEffects.HollowPoint | CouponEffects.JackHollowPoint)) > 0) || CouponId == 1600032 && ((eff & (CouponEffects.JackHollowPoint | CouponEffects.HollowPointPlus | CouponEffects.FullMetalJack)) > 0) || CouponId == 1600031 && ((eff & (CouponEffects.JackHollowPoint | CouponEffects.HollowPointPlus | CouponEffects.HollowPoint)) > 0) || CouponId == 1600036 && ((eff & (CouponEffects.HollowPoint | CouponEffects.HollowPointPlus | CouponEffects.FullMetalJack)) > 0) || CouponId == 1600028 && eff.HasFlag(CouponEffects.HP5) || CouponId == 1600040 && eff.HasFlag(CouponEffects.HP10))
            {
                Error = 0x80000000;
            }
            else
            {
                ItemsModel Item = Player.Inventory.GetItem(CouponId);
                if (Item == null)
                {
                    bool Changed = Player.Bonus.AddBonuses(CouponId);
                    CouponFlag cupom = CouponEffectXML.GetCouponEffect(CouponId);
                    if (cupom != null && cupom.EffectFlag > 0 && !Player.Effects.HasFlag(cupom.EffectFlag))
                    {
                        Player.Effects |= cupom.EffectFlag;
                        DaoManagerSQL.UpdateCouponEffect(Player.PlayerId, Player.Effects);
                    }
                    if (Changed)
                    {
                        DaoManagerSQL.UpdatePlayerBonus(Player.PlayerId, Player.Bonus.Bonuses, Player.Bonus.FreePass);
                    }
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(CouponId, $"{originalName} [Active]", ItemEquipType.Temporary, Convert.ToUInt32(DateTimeUtil.Now().AddDays(days).ToString("yyMMddHHmm")))));
                }
                else
                {
                    DateTime data = DateTime.ParseExact(Item.Count.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
                    Item.Count = Convert.ToUInt32(data.AddDays(days).ToString("yyMMddHHmm"));
                    ComDiv.UpdateDB("player_items", "count", (long)Item.Count, "object_id", Item.ObjectId, "owner_id", Player.PlayerId);
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(2, Player, Item));
                }
            }
        }
        private void CouponIncreaseGold(Account p, int cupomId)
        {
            int gold = ComDiv.GetIdStatics(cupomId, 3) * 100;
            gold += ComDiv.GetIdStatics(cupomId, 2) * 100000;
            if (DaoManagerSQL.UpdateAccountGold(p.PlayerId, p.Gold + (gold)))
            {
                p.Gold += gold;
                Client.SendPacket(new PROTOCOL_SHOP_PLUS_POINT_ACK(gold, p.Gold, 0));
            }
            else
            {
                Error = 0x80000000;
            }
        }
    }
}