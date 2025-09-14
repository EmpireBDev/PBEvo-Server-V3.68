using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;
using System;
using System.Collections.Generic;

namespace Server.Auth.Data.Utils
{
    public class AllUtils
    {
        public static void LoadPlayerInventory(Account Player)
        {
            lock (Player.Inventory.Items)
            {
                Player.Inventory.Items.AddRange(DaoManagerSQL.GetPlayerInventoryItems(Player.PlayerId));
            }
        }
        public static void LoadPlayerMissions(Account Player)
        {
            PlayerMissions Mission = DaoManagerSQL.GetPlayerMissionsDB(Player.PlayerId, Player.Mission.Mission1, Player.Mission.Mission2, Player.Mission.Mission3, Player.Mission.Mission4);
            if (Mission != null)
            {
                Player.Mission = Mission;
            }
            else if (!DaoManagerSQL.CreatePlayerMissionsDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Missions!", LoggerType.Warning);
            }
        }
        public static void ValidatePlayerInventoryStatus(Account Player)
        {
            Player.Inventory.LoadBasicItems();
            if (Player.Rank >= 46) //Brigadier | Star 1
            {
                Player.Inventory.LoadGeneralBeret();
            }
            if (Player.IsGM()) //MOD, GM, GM Access Level
            {
                Player.Inventory.LoadHatForGM();
            }
            PlayerInternetCafe(Player);
        }
        private static void PlayerInternetCafe(Account Player)
        {
            bool ICafePlayerStatus = CheckICafeStatus(Player, out string Reason);
            if (ICafePlayerStatus)
            {
                Player.Inventory.LoadCafeItems();
                lock (TemplatePackXML.CafePCs)
                {
                    foreach (ItemsModel Item in TemplatePackXML.CafePCs)
                    {
                        int CouponStatic = ComDiv.GetIdStatics(Item.Id, 1);
                        if (CouponStatic != 16)
                        {
                            return;
                        }
                        CouponFlag cupom = CouponEffectXML.GetCouponEffect(Item.Id);
                        if (cupom != null && cupom.EffectFlag > 0 && !Player.Effects.HasFlag(cupom.EffectFlag))
                        {
                            Player.Effects |= cupom.EffectFlag;
                            DaoManagerSQL.UpdateCouponEffect(Player.PlayerId, Player.Effects);
                        }
                    }
                }
            }
            else
            {
                if (Player.CafePC > 0 && ComDiv.UpdateDB("accounts", "pc_cafe", (int)CafeEnum.None, "player_id", Player.PlayerId))
                {
                    Player.CafePC = CafeEnum.None;
                    if (!string.IsNullOrEmpty(Reason) && ComDiv.DeleteDB("player_vip", "owner_id", Player.PlayerId))
                    {
                        CLogger.Print($"VIP for UID: {Player.PlayerId} Nick: {Player.Nickname} Deleted Due To {Reason}", LoggerType.Info);
                    }
                    CLogger.Print($"Player PC Cafe was resetted by default into '{Player.CafePC}'; (UID: {Player.PlayerId} Nick: {Player.Nickname})", LoggerType.Info);
                }
            }
        }
        private static bool CheckICafeStatus(Account Player, out string Reason)
        {
            bool IsLevelModeGM = (Player.HaveGMLevel() && Player.CafePC > CafeEnum.None);
            bool IsCafeRewardValid = CheckCafeAccessReward(Player, out string Result);
            if (IsLevelModeGM) //No Limited Time Used In This Case
            {
                Reason = $"Valid Access: {Player.Access} PC Cafe: {Player.CafePC} (MOD/GM ONLY)";
                return true;
            }
            else
            {
                PlayerVip Vip = DaoManagerSQL.GetPlayerVIP(Player.PlayerId);
                if (Vip != null && IsCafeRewardValid)
                {
                    if (Vip.Expirate < uint.Parse(DateTimeUtil.Now("yyMMddHHmm")))
                    {
                        Reason = "The Time Has Expired!";
                        return false;
                    }
                    string PlayerAddress = DaoManagerSQL.GetPlayerIP4Address(Player.PlayerId);
                    if (!string.IsNullOrEmpty(PlayerAddress) && Vip.Address.Equals(PlayerAddress))
                    {
                        string Benefit = Player.CafePC.ToString();
                        if (!Vip.Benefit.Equals(Benefit) && ComDiv.UpdateDB("player_vip", "last_benefit", Benefit, "owner_id", Player.PlayerId))
                        {
                            Vip.Benefit = Benefit;
                        }
                        Reason = Result;
                        return true;
                    }
                    else if (!ConfigLoader.ICafeSystem)
                    {
                        Reason = "Disabled ICAFE";
                        return true;
                    }
                    else
                    {
                        Reason = $"{Result} With Invalid Configuration!";
                        return false;
                    }
                }
                else
                {
                    Reason = $"{Result}. Database Not Found!";
                    return false;
                }
            }
        }
        private static bool CheckCafeAccessReward(Account Player, out string Result)
        {
            if (Player.Access.Equals(AccessLevel.CAFE) && (Player.CafePC == CafeEnum.Silver || Player.CafePC == CafeEnum.Gold))
            {
                Result = $"Valid Access: {Player.Access} PC Cafe: {Player.CafePC} (Only)";
                return true;
            }
            else if ((Player.Access.Equals(AccessLevel.STREAMER) || Player.Access.Equals(AccessLevel.INFLUENCER)) && (Player.CafePC == CafeEnum.Silver || Player.CafePC == CafeEnum.Gold || Player.CafePC == CafeEnum.Unk))
            {
                Result = $"Valid Access: {Player.Access} PC Cafe: {Player.CafePC} (Only)";
                return true;
            }
            else if (Player.Access.Equals(AccessLevel.VIP) && Player.CafePC > CafeEnum.None)
            {
                Result = $"Valid Access: {Player.Access} PC Cafe: {Player.CafePC}";
                return true;
            }
            else
            {
                Result = $"Invalid Access: {Player.Access} For PC Cafe: {Player.CafePC}";
                return false;
            }
        }
        public static void LoadPlayerEquipments(Account Player)
        {
            PlayerEquipment Equipment = DaoManagerSQL.GetPlayerEquipmentsDB(Player.PlayerId);
            if (Equipment != null)
            {
                Player.Equipment = Equipment;
            }
            else if (!DaoManagerSQL.CreatePlayerEquipmentsDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Equipment!", LoggerType.Warning);
            }
        }
        public static void LoadPlayerCharacters(Account Player)
        {
            List<CharacterModel> Characters = DaoManagerSQL.GetPlayerCharactersDB(Player.PlayerId);
            if (Characters.Count > 0)
            {
                Player.Character.Characters = Characters;
            }
        }
        public static void LoadPlayerStatistic(Account Player)
        {
            StatBasic Total = DaoManagerSQL.GetPlayerStatBasicDB(Player.PlayerId);
            if (Total != null)
            {
                Player.Statistic.Basic = Total;
            }
            else if (!DaoManagerSQL.CreatePlayerStatBasicDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Total)!", LoggerType.Warning);
            }
            StatSeason Season = DaoManagerSQL.GetPlayerStatSeasonDB(Player.PlayerId);
            if (Season != null)
            {
                Player.Statistic.Season = Season;
            }
            else if (!DaoManagerSQL.CreatePlayerStatSeasonDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Season)!", LoggerType.Warning);
            }
            StatClan Clan = DaoManagerSQL.GetPlayerStatClanDB(Player.PlayerId);
            if (Clan != null)
            {
                Player.Statistic.Clan = Clan;
            }
            else if (!DaoManagerSQL.CreatePlayerStatClanDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Clan)!", LoggerType.Warning);
            }
            StatDaily Dailies = DaoManagerSQL.GetPlayerStatDailiesDB(Player.PlayerId);
            if (Dailies != null)
            {
                Player.Statistic.Daily = Dailies;
            }
            else if (!DaoManagerSQL.CreatePlayerStatDailiesDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Dailies)!", LoggerType.Warning);
            }
            StatWeapon Weapon = DaoManagerSQL.GetPlayerStatWeaponsDB(Player.PlayerId);
            if (Weapon != null)
            {
                Player.Statistic.Weapon = Weapon;
            }
            else if (!DaoManagerSQL.CreatePlayerStatWeaponsDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Weapons)!", LoggerType.Warning);
            }
            StatAcemode Acemode = DaoManagerSQL.GetPlayerStatAcemodesDB(Player.PlayerId);
            if (Acemode != null)
            {
                Player.Statistic.Acemode = Acemode;
            }
            else if (!DaoManagerSQL.CreatePlayerStatAcemodesDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Acemode)!", LoggerType.Warning);
            }
            StatBattleroyale Battleroyale = DaoManagerSQL.GetPlayerStatBattleroyaleDB(Player.PlayerId);
            if (Battleroyale != null)
            {
                Player.Statistic.Battleroyale = Battleroyale;
            }
            else if (!DaoManagerSQL.CreatePlayerStatBattleroyaleDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Statistic (Battleroyale)!", LoggerType.Warning);
            }
            Player.Statistic.OwnerId = Player.PlayerId;
        }
        public static void LoadPlayerTitles(Account Player)
        {
            PlayerTitles Title = DaoManagerSQL.GetPlayerTitlesDB(Player.PlayerId);
            if (Title != null)
            {
                Player.Title = Title;
            }
            else if (!DaoManagerSQL.CreatePlayerTitlesDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Equipment!", LoggerType.Warning);
            }
        }
        public static void LoadPlayerBonus(Account Player)
        {
            PlayerBonus Bonus = DaoManagerSQL.GetPlayerBonusDB(Player.PlayerId);
            if (Bonus != null)
            {
                Player.Bonus = Bonus;
            }
            else if (!DaoManagerSQL.CreatePlayerBonusDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Bonus!", LoggerType.Warning);
            }
        }
        public static void LoadPlayerFriend(Account Player, bool LoadFulLDatabase)
        {
            List<FriendModel> Friends = DaoManagerSQL.GetPlayerFriendsDB(Player.PlayerId);
            if (Friends.Count > 0)
            {
                Player.Friend.Friends = Friends;
                if (LoadFulLDatabase)
                {
                    AccountManager.GetFriendlyAccounts(Player.Friend);
                }
            }
        }
        public static void LoadPlayerEvent(Account Player)
        {
            PlayerEvent Event = DaoManagerSQL.GetPlayerEventDB(Player.PlayerId);
            if (Event != null)
            {
                Player.Event = Event;
            }
            else if (!DaoManagerSQL.CreatePlayerEventDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Event!", LoggerType.Warning);
            }
        }
        public static void LoadPlayerConfig(Account Player)
        {
            PlayerConfig Config = DaoManagerSQL.GetPlayerConfigDB(Player.PlayerId);
            if (Config != null)
            {
                Player.Config = Config;
            }
            else if (!DaoManagerSQL.CreatePlayerConfigDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Config!", LoggerType.Warning);
            }
        }
        public static void LoadPlayerQuickstarts(Account Player)
        {
            List<QuickstartModel> QuickStarts = DaoManagerSQL.GetPlayerQuickstartsDB(Player.PlayerId);
            if (QuickStarts.Count > 0)
            {
                Player.Quickstart.Quickjoins = QuickStarts;
            }
            else if (!DaoManagerSQL.CreatePlayerQuickstartsDB(Player.PlayerId))
            {
                CLogger.Print("There was an error when creating Player Quickstarts!", LoggerType.Warning);
            }
        }
        public static bool DiscountPlayerItems(Account Player)
        {
            try
            {
                bool UpdateCouponEffect = false;
                uint CurrentDate = Convert.ToUInt32(DateTimeUtil.Now("yyMMddHHmm"));
                List<object> RemovedItems = new List<object>();
                int Bonuses = Player.Bonus != null ? Player.Bonus.Bonuses : 0, FreePass = Player.Bonus != null ? Player.Bonus.FreePass : 0;
                lock (Player.Inventory.Items)
                {
                    for (int i = 0; i < Player.Inventory.Items.Count; i++)
                    {
                        ItemsModel Item = Player.Inventory.Items[i];
                        if (Item.Count <= CurrentDate && Item.Equip == ItemEquipType.Temporary)
                        {
                            if (Item.Category == ItemCategory.Character && ComDiv.GetIdStatics(Item.Id, 1) == 6)
                            {
                                CharacterModel Character = Player.Character.GetCharacter(Item.Id);
                                if (Character != null)
                                {
                                    int Index = 0;
                                    foreach (CharacterModel Chara in Player.Character.Characters)
                                    {
                                        if (Chara.Slot != Character.Slot)
                                        {
                                            Chara.Slot = Index;
                                            DaoManagerSQL.UpdatePlayerCharacter(Index, Chara.ObjectId, Player.PlayerId);
                                            Index++;
                                        }
                                    }
                                    if (DaoManagerSQL.DeletePlayerCharacter(Character.ObjectId, Player.PlayerId))
                                    {
                                        Player.Character.RemoveCharacter(Character);
                                    }
                                }
                            }
                            else if (Item.Category == ItemCategory.Coupon)
                            {
                                if (Player.Bonus == null)
                                {
                                    continue;
                                }
                                bool changed = Player.Bonus.RemoveBonuses(Item.Id);
                                if (!changed)
                                {
                                    if (Item.Id == 1600014)
                                    {
                                        ComDiv.UpdateDB("player_bonus", "crosshair_color", 4, "owner_id", Player.PlayerId);
                                        Player.Bonus.CrosshairColor = 4;
                                    }
                                    else if (Item.Id == 1600006)
                                    {
                                        ComDiv.UpdateDB("accounts", "nick_color", 0, "player_id", Player.PlayerId);
                                        Player.NickColor = 0;
                                    }
                                    else if (Item.Id == 1600009)
                                    {
                                        ComDiv.UpdateDB("player_bonus", "fake_rank", 55, "owner_id", Player.PlayerId);
                                        Player.Bonus.FakeRank = 55;
                                    }
                                    else if (Item.Id == 1600010)
                                    {
                                        if (Player.Bonus.FakeNick.Length > 0)
                                        {
                                            ComDiv.UpdateDB("player_bonus", "fake_nick", "", "owner_id", Player.PlayerId);
                                            ComDiv.UpdateDB("accounts", "nickname", Player.Bonus.FakeNick, "player_id", Player.PlayerId);
                                            Player.Nickname = Player.Bonus.FakeNick;
                                            Player.Bonus.FakeNick = "";
                                        }
                                    }
                                    else if (Item.Id == 1600187)
                                    {
                                        ComDiv.UpdateDB("player_bonus", "muzzle_color", 0, "owner_id", Player.PlayerId);
                                        Player.Bonus.MuzzleColor = 0;
                                    }
                                }
                                CouponFlag Coupon = CouponEffectXML.GetCouponEffect(Item.Id);
                                if (Coupon != null && Coupon.EffectFlag > 0 && Player.Effects.HasFlag(Coupon.EffectFlag))
                                {
                                    Player.Effects -= Coupon.EffectFlag;
                                    UpdateCouponEffect = true;
                                }
                            }
                            RemovedItems.Add(Item.ObjectId);
                            Player.Inventory.Items.RemoveAt(i--);
                        }
                        else if (Item.Count == 0)
                        {
                            RemovedItems.Add(Item.ObjectId);
                            Player.Inventory.Items.RemoveAt(i--);
                        }
                    }
                    ComDiv.DeleteDB("player_items", "object_id", RemovedItems.ToArray(), "owner_id", Player.PlayerId);
                }
                RemovedItems = null;
                if (Player.Bonus != null && (Player.Bonus.Bonuses != Bonuses || Player.Bonus.FreePass != FreePass))
                {
                    DaoManagerSQL.UpdatePlayerBonus(Player.PlayerId, Player.Bonus.Bonuses, Player.Bonus.FreePass);
                }
                if (Player.Effects < 0)
                {
                    Player.Effects = 0;
                }
                if (UpdateCouponEffect)
                {
                    ComDiv.UpdateDB("accounts", "coupon_effect", (long)Player.Effects, "player_id", Player.PlayerId);
                }
                int type = ComDiv.CheckEquipedItems(Player.Equipment, Player.Inventory.Items, false);
                if (type > 0)
                {
                    DBQuery Query = new DBQuery();
                    if ((type & 2) == 2)
                    {
                        ComDiv.UpdateWeapons(Player.Equipment, Query);
                    }
                    if ((type & 1) == 1)
                    {
                        ComDiv.UpdateChars(Player.Equipment, Query);
                    }
                    if ((type & 3) == 3)
                    {
                        ComDiv.UpdateItems(Player.Equipment, Query);
                    }
                    ComDiv.UpdateDB("player_equipments", "owner_id", Player.PlayerId, Query.GetTables(), Query.GetValues());
                    Query = null;
                }
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static void CheckGameEvents(Account Player, EventVisitModel Attendance, uint DateNow)
        {
            PlayerEvent Event = Player.Event;
            if (Event != null)
            {
                EventQuestModel EvQuest = EventQuestSync.GetRunningEvent();
                if (EvQuest != null)
                {
                    uint LastQuestDate = Event.LastQuestDate;
                    int LastQuestFinish = Event.LastQuestFinish;
                    if (Event.LastQuestDate < EvQuest.StartDate)
                    {
                        Event.LastQuestDate = 0;
                        Event.LastQuestFinish = 0;
                    }
                    if (Event.LastQuestFinish == 0)
                    {
                        Player.Mission.Mission4 = 13;
                        if (Event.LastQuestDate == 0)
                        {
                            Event.LastQuestDate = DateNow;
                        }
                    }
                    if (Event.LastQuestDate != LastQuestDate || Event.LastQuestFinish != LastQuestFinish)
                    {
                        EventQuestSync.ResetPlayerEvent(Player.PlayerId, Event);
                    }
                }
                EventLoginModel EvLogin = EventLoginSync.GetRunningEvent();
                if (EvLogin != null && !(EvLogin.BeginDate < Event.LastLoginDate && Event.LastLoginDate < EvLogin.FinishDate))
                {
                    GoodsItem Good = ShopManager.GetGood(EvLogin.GoodId);
                    if (Good != null)
                    {
                        ComDiv.TryCreateItem(new ItemsModel(Good.Item), Player.Inventory, Player.PlayerId);
                    }
                    ComDiv.UpdateDB("player_events", "last_login_date", (long)DateNow, "owner_id", Player.PlayerId);
                }
                if (Attendance != null && Event.LastVisitEventId != Attendance.Id)
                {
                    Event.LastVisitEventId = Attendance.Id;
                    Event.LastVisitSequence1 = 0;
                    Event.LastVisitSequence2 = 0;
                    Event.NextVisitDate = 0;
                    EventVisitSync.ResetPlayerEvent(Player.PlayerId, Attendance.Id);
                }
                EventXmasModel EvChristmas = EventXmasSync.GetRunningEvent();
                if (EvChristmas != null)
                {
                    if (Event.LastXmasRewardDate < EvChristmas.StartDate)
                    {
                        Event.LastXmasRewardDate = 0;
                        ComDiv.UpdateDB("player_events", "last_xmas_reward_date", 0, "owner_id", Player.PlayerId);
                    }
                    if (!(Event.LastXmasRewardDate > EvChristmas.StartDate && Event.LastXmasRewardDate <= EvChristmas.EndDate))
                    {
                        //Christmas = true;
                    }
                }
            }
            ComDiv.UpdateDB("accounts", "last_login_date", (long)DateNow, "player_id", Player.PlayerId);
        }
        public static byte[] LoadCouponEffects(Account Player)
        {
            byte[] Flag = new byte[4];
            if (Player.Effects.HasFlag(CouponEffects.Ammo40) || Player.Effects.HasFlag(CouponEffects.Ammo10) || Player.Effects.HasFlag(CouponEffects.GetDroppedWeapon) || Player.Effects.HasFlag(CouponEffects.QuickChangeWeapon) || Player.Effects.HasFlag(CouponEffects.QuickChangeReload))
            {
                if (Player.Effects.HasFlag(CouponEffects.Ammo40))
                {
                    Flag[0] += 1;
                }
                if (Player.Effects.HasFlag(CouponEffects.Ammo10))
                {
                    Flag[0] += 2;
                }
                if (Player.Effects.HasFlag(CouponEffects.GetDroppedWeapon))
                {
                    Flag[0] += 4;
                }
                if (Player.Effects.HasFlag(CouponEffects.QuickChangeWeapon))
                {
                    Flag[0] += 16;
                }
                if (Player.Effects.HasFlag(CouponEffects.QuickChangeReload))
                {
                    Flag[0] += 64;
                }
            }
            if (Player.Effects.HasFlag(CouponEffects.Invincible) || Player.Effects.HasFlag(CouponEffects.FullMetalJack) || Player.Effects.HasFlag(CouponEffects.HollowPoint) || Player.Effects.HasFlag(CouponEffects.HollowPointPlus) || Player.Effects.HasFlag(CouponEffects.C4SpeedKit))
            {
                if (Player.Effects.HasFlag(CouponEffects.Invincible))
                {
                    Flag[1] += 1;
                }
                if (Player.Effects.HasFlag(CouponEffects.FullMetalJack))
                {
                    Flag[1] += 4;
                }
                if (Player.Effects.HasFlag(CouponEffects.HollowPoint))
                {
                    Flag[1] += 16;
                }
                if (Player.Effects.HasFlag(CouponEffects.HollowPointPlus))
                {
                    Flag[1] += 64;
                }
                if (Player.Effects.HasFlag(CouponEffects.C4SpeedKit))
                {
                    Flag[1] += 128;
                }
            }
            if (Player.Effects.HasFlag(CouponEffects.ExtraGrenade) || Player.Effects.HasFlag(CouponEffects.ExtraThrowGrenade) || Player.Effects.HasFlag(CouponEffects.JackHollowPoint) || Player.Effects.HasFlag(CouponEffects.HP5) || Player.Effects.HasFlag(CouponEffects.HP10) || Player.Effects.HasFlag(CouponEffects.Defense5) || Player.Effects.HasFlag(CouponEffects.Defense10) || Player.Effects.HasFlag(CouponEffects.Defense20))
            {
                if (Player.Effects.HasFlag(CouponEffects.ExtraGrenade))
                {
                    Flag[2] += 1;
                }
                if (Player.Effects.HasFlag(CouponEffects.ExtraThrowGrenade))
                {
                    Flag[2] += 2;
                }
                if (Player.Effects.HasFlag(CouponEffects.JackHollowPoint))
                {
                    Flag[2] += 4;
                }
                if (Player.Effects.HasFlag(CouponEffects.HP5))
                {
                    Flag[2] += 8;
                }
                if (Player.Effects.HasFlag(CouponEffects.HP10))
                {
                    Flag[2] += 16;
                }
                if (Player.Effects.HasFlag(CouponEffects.Defense5))
                {
                    Flag[2] += 32;
                }
                if (Player.Effects.HasFlag(CouponEffects.Defense10))
                {
                    Flag[2] += 64;
                }
                if (Player.Effects.HasFlag(CouponEffects.Defense20))
                {
                    Flag[2] += 128;
                }
            }
            if (Player.Effects.HasFlag(CouponEffects.Defense90) || Player.Effects.HasFlag(CouponEffects.Respawn20) || Player.Effects.HasFlag(CouponEffects.Respawn30) || Player.Effects.HasFlag(CouponEffects.Respawn50) || Player.Effects.HasFlag(CouponEffects.Respawn100))
            {
                if (Player.Effects.HasFlag(CouponEffects.Defense90))
                {
                    Flag[3] += 1;
                }
                if (Player.Effects.HasFlag(CouponEffects.Respawn20))
                {
                    Flag[3] += 2;
                }
                if (Player.Effects.HasFlag(CouponEffects.Respawn30))
                {
                    Flag[3] += 8;
                }
                if (Player.Effects.HasFlag(CouponEffects.Respawn50))
                {
                    Flag[3] += 32;
                }
                if (Player.Effects.HasFlag(CouponEffects.Respawn100))
                {
                    Flag[3] += 128;
                }
            }
            return Flag;
        }
        public static ushort DataFromItems(List<ItemsModel> Items, out List<ItemsModel> Charas, out List<ItemsModel> Weapons, out List<ItemsModel> Coupons)
        {
            Charas = new List<ItemsModel>();
            Weapons = new List<ItemsModel>();
            Coupons = new List<ItemsModel>();
            foreach (ItemsModel item in Items)
            {
                if (item.Category == ItemCategory.Weapon)
                {
                    Weapons.Add(item);
                }
                else if (item.Category == ItemCategory.Character)
                {
                    Charas.Add(item);
                }
                else if (item.Category == ItemCategory.Coupon)
                {
                    Coupons.Add(item);
                }
            }
            return (ushort)(Charas.Count + Weapons.Count + Coupons.Count);
        }
        public static List<ItemsModel> LimitationIndex(Account Player, List<ItemsModel> Items)
        {
            int MaxInventoryItems = (600 + Player.InventoryPlus);
            if (Items.Count > MaxInventoryItems)
            {
                int LimitationIndex = (MaxInventoryItems / 3);
                if (Items.Count > LimitationIndex)
                {
                    Items.RemoveRange(LimitationIndex, Items.Count - MaxInventoryItems);
                }
            }
            return Items;
        }
        public static uint GetFeatures()
        {
            uint Result = (uint)AccountFeatures.ALL;

            if (!AuthXender.Client.Config.EnableClan)
                Result -= (uint)AccountFeatures.CLAN;
            if (!AuthXender.Client.Config.EnableTicket)
                Result -= (uint)AccountFeatures.TICKET;
            if (!AuthXender.Client.Config.EnableTags)
                Result -= (uint)AccountFeatures.TAGS;

            Result -= (uint)AccountFeatures.PLAYTIME;

            return Result;
        }
    }
}
