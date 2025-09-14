using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.RAW;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Npgsql;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Data;

namespace Server.Game.Data.Utils
{
    public static class AllUtils
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
        public static int GetKillScore(KillingMessage msg)
        {
            int score = 0;
            if (msg == KillingMessage.MassKill || msg == KillingMessage.PiercingShot)
            {
                score += 6;
            }
            else if (msg == KillingMessage.ChainStopper)
            {
                score += 8;
            }
            else if (msg == KillingMessage.Headshot)
            {
                score += 10;
            }
            else if (msg == KillingMessage.ChainHeadshot)
            {
                score += 14;
            }
            else if (msg == KillingMessage.ChainSlugger)
            {
                score += 6;
            }
            else if (msg == KillingMessage.ObjectDefense)
            {
                score += 7;
            }
            else if (msg != KillingMessage.Suicide)
            {
                score += 5;
            }
            return score;
        }
        private static ClassType ConvertWeaponClass(ClassType weaponClass)
        {
            if (weaponClass == ClassType.DualSMG)
            {
                return ClassType.SMG;
            }
            else if (weaponClass == ClassType.DualHandGun)
            {
                return ClassType.HandGun;
            }
            else if (weaponClass == ClassType.DualKnife || weaponClass == ClassType.Knuckle)
            {
                return ClassType.Knife;
            }
            else if (weaponClass == ClassType.DualShotgun)
            {
                return ClassType.Shotgun;
            }
            return weaponClass;
        }
        public static TeamEnum GetWinnerTeam(RoomModel room)
        {
            if (room == null)
            {
                return TeamEnum.TEAM_DRAW;
            }
            TeamEnum value = TeamEnum.TEAM_DRAW;
            if (room.RoomType == RoomCondition.Bomb || room.RoomType == RoomCondition.Destroy || room.RoomType == RoomCondition.Annihilation || room.RoomType == RoomCondition.Defense || room.RoomType == RoomCondition.Convoy)
            {
                if (room.CTRounds == room.FRRounds)
                {
                    value = TeamEnum.TEAM_DRAW;
                }
                else if (room.CTRounds > room.FRRounds)
                {
                    value = TeamEnum.CT_TEAM;
                }
                else if (room.CTRounds < room.FRRounds)
                {
                    value = TeamEnum.FR_TEAM;
                }
            }
            else if (room.IsDinoMode("DE"))
            {
                if (room.CTDino == room.FRDino)
                {
                    value = TeamEnum.TEAM_DRAW;
                }
                else if (room.CTDino > room.FRDino)
                {
                    value = TeamEnum.CT_TEAM;
                }
                else if (room.CTDino < room.FRDino)
                {
                    value = TeamEnum.FR_TEAM;
                }
            }
            else
            {
                if (room.CTKills == room.FRKills)
                {
                    value = TeamEnum.TEAM_DRAW;
                }
                else if (room.CTKills > room.FRKills)
                {
                    value = TeamEnum.CT_TEAM;
                }
                else if (room.CTKills < room.FRKills)
                {
                    value = TeamEnum.FR_TEAM;
                }
            }
            return value;
        }
        public static TeamEnum GetWinnerTeam(RoomModel room, int RedPlayers, int BluePlayers)
        {
            if (room == null)
            {
                return TeamEnum.TEAM_DRAW;
            }
            TeamEnum value = TeamEnum.TEAM_DRAW;
            if (RedPlayers == 0)
            {
                value = TeamEnum.CT_TEAM;
            }
            else if (BluePlayers == 0)
            {
                value = TeamEnum.FR_TEAM;
            }
            return value;
        }
        public static void UpdateMatchCount(bool WonTheMatch, Account Player, int WinnerTeam, DBQuery TotalQuery, DBQuery SeasonQuery)
        {
            if (WinnerTeam == 2)
            {
                TotalQuery.AddQuery("match_draws", ++Player.Statistic.Basic.MatchDraws);
                SeasonQuery.AddQuery("match_draws", ++Player.Statistic.Season.MatchDraws);
            }
            else if (WonTheMatch)
            {
                TotalQuery.AddQuery("match_wins", ++Player.Statistic.Basic.MatchWins);
                SeasonQuery.AddQuery("match_wins", ++Player.Statistic.Season.MatchWins);
            }
            else
            {
                TotalQuery.AddQuery("match_loses", ++Player.Statistic.Basic.MatchLoses);
                SeasonQuery.AddQuery("match_loses", ++Player.Statistic.Season.MatchLoses);
            }
            TotalQuery.AddQuery("matches", ++Player.Statistic.Basic.Matches);
            TotalQuery.AddQuery("total_matches", ++Player.Statistic.Basic.TotalMatchesCount);
            SeasonQuery.AddQuery("matches", ++Player.Statistic.Season.Matches);
            SeasonQuery.AddQuery("total_matches", ++Player.Statistic.Season.TotalMatchesCount);
        }
        public static void UpdateDailyRecord(bool WonTheMatch, Account Player, int winnerTeam, DBQuery query)
        {
            if (winnerTeam == 2)
            {
                query.AddQuery("match_draws", ++Player.Statistic.Daily.MatchDraws);
            }
            else if (WonTheMatch)
            {
                query.AddQuery("match_wins", ++Player.Statistic.Daily.MatchWins);
            }
            else
            {
                query.AddQuery("match_loses", ++Player.Statistic.Daily.MatchLoses);
            }
            query.AddQuery("matches", ++Player.Statistic.Daily.Matches);
        }
        public static void UpdateMatchCountFreeForAll(RoomModel Room, Account Player, int SlotWin, DBQuery TotalQuery, DBQuery SeasonQuery)
        {
            int SlotIndex;
            int[] Array = new int[16];
            for (int i = 0; i < Array.Length; i++)
            {
                SlotModel Slot = Room.Slots[i];
                if (Slot.PlayerId != 0)
                {
                    Array[i] = Slot.AllKills;
                }
                else
                {
                    Array[i] = 0;
                }
            }
            int SlotKills = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] > Array[SlotKills])
                {
                    SlotKills = i;
                }
            }
            SlotIndex = Array[SlotKills];
            if (SlotIndex == SlotWin)
            {
                TotalQuery.AddQuery("match_wins", ++Player.Statistic.Basic.MatchWins);
                SeasonQuery.AddQuery("season_match_wins", ++Player.Statistic.Season.MatchWins);
            }
            else
            {
                TotalQuery.AddQuery("match_loses", ++Player.Statistic.Basic.MatchLoses);
                SeasonQuery.AddQuery("season_match_loses", ++Player.Statistic.Season.MatchLoses);
            }
            TotalQuery.AddQuery("matches", ++Player.Statistic.Basic.Matches);
            TotalQuery.AddQuery("total_matches", ++Player.Statistic.Basic.TotalMatchesCount);
            SeasonQuery.AddQuery("season_matches", ++Player.Statistic.Season.Matches);
            SeasonQuery.AddQuery("season_total_matches", ++Player.Statistic.Season.TotalMatchesCount);
        }
        public static void UpdateMatchDailyRecordFreeForAll(RoomModel Room, Account Player, int SlotWin, DBQuery Query)
        {
            int SlotIndex;
            int[] Array = new int[16];
            for (int i = 0; i < Array.Length; i++)
            {
                SlotModel Slot = Room.Slots[i];
                if (Slot.PlayerId != 0)
                {
                    Array[i] = Slot.AllKills;
                }
                else
                {
                    Array[i] = 0;
                }
            }
            int SlotKills = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] > Array[SlotKills])
                {
                    SlotKills = i;
                }
            }
            SlotIndex = Array[SlotKills];
            if (SlotIndex == SlotWin)
            {
                Query.AddQuery("match_wins", ++Player.Statistic.Daily.MatchWins);
            }
            else
            {
                Query.AddQuery("match_loses", ++Player.Statistic.Daily.MatchLoses);
            }
            Query.AddQuery("matches", ++Player.Statistic.Daily.Matches);
        }
        public static void UpdateWeaponRecord(Account Player, SlotModel Slot, DBQuery Query)
        {
            StatWeapon Weapon = Player.Statistic.Weapon;
            if (Slot.AR[0] > 0)
            {
                Query.AddQuery("assault_rifle_kills", ++Weapon.AssaultKills);
            }
            if (Slot.AR[1] > 0)
            {
                Query.AddQuery("assault_rifle_deaths", ++Weapon.AssaultDeaths);
            }
            if (Slot.SMG[0] > 0)
            {
                Query.AddQuery("sub_machine_gun_kills", ++Weapon.SmgKills);
            }
            if (Slot.SMG[1] > 0)
            {
                Query.AddQuery("sub_machine_gun_deaths", ++Weapon.SmgDeaths);
            }
            if (Slot.SR[0] > 0)
            {
                Query.AddQuery("sniper_rifle_kills", ++Weapon.SniperKills);
            }
            if (Slot.SR[1] > 0)
            {
                Query.AddQuery("sniper_rifle_deaths", ++Weapon.SniperDeaths);
            }
            if (Slot.SG[0] > 0)
            {
                Query.AddQuery("shot_gun_kills", ++Weapon.ShotgunKills);
            }
            if (Slot.SG[1] > 0)
            {
                Query.AddQuery("shot_gun_deaths", ++Weapon.ShotgunDeaths);
            }
            if (Slot.MG[0] > 0)
            {
                Query.AddQuery("machine_gun_kills", ++Weapon.MachinegunKills);
            }
            if (Slot.MG[1] > 0)
            {
                Query.AddQuery("machine_gun_deaths", ++Weapon.MachinegunDeaths);
            }
        }
        public static void GenerateMissionAwards(Account Player, DBQuery query)
        {
            try
            {
                PlayerMissions missions = Player.Mission;
                int activeM = missions.ActualMission, missionId = missions.GetCurrentMissionId(), cardId = missions.GetCurrentCard();
                if (missionId <= 0 || missions.SelectedCard)
                {
                    return;
                }
                int CompletedLastCardCount = 0, allCompletedCount = 0;
                byte[] missionL = missions.GetCurrentMissionList();
                List<MissionCardModel> cards = MissionCardRAW.GetCards(missionId, -1);
                foreach (MissionCardModel card in cards)
                {
                    if (missionL[card.ArrayIdx] >= card.MissionLimit)
                    {
                        allCompletedCount++;
                        if (card.CardBasicId == cardId)
                        {
                            CompletedLastCardCount++;
                        }
                    }
                }
                if (allCompletedCount >= 40)
                {
                    int blueOrder = Player.MasterMedal, brooch = Player.Ribbon, medal = Player.Medal, insignia = Player.Ensign;
                    MissionCardAwards c = MissionCardRAW.GetAward(missionId, cardId);
                    if (c != null)
                    {
                        Player.Ribbon += c.Ribbon;
                        Player.Medal += c.Medal;
                        Player.Ensign += c.Ensign;
                        Player.Gold += c.Gold;
                        Player.Exp += c.Exp;
                    }
                    MissionAwards Mission = MissionAwardXML.GetAward(missionId);
                    if (Mission != null)
                    {
                        Player.MasterMedal += Mission.MasterMedal;
                        Player.Exp += Mission.Exp;
                        Player.Gold += Mission.Gold;
                    }
                    List<ItemsModel> Items = MissionCardRAW.GetMissionAwards(missionId);
                    if (Items.Count > 0)
                    {
                        foreach (ItemsModel Item in Items)
                        {
                            Player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Item));
                        }
                    }
                    Player.SendPacket(new PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_ACK(273, 4, Player));
                    if (Player.Ribbon != brooch)
                    {
                        query.AddQuery("ribbon", Player.Ribbon);
                    }
                    if (Player.Ensign != insignia)
                    {
                        query.AddQuery("ensign", Player.Ensign);
                    }
                    if (Player.Medal != medal)
                    {
                        query.AddQuery("medal", Player.Medal);
                    }
                    if (Player.MasterMedal != blueOrder)
                    {
                        query.AddQuery("master_medal", Player.MasterMedal);
                    }
                    query.AddQuery($"mission_id{(activeM + 1)}", 0);
                    ComDiv.UpdateDB("player_missions", "owner_id", Player.PlayerId, new string[] { $"card{(activeM + 1)}", $"mission{(activeM + 1)}_raw" }, 0, new byte[0]);

                    if (activeM == 0)
                    {
                        missions.Mission1 = 0;
                        missions.Card1 = 0;
                        missions.List1 = new byte[40];
                    }
                    else if (activeM == 1)
                    {
                        missions.Mission2 = 0;
                        missions.Card2 = 0;
                        missions.List2 = new byte[40];
                    }
                    else if (activeM == 2)
                    {
                        missions.Mission3 = 0;
                        missions.Card3 = 0;
                        missions.List3 = new byte[40];
                    }
                    else if (activeM == 3)
                    {
                        missions.Mission4 = 0;
                        missions.Card3 = 0;
                        missions.List4 = new byte[40];
                        if (Player.Event != null)
                        {
                            Player.Event.LastQuestFinish = 1;
                            ComDiv.UpdateDB("player_events", "last_quest_finish", 1, "owner_id", Player.PlayerId);
                        }
                    }
                }
                else if (CompletedLastCardCount == 4 && !missions.SelectedCard)
                {
                    MissionCardAwards c = MissionCardRAW.GetAward(missionId, cardId);
                    if (c != null)
                    {
                        int brooch = Player.Ribbon, medal = Player.Medal, insignia = Player.Ensign;
                        Player.Ribbon += c.Ribbon;
                        Player.Medal += c.Medal;
                        Player.Ensign += c.Ensign;
                        Player.Gold += c.Gold;
                        Player.Exp += c.Exp;
                        if (Player.Ribbon != brooch)
                        {
                            query.AddQuery("ribbon", Player.Ribbon);
                        }
                        if (Player.Ensign != insignia)
                        {
                            query.AddQuery("ensign", Player.Ensign);
                        }
                        if (Player.Medal != medal)
                        {
                            query.AddQuery("medal", Player.Medal);
                        }
                    }
                    missions.SelectedCard = true;
                    Player.SendPacket(new PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_ACK(1, 1, Player));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"AllUtils.GenerateMissionAwards: {ex.Message}", LoggerType.Error, ex);
            }
        }
        public static void ResetSlotInfo(RoomModel room, SlotModel slot, bool updateInfo)
        {
            if (slot.State >= SlotState.LOAD)
            {
                room.ChangeSlotState(slot, SlotState.NORMAL, updateInfo);
                slot.ResetSlot();
            }
        }
        public static void EndMatchMission(RoomModel room, Account player, SlotModel slot, TeamEnum winnerTeam)
        {
            if (winnerTeam != TeamEnum.TEAM_DRAW)
            {
                CompleteMission(room, player, slot, slot.Team == winnerTeam ? MissionType.WIN : MissionType.DEFEAT, 0);
            }
        }
        public static void VotekickResult(RoomModel room)
        {
            VoteKickModel votekick = room.votekick;
            if (votekick != null)
            {
                int Count = votekick.GetInGamePlayers();
                if (votekick.Accept > votekick.Denie && votekick.Enemies > 0 && votekick.Allies > 0 && votekick.Votes.Count >= Count / 2)
                {
                    Account Member = room.GetPlayerBySlot(votekick.VictimIdx);
                    if (Member != null)
                    {
                        Member.SendPacket(new PROTOCOL_BATTLE_NOTIFY_BE_KICKED_BY_KICKVOTE_ACK());
                        room.KickedPlayers.Add(Member.PlayerId);
                        room.RemovePlayer(Member, true, 2);
                    }
                }
                uint erro = 0;
                if (votekick.Allies == 0)
                {
                    erro = 2147488001;
                }
                else if (votekick.Enemies == 0)
                {
                    erro = 2147488002;
                }
                else if (votekick.Denie < votekick.Accept || votekick.Votes.Count < Count / 2)
                {
                    erro = 2147488000;
                }
                using (PROTOCOL_BATTLE_NOTIFY_KICKVOTE_RESULT_ACK packet = new PROTOCOL_BATTLE_NOTIFY_KICKVOTE_RESULT_ACK(erro, votekick))
                {
                    room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                }
                //LogVotekickResult(room);
            }
            room.votekick = null;
        }
        public static void ResetBattleInfo(RoomModel Room)
        {
            //LogRoomResult(room);
            foreach (SlotModel slot in Room.Slots)
            {
                if (slot.PlayerId > 0 && slot.State >= SlotState.LOAD)
                {
                    slot.State = SlotState.NORMAL;
                    slot.ResetSlot();
                }
            }
            Room.PreMatchCD = false;
            Room.BlockedClan = false;
            Room.Rounds = 1;
            Room.SpawnsCount = 0;
            Room.FRKills = 0;
            Room.FRAssists = 0;
            Room.FRDeaths = 0;
            Room.CTKills = 0;
            Room.CTAssists = 0;
            Room.CTDeaths = 0;
            Room.FRDino = 0;
            Room.CTDino = 0;
            Room.FRRounds = 0;
            Room.CTRounds = 0;
            Room.BattleStart = new DateTime();
            Room.TimeRoom = 0;
            Room.Bar1 = 0;
            Room.Bar2 = 0;
            Room.SwapRound = false;
            Room.IngameAiLevel = 0;
            Room.State = RoomState.Ready;
            Room.UpdateRoomInfo();
            Room.votekick = null;
            Room.UdpServer = null;
            if (Room.RoundTime.Timer != null)
            {
                Room.RoundTime.Timer = null;
            }
            if (Room.VoteTime.Timer != null)
            {
                Room.VoteTime.Timer = null;
            }
            if (Room.BombTime.Timer != null)
            {
                Room.BombTime.Timer = null;
            }
            Room.UpdateSlotsInfo();
        }
        public static List<int> GetDinossaurs(RoomModel room, bool forceNewTRex, int forceRexIdx)
        {
            List<int> dinos = new List<int>();
            if (room.IsDinoMode())
            {
                TeamEnum teamIdx = room.Rounds == 1 ? TeamEnum.FR_TEAM : TeamEnum.CT_TEAM;
                int[] array = room.GetTeamArray(teamIdx);
                for (int i = 0; i < array.Length; i++)
                {
                    int slotIdx = array[i];
                    SlotModel slot = room.Slots[slotIdx];
                    if (slot.State == SlotState.BATTLE && !slot.SpecGM)
                    {
                        dinos.Add(slotIdx);
                    }
                }
                if ((room.TRex == -1 || (int)room.Slots[room.TRex].State <= 14 || forceNewTRex) && dinos.Count > 1 && room.IsDinoMode("DE"))
                {
                    //Logger.warning("Trex: " + room.TRex);
                    if (forceRexIdx >= 0 && dinos.Contains(forceRexIdx))
                    {
                        room.TRex = forceRexIdx;
                    }
                    else if (forceRexIdx == -2)
                    {
                        room.TRex = dinos[new Random().Next(0, dinos.Count)];
                    }
                    //Logger.warning("ForceRexIdx: " + forceRexIdx + " Force: " + forceNewTRex + " TeamIdx: " + teamIdx + " Trex: " + room.TRex);
                }
            }
            return dinos;
        }
        public static void BattleEndPlayersCount(RoomModel room, bool isBotMode)
        {
            if (room == null || isBotMode || !room.IsPreparing())
            {
                return;
            }
            int blue = 0, red = 0, blue2 = 0, red2 = 0;
            foreach (SlotModel slot in room.Slots)
            {
                if (slot.State == SlotState.BATTLE)
                {
                    if (slot.Team == 0)
                    {
                        red++;
                    }
                    else
                    {
                        blue++;
                    }
                }
                else if (slot.State >= SlotState.LOAD)
                {
                    if (slot.Team == 0)
                    {
                        red2++;
                    }
                    else
                    {
                        blue2++;
                    }
                }
            }
            if ((red == 0 || blue == 0) && room.State == RoomState.Battle || (red2 == 0 || blue2 == 0) && room.State <= RoomState.PreBattle)
            {
                EndBattle(room, isBotMode);
            }
        }
        public static void EndBattle(RoomModel room)
        {
            EndBattle(room, room.IsBotMode());
        }
        public static void EndBattle(RoomModel room, bool isBotMode)
        {
            TeamEnum winnerTeam = GetWinnerTeam(room);
            EndBattle(room, isBotMode, winnerTeam);
        }
        public static void EndBattleNoPoints(RoomModel room)
        {
            List<Account> players = room.GetAllPlayers(SlotState.READY, 1);
            if (players.Count > 0)
            {
                GetBattleResult(room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                bool isBotMode = room.IsBotMode();
                foreach (Account pR in players)
                {
                    pR.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(pR, TeamEnum.TEAM_DRAW, inBattle, missionCompletes, isBotMode, Data));
                }
            }
            ResetBattleInfo(room);
        }
        public static void EndBattle(RoomModel room, bool isBotMode, TeamEnum winnerTeam)
        {
            List<Account> players = room.GetAllPlayers(SlotState.READY, 1);
            if (players.Count > 0)
            {
                room.CalculateResult(winnerTeam, isBotMode);
                GetBattleResult(room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                foreach (Account pR in players)
                {
                    pR.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(pR, winnerTeam, inBattle, missionCompletes, isBotMode, Data));
                }
            }
            ResetBattleInfo(room);
        }
        public static int Percentage(int total, int percent)
        {
            return total * percent / 100;
        }
        public static void BattleEndRound(RoomModel room, TeamEnum winner, bool forceRestart)
        {
            int roundsByMask = room.GetRoundsByMask();
            if (room.FRRounds >= roundsByMask || room.CTRounds >= roundsByMask)
            {
                room.StopBomb();
                using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(room, winner, RoundEndType.AllDeath))
                {
                    room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                }
                EndBattle(room, room.IsBotMode(), winner);
            }
            else if (!room.ActiveC4 || forceRestart)
            {
                room.StopBomb();
                room.Rounds++;
                RoundSync.SendUDPRoundSync(room);
                using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(room, winner, RoundEndType.AllDeath))
                {
                    room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                }
                room.RoundRestart();
            }
        }
        public static void BattleEndRound(RoomModel room, TeamEnum winner, RoundEndType motive)
        {
            using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(room, winner, motive))
            {
                room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
            }
            room.StopBomb();
            int roundsByMask = room.GetRoundsByMask();
            if (room.FRRounds >= roundsByMask || room.CTRounds >= roundsByMask)
            {
                EndBattle(room, room.IsBotMode(), winner);
            }
            else
            {
                room.Rounds++;
                RoundSync.SendUDPRoundSync(room);
                room.RoundRestart();
            }
        }
        public static int AddFriend(Account Owner, Account Friend, int state)
        {
            if (Owner == null || Friend == null)
            {
                return -1;
            }
            try
            {
                FriendModel OwnerFriend = Owner.Friend.GetFriend(Friend.PlayerId);
                if (OwnerFriend == null)
                {
                    using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                    {
                        NpgsqlCommand command = connection.CreateCommand();
                        connection.Open();
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@friend", Friend.PlayerId);
                        command.Parameters.AddWithValue("@owner", Owner.PlayerId);
                        command.Parameters.AddWithValue("@state", state);
                        command.CommandText = "INSERT INTO player_friends (id, owner_id, state) VALUES (@friend, @owner, @state)";
                        command.ExecuteNonQuery();
                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                    }
                    lock (Owner.Friend.Friends)
                    {
                        FriendModel FriendOwner = new FriendModel(Friend.PlayerId, Friend.Rank, Friend.NickColor, Friend.Nickname, Friend.IsOnline, Friend.Status)
                        {
                            State = state,
                            Removed = false
                        };
                        Owner.Friend.Friends.Add(FriendOwner);
                        SendFriendInfo.Load(Owner, FriendOwner, 0);
                    }
                    return 0;
                }
                else
                {
                    if (OwnerFriend.Removed)
                    {
                        OwnerFriend.Removed = false;
                        DaoManagerSQL.UpdatePlayerFriendBlock(Owner.PlayerId, OwnerFriend);
                        SendFriendInfo.Load(Owner, OwnerFriend, 1);
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"AllUtils.AddFriend: {ex.Message}", LoggerType.Error, ex);
                return -1;
            }
        }
        public static void SyncPlayerToFriends(Account p, bool all)
        {
            if (p == null || p.Friend.Friends.Count == 0)
            {
                return;
            }
            PlayerInfo info = new PlayerInfo(p.PlayerId, p.Rank, p.NickColor, p.Nickname, p.IsOnline, p.Status);
            for (int i = 0; i < p.Friend.Friends.Count; i++)
            {
                FriendModel friend = p.Friend.Friends[i];
                if (all || friend.State == 0 && !friend.Removed)
                {
                    Account f1 = AccountManager.GetAccount(friend.PlayerId, 287);
                    if (f1 != null)
                    {
                        int idx = -1;
                        FriendModel f2 = f1.Friend.GetFriend(p.PlayerId, out idx);
                        if (f2 != null)
                        {
                            f2.Info = info;
                            f1.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Update, f2, idx), false);
                        }
                    }
                }
            }
        }
        public static void SyncPlayerToClanMembers(Account player)
        {
            if (player == null || player.ClanId <= 0)
            {
                return;
            }
            using (PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK packet = new PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(player))
            {
                ClanManager.SendPacket(packet, player.ClanId, player.PlayerId, true, true);
            }
        }
        public static void UpdateSlotEquips(Account Player)
        {
            RoomModel room = Player.Room;
            if (room != null)
            {
                UpdateSlotEquips(Player, room);
            }
        }
        public static void UpdateSlotEquips(Account p, RoomModel room)
        {
            if (room.GetSlot(p.SlotId, out SlotModel slot))
            {
                slot.Equipment = p.Equipment;
            }
            room.UpdateSlotsInfo();
        }
        public static ushort GetSlotsFlag(RoomModel Room, bool OnlyNoSpectators, bool MissionSuccess)
        {
            if (Room == null)
            {
                return 0;
            }
            int Flags = 0;
            foreach (SlotModel Slot in Room.Slots)
            {
                if (Slot.State >= SlotState.LOAD && (MissionSuccess && Slot.MissionsCompleted || !MissionSuccess && (!OnlyNoSpectators || !Slot.Spectator)))
                {
                    Flags += Slot.Flag;
                }
            }
            return (ushort)Flags;
        }
        public static void GetBattleResult(RoomModel Room, out ushort MissionFlag, out ushort SlotFlag, out byte[] Data)
        {
            MissionFlag = 0;
            SlotFlag = 0;
            Data = new byte[272];
            if (Room == null)
            {
                return;
            }
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = 0; i < 16; i++)
                {
                    SlotModel Slot = Room.Slots[i];
                    if (Slot.State >= SlotState.LOAD)
                    {
                        ushort Flag = (ushort)Slot.Flag;
                        if (Slot.MissionsCompleted)
                        {
                            MissionFlag += Flag;
                        }
                        SlotFlag += Flag;
                    }
                }
                foreach (SlotModel Slot in Room.Slots)
                {
                    S.WriteH((ushort)Slot.Exp);
                }
                foreach (SlotModel Slot in Room.Slots)
                {
                    S.WriteH((ushort)Slot.Gold);
                }
                foreach (SlotModel Slot in Room.Slots)
                {
                    S.WriteC((byte)Slot.BonusFlags);
                }
                foreach (SlotModel Slot in Room.Slots)
                {
                    S.WriteH((ushort)Slot.BonusCafeExp);
                    S.WriteH((ushort)Slot.BonusItemExp);
                    S.WriteH((ushort)Slot.BonusEventExp);
                }
                foreach (SlotModel Slot in Room.Slots)
                {
                    S.WriteH((ushort)Slot.BonusCafePoint);
                    S.WriteH((ushort)Slot.BonusItemPoint);
                    S.WriteH((ushort)Slot.BonusEventPoint);
                }
                Data = S.ToArray();
            }
        }
        public static bool DiscountPlayerItems(SlotModel Slot, Account Player)
        {
            try
            {
                bool LoadCode = false, UpdateCouponEffect = false;
                uint CurrentDate = Convert.ToUInt32(DateTimeUtil.Now("yyMMddHHmm"));
                List<ItemsModel> UpdateList = new List<ItemsModel>();
                List<object> RemovedItems = new List<object>();
                List<object> RemovedCharas = new List<object>();
                int Bonuses = Player.Bonus != null ? Player.Bonus.Bonuses : 0, FreePass = Player.Bonus != null ? Player.Bonus.FreePass : 0;
                lock (Player.Inventory.Items)
                {
                    for (int i = 0; i < Player.Inventory.Items.Count; i++)
                    {
                        ItemsModel Item = Player.Inventory.Items[i];
                        if (Item.Equip == ItemEquipType.Durable && Slot.ItemUsages.Contains(Item.Id) && !Slot.SpecGM)
                        {
                            if (Item.Count <= CurrentDate && (Item.Id == 800216 || Item.Id == 2700013 || Item.Id == 800169))
                            {
                                DaoManagerSQL.DeletePlayerInventoryItem(Item.ObjectId, Player.PlayerId);
                            }
                            if (--Item.Count < 1)
                            {
                                RemovedItems.Add(Item.ObjectId);
                                Player.Inventory.Items.RemoveAt(i--);
                            }
                            else
                            {
                                UpdateList.Add(Item);
                                ComDiv.UpdateDB("player_items", "count", (long)Item.Count, "object_id", Item.ObjectId, "owner_id", Player.PlayerId);
                            }
                        }
                        else if (Item.Count <= CurrentDate && Item.Equip == ItemEquipType.Temporary)
                        {
                            if (Item.Category == ItemCategory.Coupon)
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
                                        LoadCode = true;
                                    }
                                    else if (Item.Id == 1600006)
                                    {
                                        ComDiv.UpdateDB("accounts", "nick_color", 0, "player_id", Player.PlayerId);
                                        Player.NickColor = 0;
                                        if (Player.Room != null)
                                        {
                                            using (PROTOCOL_ROOM_GET_COLOR_NICK_ACK packet = new PROTOCOL_ROOM_GET_COLOR_NICK_ACK(Player.SlotId, Player.NickColor))
                                            {
                                                Player.Room.SendPacketToPlayers(packet);
                                            }
                                            Player.Room.UpdateSlotsInfo();
                                        }
                                        LoadCode = true;
                                    }
                                    else if (Item.Id == 1600009)
                                    {
                                        ComDiv.UpdateDB("player_bonus", "fake_rank", 55, "owner_id", Player.PlayerId);
                                        Player.Bonus.FakeRank = 55;
                                        if (Player.Room != null)
                                        {
                                            using (PROTOCOL_ROOM_GET_RANK_ACK packet = new PROTOCOL_ROOM_GET_RANK_ACK(Player.SlotId, Player.Rank))
                                            {
                                                Player.Room.SendPacketToPlayers(packet);
                                            }
                                            Player.Room.UpdateSlotsInfo();
                                        }
                                        LoadCode = true;
                                    }
                                    else if (Item.Id == 1600010)
                                    {
                                        ComDiv.UpdateDB("player_bonus", "fake_nick", "", "owner_id", Player.PlayerId);
                                        ComDiv.UpdateDB("accounts", "nickname", Player.Bonus.FakeNick, "player_id", Player.PlayerId);
                                        Player.Nickname = Player.Bonus.FakeNick;
                                        Player.Bonus.FakeNick = "";
                                        if (Player.Room != null)
                                        {
                                            using (PROTOCOL_ROOM_GET_NICKNAME_ACK packet = new PROTOCOL_ROOM_GET_NICKNAME_ACK(Player.SlotId, Player.Nickname))
                                            {
                                                Player.Room.SendPacketToPlayers(packet);
                                            }
                                            Player.Room.UpdateSlotsInfo();
                                        }
                                        LoadCode = true;
                                    }
                                    else if (Item.Id == 1600187)
                                    {
                                        ComDiv.UpdateDB("player_bonus", "muzzle_color", 0, "owner_id", Player.PlayerId);
                                        Player.Bonus.MuzzleColor = 0;
                                        if (Player.Room != null)
                                        {
                                            using (PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK packet = new PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK(Player.SlotId, Player.Bonus.MuzzleColor))
                                            {
                                                Player.Room.SendPacketToPlayers(packet);
                                            }
                                        }
                                        LoadCode = true;
                                    }
                                }
                                CouponFlag Coupon = CouponEffectXML.GetCouponEffect(Item.Id);
                                if (Coupon != null && Coupon.EffectFlag > 0 && Player.Effects.HasFlag(Coupon.EffectFlag))
                                {
                                    Player.Effects -= Coupon.EffectFlag;
                                    UpdateCouponEffect = true;
                                }
                            }
                            else if (Item.Category == ItemCategory.Character && ComDiv.GetIdStatics(Item.Id, 1) == 6)
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
                                    Player.SendPacket(new PROTOCOL_CHAR_DELETE_CHARA_ACK(0, Character.Slot, Player, Item));
                                    if (DaoManagerSQL.DeletePlayerCharacter(Character.ObjectId, Player.PlayerId))
                                    {
                                        Player.Character.RemoveCharacter(Character);
                                    }
                                }
                            }
                            if (ComDiv.GetIdStatics(Item.Id, 1) == 6)
                            {
                                RemovedCharas.Add(Item.ObjectId);
                            }
                            else
                            {
                                RemovedItems.Add(Item.ObjectId);
                            }
                            Player.Inventory.Items.RemoveAt(i--);
                        }
                        else if (Item.Count == 0)
                        {
                            if (ComDiv.GetIdStatics(Item.Id, 1) == 6)
                            {
                                RemovedCharas.Add(Item.ObjectId);
                            }
                            else
                            {
                                RemovedItems.Add(Item.ObjectId);
                            }
                            Player.Inventory.Items.RemoveAt(i--);
                        }
                    }
                    ComDiv.DeleteDB("player_items", "object_id", RemovedItems.ToArray(), "owner_id", Player.PlayerId);
                    ComDiv.DeleteDB("player_items", "object_id", RemovedCharas.ToArray(), "owner_id", Player.PlayerId);
                }
                for (int i = 0; i < RemovedItems.Count; i++)
                {
                    Player.SendPacket(new PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK(1, (long)RemovedItems[i]));
                }
                RemovedItems = null;
                RemovedCharas = null;
                if (Player.Bonus != null && (Player.Bonus.Bonuses != Bonuses || Player.Bonus.FreePass != FreePass))
                {
                    DaoManagerSQL.UpdatePlayerBonus(Player.PlayerId, Player.Bonus.Bonuses, Player.Bonus.FreePass);
                }
                if (Player.Effects < 0)
                {
                    Player.Effects = 0;
                }
                if (UpdateList.Count > 0)
                {
                    foreach (ItemsModel Item in UpdateList)
                    {
                        Player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(2, Player, Item));
                    }
                }
                UpdateList = null;
                if (UpdateCouponEffect)
                {
                    ComDiv.UpdateDB("accounts", "coupon_effect", (long)Player.Effects, "player_id", Player.PlayerId);
                }
                if (LoadCode)
                {
                    Player.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, Player));
                }
                int Type = ComDiv.CheckEquipedItems(Player.Equipment, Player.Inventory.Items, false);
                if (Type > 0)
                {
                    DBQuery Query = new DBQuery();
                    if ((Type & 2) == 2)
                    {
                        ComDiv.UpdateWeapons(Player.Equipment, Query);
                    }
                    if ((Type & 1) == 1)
                    {
                        ComDiv.UpdateChars(Player.Equipment, Query);
                    }
                    if ((Type & 3) == 3)
                    {
                        ComDiv.UpdateItems(Player.Equipment, Query);
                    }
                    ComDiv.UpdateDB("player_equipments", "owner_id", Player.PlayerId, Query.GetTables(), Query.GetValues());
                    Player.SendPacket(new PROTOCOL_SERVER_MESSAGE_CHANGE_INVENTORY_ACK(Player));
                    Slot.Equipment = Player.Equipment;
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
        public static void TryBalancePlayer(RoomModel Room, Account Player, bool InBattle, ref SlotModel MySlot)
        {
            SlotModel OldSlot = Room.GetSlot(Player.SlotId);
            if (OldSlot == null)
            {
                return;
            }
            TeamEnum PlayerTeamIdx = OldSlot.Team;
            TeamEnum TeamIdx = GetBalanceTeamIdx(Room, InBattle, PlayerTeamIdx);
            if (PlayerTeamIdx == TeamIdx || TeamIdx == TeamEnum.ALL_TEAM)
            {
                return;
            }
            SlotModel NewSlot = null;
            int[] TeamArray = PlayerTeamIdx == TeamEnum.CT_TEAM ? Room.FR_TEAM : Room.CT_TEAM;
            for (int i = 0; i < TeamArray.Length; i++)
            {
                SlotModel Slot = Room.Slots[TeamArray[i]];
                if (Slot.State != SlotState.CLOSE && Slot.PlayerId == 0)
                {
                    NewSlot = Slot;
                    break;
                }
            }
            if (NewSlot == null)
            {
                return;
            }
            List<SlotModel> ChangeList = new List<SlotModel>();
            lock (Room.Slots)
            {
                Room.SwitchSlots(ChangeList, NewSlot.Id, OldSlot.Id, false);
            }
            if (ChangeList.Count > 0)
            {
                //Logger.LogProblems("[AllUtils.TryBalancePlayer] Player: '" + player.player_id + "' '" + player.player_name + "'; OldSlot: " + player._slotId + "; NewSlot: " + newSlot._id, "ErrorC");
                Player.SlotId = OldSlot.Id;
                MySlot = OldSlot;
                using (PROTOCOL_ROOM_TEAM_BALANCE_ACK Packet = new PROTOCOL_ROOM_TEAM_BALANCE_ACK(ChangeList, Room.Leader, 1))
                {
                    Room.SendPacketToPlayers(Packet);
                }
                Room.UpdateSlotsInfo();
            }
        }
        public static TeamEnum GetBalanceTeamIdx(RoomModel room, bool inBattle, TeamEnum PlayerTeamIdx)
        {
            int redPlayers = inBattle && PlayerTeamIdx == 0 ? 1 : 0, bluePlayers = inBattle && PlayerTeamIdx == TeamEnum.CT_TEAM ? 1 : 0;
            foreach (SlotModel slot in room.Slots)
            {
                if (slot.State == SlotState.NORMAL && !inBattle || slot.State >= SlotState.LOAD && inBattle)
                {
                    if (slot.Team == 0)
                    {
                        redPlayers++;
                    }
                    else
                    {
                        bluePlayers++;
                    }
                }
            }
            return redPlayers + 1 < bluePlayers ? TeamEnum.FR_TEAM : bluePlayers + 1 < redPlayers + 1 ? TeamEnum.CT_TEAM : TeamEnum.ALL_TEAM;
        }
        public static int GetNewSlotId(int slotIdx)
        {
            return slotIdx % 2 == 0 ? (slotIdx + 1) : (slotIdx - 1);
        }
        public static void GetXmasReward(Account Player)
        {
            EventXmasModel EvXmas = EventXmasSync.GetRunningEvent();
            if (EvXmas == null)
            {
                return;
            }
            PlayerEvent Event = Player.Event;
            uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
            if (Event != null && !(Event.LastXmasRewardDate > EvXmas.StartDate && Event.LastXmasRewardDate <= EvXmas.EndDate) && ComDiv.UpdateDB("player_events", "last_xmas_reward_date", (long)Date, "owner_id", Player.PlayerId))
            {
                Event.LastXmasRewardDate = Date;
                GoodsItem Good = ShopManager.GetGood(EvXmas.GoodId);
                if (Good != null)
                {
                    Player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(Good.Item)));
                }
            }
        }
        public static void BattleEndRoundPlayersCount(RoomModel room)
        {
            if (room.RoundTime.Timer == null && (room.RoomType == RoomCondition.Bomb || room.RoomType == RoomCondition.Annihilation || room.RoomType == RoomCondition.Convoy || room.RoomType == RoomCondition.Ace))
            {
                room.GetPlayingPlayers(true, out int redPlayers, out int bluePlayers, out int redDeaths, out int blueDeaths);
                if (redDeaths == redPlayers)
                {
                    if (!room.ActiveC4)
                    {
                        room.CTRounds++;
                    }
                    BattleEndRound(room, TeamEnum.CT_TEAM, false);
                }
                else if (blueDeaths == bluePlayers)
                {
                    room.FRRounds++;
                    BattleEndRound(room, TeamEnum.FR_TEAM, true);
                }
            }
        }
        public static void BattleEndKills(RoomModel room)
        {
            BaseEndByKills(room, room.IsBotMode());
        }
        public static void BattleEndKills(RoomModel room, bool isBotMode)
        {
            BaseEndByKills(room, isBotMode);
        }
        private static void BaseEndByKills(RoomModel Room, bool IsBotMode)
        {
            int KillsByMask = Room.GetKillsByMask();
            if (Room.FRKills < KillsByMask && Room.CTKills < KillsByMask)
            {
                return;
            }
            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
            if (Players.Count > 0)
            {
                TeamEnum winner = GetWinnerTeam(Room);
                Room.CalculateResult(winner, IsBotMode);
                GetBattleResult(Room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK Packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Room, winner, RoundEndType.TimeOut))
                {
                    byte[] Buffer = Packet.GetCompleteBytes("AllUtils.BaseEndByKills");
                    foreach (Account Member in Players)
                    {
                        SlotModel Slot = Room.GetSlot(Member.SlotId);
                        if (Slot != null)
                        {
                            if (Slot.State == SlotState.BATTLE)
                            {
                                Member.SendCompletePacket(Buffer, Packet.GetType().Name);
                            }
                            Member.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Member, winner, inBattle, missionCompletes, IsBotMode, Data));
                        }
                    }
                }
            }
            ResetBattleInfo(Room);
        }
        public static void BattleEndKillsFreeForAll(RoomModel room)
        {
            BaseEndByKillsFreeForAll(room);
        }
        private static void BaseEndByKillsFreeForAll(RoomModel Room)
        {
            int killsByMask = Room.GetKillsByMask();
            int[] Array = new int[16];
            for (int i = 0; i < Array.Length; i++)
            {
                SlotModel Slot = Room.Slots[i];
                if (Slot.PlayerId != 0)
                {
                    Array[i] = Slot.AllKills;
                }
                else
                {
                    Array[i] = 0;
                }
            }
            int SlotKills = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] > Array[SlotKills])
                {
                    SlotKills = i;
                }
            }
            if (Array[SlotKills] < killsByMask)
            {
                return;
            }
            List<Account> players = Room.GetAllPlayers(SlotState.READY, 1);
            if (players.Count > 0)
            {
                Room.CalculateResultFreeForAll(SlotKills);
                GetBattleResult(Room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK Packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Room, SlotKills, RoundEndType.FreeForAll))
                {
                    byte[] Buffer = Packet.GetCompleteBytes("AllUtils.BaseEndByKills");
                    foreach (Account Member in players)
                    {
                        SlotModel Slot = Room.GetSlot(Member.SlotId);
                        if (Slot != null)
                        {
                            if (Slot.State == SlotState.BATTLE)
                            {
                                Member.SendCompletePacket(Buffer, Packet.GetType().Name);
                            }
                            Member.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Member, SlotKills, inBattle, missionCompletes, false, Data));
                        }
                    }
                }
            }
            ResetBattleInfo(Room);
        }
        public static bool CheckClanMatchRestrict(RoomModel room)
        {
            if (room.ChannelType == ChannelType.Clan)
            {
                SortedList<int, ClanTeam> clans = GetClanListMatchPlayers(room);
                foreach (ClanTeam Match in clans.Values)
                {
                    if (Match.RedPlayers >= 1 && Match.BluePlayers >= 1)
                    {
                        room.BlockedClan = true;
                        //Logger.warning("XP canceled in clan [Room: " + room._roomId + "; Channel: " + room._channelId + "; ClanId: " + cm.clanId + "]");
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool Have2ClansToClanMatch(RoomModel room)
        {
            SortedList<int, ClanTeam> clans = GetClanListMatchPlayers(room);
            return (clans.Count == 2);
        }
        public static bool HavePlayersToClanMatch(RoomModel room)
        {
            SortedList<int, ClanTeam> clans = GetClanListMatchPlayers(room);
            bool teamRed = false, teamBlue = false;
            foreach (ClanTeam clan in clans.Values)
            {
                if (clan.RedPlayers >= 4)
                {
                    teamRed = true;
                }
                else if (clan.BluePlayers >= 4)
                {
                    teamBlue = true;
                }
            }
            return (teamRed && teamBlue);
        }
        private static SortedList<int, ClanTeam> GetClanListMatchPlayers(RoomModel room)
        {
            SortedList<int, ClanTeam> clans = new SortedList<int, ClanTeam>();
            for (int i = 0; i < room.GetAllPlayers().Count; i++)
            {
                Account pR = room.GetAllPlayers()[i];
                if (pR.ClanId == 0)
                {
                    continue;
                }
                if (clans.TryGetValue(pR.ClanId, out ClanTeam model) && model != null)
                {
                    if (pR.SlotId % 2 == 0)
                    {
                        model.RedPlayers++;
                    }
                    else
                    {
                        model.BluePlayers++;
                    }
                }
                else
                {
                    model = new ClanTeam() { ClanId = pR.ClanId };
                    if (pR.SlotId % 2 == 0)
                    {
                        model.RedPlayers++;
                    }
                    else
                    {
                        model.BluePlayers++;
                    }
                    clans.Add(pR.ClanId, model);
                }
            }
            return clans;
        }
        public static void PlayTimeEvent(long PlayedTime, Account Player, EventPlaytimeModel EvPlaytime, bool IsBotMode)
        {
            RoomModel Room = Player.Room;
            PlayerEvent Event = Player.Event;
            if (Room == null || IsBotMode || Event == null)
            {
                return;
            }
            long LastPlaytimeValue = Event.LastPlaytimeValue, LastPlaytimeFinish = Event.LastPlaytimeFinish, LastPlaytimeDate = Event.LastPlaytimeDate;
            if (Event.LastPlaytimeDate < EvPlaytime.StartDate)
            {
                Event.LastPlaytimeFinish = 0;
                Event.LastPlaytimeValue = 0;
            }
            if (Event.LastPlaytimeFinish == 0)
            {
                Event.LastPlaytimeValue += PlayedTime;
                if (Event.LastPlaytimeValue >= EvPlaytime.Time)
                {
                    Event.LastPlaytimeFinish = 1;
                }
                Event.LastPlaytimeDate = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
                if (Event.LastPlaytimeValue >= EvPlaytime.Time)
                {
                    Player.SendPacket(new PROTOCOL_BATTLE_PLAYER_TIME_ACK(0, EvPlaytime));
                }
                else
                {
                    Player.SendPacket(new PROTOCOL_BATTLE_PLAYER_TIME_ACK(1, new EventPlaytimeModel() { Time = EvPlaytime.Time - Event.LastPlaytimeValue }));
                }
            }
            else if (Event.LastPlaytimeFinish == 1)
            {
                Player.SendPacket(new PROTOCOL_BATTLE_PLAYER_TIME_ACK(0, EvPlaytime));
            }
            if (Event.LastPlaytimeValue != LastPlaytimeValue || Event.LastPlaytimeFinish != LastPlaytimeFinish || Event.LastPlaytimeDate != LastPlaytimeDate)
            {
                EventPlaytimeSync.ResetPlayerEvent(Player.PlayerId, Event);
            }
        }
        public static void CompleteMission(RoomModel room, SlotModel slot, FragInfos kills, MissionType autoComplete, int moreInfo)
        {
            try
            {
                Account player = room.GetPlayerBySlot(slot);
                if (player == null)
                {
                    return;
                }
                MissionCompleteBase(room, player, slot, kills, autoComplete, moreInfo);
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.CompleteMission1] {ex.Message}", LoggerType.Error, ex);
            }
        }
        public static void CompleteMission(RoomModel room, SlotModel slot, MissionType autoComplete, int moreInfo)
        {
            try
            {
                Account player = room.GetPlayerBySlot(slot);
                if (player == null)
                {
                    return;
                }
                MissionCompleteBase(room, player, slot, autoComplete, moreInfo);
            }
            catch (Exception ex)
            {
                CLogger.Print($"[AllUtils.CompleteMission2] {ex.Message}", LoggerType.Error, ex);
            }
        }
        public static void CompleteMission(RoomModel room, Account player, SlotModel slot, FragInfos kills, MissionType autoComplete, int moreInfo)
        {
            MissionCompleteBase(room, player, slot, kills, autoComplete, moreInfo);
        }
        public static void CompleteMission(RoomModel room, Account player, SlotModel slot, MissionType autoComplete, int moreInfo)
        {
            MissionCompleteBase(room, player, slot, autoComplete, moreInfo);
        }
        private static void MissionCompleteBase(RoomModel room, Account pR, SlotModel slot, FragInfos kills, MissionType autoComplete, int moreInfo)
        {
            try
            {
                PlayerMissions missions = slot.Missions;
                if (missions == null)
                {
                    //Logger.error("Missions[1] null! by PlayerId: " + slot._playerId);
                    return;
                }
                int cmId = missions.GetCurrentMissionId(), cardId = missions.GetCurrentCard();
                if (cmId <= 0 || missions.SelectedCard)
                {
                    return;
                }
                List<MissionCardModel> cards = MissionCardRAW.GetCards(cmId, cardId);
                if (cards.Count == 0)
                {
                    return;
                }
                KillingMessage km = kills.GetAllKillFlags();
                byte[] missionArray = missions.GetCurrentMissionList();

                ClassType weaponClass = ComDiv.GetIdClassType(kills.Weapon);
                ClassType convertedClass = ConvertWeaponClass(weaponClass);
                int weaponId = ComDiv.GetIdStatics(kills.Weapon, 3);
                ClassType moreClass = moreInfo > 0 ? ComDiv.GetIdClassType(moreInfo) : 0;
                ClassType moreConvClass = moreInfo > 0 ? ConvertWeaponClass(moreClass) : 0;
                int moreId = moreInfo > 0 ? ComDiv.GetIdStatics(moreInfo, 3) : 0;

                foreach (MissionCardModel card in cards)
                {
                    int count = 0;
                    if (card.MapId == 0 || card.MapId == (int)room.MapId)
                    {
                        if (kills.Frags.Count > 0)
                        {
                            if (card.MissionType == MissionType.KILL || card.MissionType == MissionType.CHAINSTOPPER && km.HasFlag(KillingMessage.ChainStopper) || card.MissionType == MissionType.CHAINSLUGGER && km.HasFlag(KillingMessage.ChainSlugger) || card.MissionType == MissionType.CHAINKILLER && slot.KillsOnLife >= 4 || card.MissionType == MissionType.TRIPLE_KILL && slot.KillsOnLife == 3 || card.MissionType == MissionType.DOUBLE_KILL && slot.KillsOnLife == 2 || card.MissionType == MissionType.HEADSHOT && (km.HasFlag(KillingMessage.Headshot) || km.HasFlag(KillingMessage.ChainHeadshot)) || card.MissionType == MissionType.CHAINHEADSHOT && km.HasFlag(KillingMessage.ChainHeadshot) || card.MissionType == MissionType.PIERCING && km.HasFlag(KillingMessage.PiercingShot) || card.MissionType == MissionType.MASS_KILL && km.HasFlag(KillingMessage.MassKill) || card.MissionType == MissionType.KILL_MAN && room.IsDinoMode() && (slot.Team == TeamEnum.CT_TEAM && room.Rounds == 2 || slot.Team == TeamEnum.FR_TEAM && room.Rounds == 1))
                            {
                                count = CheckPlayersClass1(card, weaponClass, convertedClass, weaponId, kills);
                            }
                            else if (card.MissionType == MissionType.KILL_WEAPONCLASS || card.MissionType == MissionType.DOUBLE_KILL_WEAPONCLASS && slot.KillsOnLife == 2 || card.MissionType == MissionType.TRIPLE_KILL_WEAPONCLASS && slot.KillsOnLife == 3)
                            {
                                count = CheckPlayersClass2(card, kills);
                            }
                        }
                        else if (card.MissionType == MissionType.DEATHBLOW && autoComplete == MissionType.DEATHBLOW)
                        {
                            count = CheckPlayerClass(card, moreClass, moreConvClass, moreId);
                        }
                        else if (card.MissionType == autoComplete)
                        {
                            count = 1;
                        }
                    }
                    if (count == 0)
                    {
                        continue;
                    }

                    int ArrayIdx = card.ArrayIdx;
                    if (missionArray[ArrayIdx] + 1 > card.MissionLimit)
                    {
                        continue;
                    }
                    slot.MissionsCompleted = true;
                    missionArray[ArrayIdx] += (byte)count;
                    if (missionArray[ArrayIdx] > card.MissionLimit)
                    {
                        missionArray[ArrayIdx] = (byte)card.MissionLimit;
                    }

                    int progress = missionArray[ArrayIdx];
                    pR.SendPacket(new PROTOCOL_BASE_QUEST_CHANGE_ACK(progress, card));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex); ;
            }
        }
        private static void MissionCompleteBase(RoomModel room, Account pR, SlotModel slot, MissionType autoComplete, int moreInfo)
        {
            try
            {
                PlayerMissions missions = slot.Missions;
                if (missions == null)
                {
                    //Logger.error("Missions[2] null! by PlayerId: " + slot._playerId);
                    return;
                }
                int cmId = missions.GetCurrentMissionId(), cardId = missions.GetCurrentCard();
                if (cmId <= 0 || missions.SelectedCard)
                {
                    return;
                }
                List<MissionCardModel> cards = MissionCardRAW.GetCards(cmId, cardId);
                if (cards.Count == 0)
                {
                    return;
                }
                byte[] missionArray = missions.GetCurrentMissionList();

                ClassType moreClass = moreInfo > 0 ? ComDiv.GetIdClassType(moreInfo) : 0;
                ClassType moreConvClass = moreInfo > 0 ? ConvertWeaponClass(moreClass) : 0;
                int moreId = moreInfo > 0 ? ComDiv.GetIdStatics(moreInfo, 3) : 0;

                foreach (MissionCardModel card in cards)
                {
                    int count = 0;
                    if (card.MapId == 0 || card.MapId == (int)room.MapId)
                    {
                        if (card.MissionType == MissionType.DEATHBLOW && autoComplete == MissionType.DEATHBLOW)
                        {
                            count = CheckPlayerClass(card, moreClass, moreConvClass, moreId);
                        }
                        else if (card.MissionType == autoComplete)
                        {
                            count = 1;
                        }
                    }
                    if (count == 0)
                    {
                        continue;
                    }

                    int ArrayIdx = card.ArrayIdx;
                    if (missionArray[ArrayIdx] + 1 > card.MissionLimit)
                    {
                        continue;
                    }
                    slot.MissionsCompleted = true;
                    missionArray[ArrayIdx] += (byte)count;
                    if (missionArray[ArrayIdx] > card.MissionLimit)
                    {
                        missionArray[ArrayIdx] = (byte)card.MissionLimit;
                    }

                    int progress = missionArray[ArrayIdx];
                    pR.SendPacket(new PROTOCOL_BASE_QUEST_CHANGE_ACK(progress, card));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private static int CheckPlayersClass1(MissionCardModel card, ClassType weaponClass, ClassType convertedClass, int weaponId, FragInfos infos)
        {
            int count = 0;
            if ((card.WeaponReqId == 0 || card.WeaponReqId == weaponId) && (card.WeaponReq == ClassType.Unknown || card.WeaponReq == weaponClass || card.WeaponReq == convertedClass))
            {
                foreach (FragModel f in infos.Frags)
                {
                    if (f.VictimSlot % 2 != infos.KillerIdx % 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        private static int CheckPlayersClass2(MissionCardModel card, FragInfos infos)
        {
            int count = 0;
            foreach (FragModel f in infos.Frags)
            {
                if (f.VictimSlot % 2 != infos.KillerIdx % 2 && (card.WeaponReq == ClassType.Unknown || card.WeaponReq == (ClassType)f.VictimWeaponClass || card.WeaponReq == ConvertWeaponClass((ClassType)f.VictimWeaponClass)))
                {
                    count++;
                }
            }
            return count;
        }
        private static int CheckPlayerClass(MissionCardModel card, ClassType weaponClass, ClassType convertedClass, int weaponId, int killerId, FragModel frag)
        {
            if ((card.WeaponReqId == 0 || card.WeaponReqId == weaponId) && (card.WeaponReq == ClassType.Unknown || card.WeaponReq == weaponClass || card.WeaponReq == convertedClass))
            {
                if (frag.VictimSlot % 2 != killerId % 2)
                {
                    return 1;
                }
            }
            return 0;
        }
        private static int CheckPlayerClass(MissionCardModel card, ClassType weaponClass, ClassType convertedClass, int weaponId)
        {
            if ((card.WeaponReqId == 0 || card.WeaponReqId == weaponId) && (card.WeaponReq == ClassType.Unknown || card.WeaponReq == weaponClass || card.WeaponReq == convertedClass))
            {
                return 1;
            }
            return 0;
        }
        public static void EnableQuestMission(Account Player)
        {
            PlayerEvent Event = Player.Event;
            if (Event == null)
            {
                return;
            }
            if (Event.LastQuestFinish == 0 && EventQuestSync.GetRunningEvent() != null)
            {
                Player.Mission.Mission4 = 13;
            }
        }
        private static int GetCouponChild(int IndexKey, SortedList<int, int> Coupons)
        {
            if (Coupons.TryGetValue(IndexKey, out int Child))
            {
                return Child;
            }
            return 0;
        }
        private static int GetItemChild(int IndexKey, SortedList<int, int> Items)
        {
            if (Items.TryGetValue(IndexKey, out int Child))
            {
                return Child;
            }
            return 0;
        }
        private static int ValidateItemId(Account Player, int ItemId)
        {
            ItemsModel Item = Player.Inventory.GetItem(ItemId);
            if (Item != null)
            {
                return Item.Id;
            }
            return 0;
        }
        public static void ValidateAccesoryEquipment(Account Player, int AccessoryId)
        {
            if (Player.Equipment.AccessoryId != AccessoryId)
            {
                Player.Equipment.AccessoryId = ValidateItemId(Player, AccessoryId);
                ComDiv.UpdateDB("player_equipments", "accesory_id", Player.Equipment.AccessoryId, "owner_id", Player.PlayerId);
            }
        }
        public static void ValidateCouponEquipment(Account Player, SortedList<int, int> Coupons)
        {
            for (int i = 0; i < Coupons.Keys.Count; i++)
            {
                ItemsModel Item = Player.Inventory.GetItem(GetCouponChild(i, Coupons));
                if (Item != null)
                {
                    CouponFlag Effect = CouponEffectXML.GetCouponEffect(Item.Id);
                    if (Effect != null)
                    {
                        if (Effect.EffectFlag > 0 && Player.Effects.HasFlag(Effect.EffectFlag))
                        {
                            Player.Effects -= Effect.EffectFlag;
                        }
                        else if (Effect.EffectFlag > 0 && !Player.Effects.HasFlag(Effect.EffectFlag))
                        {
                            Player.Effects |= Effect.EffectFlag;
                        }
                        DaoManagerSQL.UpdateCouponEffect(Player.PlayerId, Player.Effects);
                    }
                    bool Changed = Player.Bonus.AddBonuses(Item.Id);
                    if (Changed)
                    {
                        DaoManagerSQL.UpdatePlayerBonus(Player.PlayerId, Player.Bonus.Bonuses, Player.Bonus.FreePass);
                    }
                }
            }
        }
        public static void ValidateCharacterEquipment(Account Player, PlayerEquipment Equip, int[] EquipmentList, int CharaPos, int FR_SLOT, int CT_SLOT)
        {
            DBQuery Query = new DBQuery();
            CharacterModel Chara = Player.Character.GetCharacter(CharaPos);
            if (Chara != null)
            {
                if (ComDiv.GetIdStatics(Chara.Id, 2) == 1 && FR_SLOT == Chara.Slot)
                {
                    if (Equip.CharaRedId != Chara.Id)
                    {
                        Equip.CharaRedId = Chara.Id;
                        Query.AddQuery("chara_red_side", Equip.CharaRedId);
                    }
                }
                else if (ComDiv.GetIdStatics(Chara.Id, 2) == 2 && CT_SLOT == Chara.Slot)
                {
                    if (Equip.CharaBlueId != Chara.Id)
                    {
                        Equip.CharaBlueId = Chara.Id;
                        Query.AddQuery("chara_blue_side", Equip.CharaBlueId);
                    }
                }
            }
            for (int ItemSlot = 0; ItemSlot < EquipmentList.Length; ItemSlot++)
            {
                ItemsModel Item = Player.Inventory.GetItem(EquipmentList[ItemSlot]);
                if (Item != null)
                {
                    switch (ItemSlot)
                    {
                        case 0:
                        {
                            if (Equip.WeaponPrimary != Item.Id)
                            {
                                Equip.WeaponPrimary = Item.Id;
                                Query.AddQuery("weapon_primary", Equip.WeaponPrimary);
                            }
                            break;
                        }
                        case 1:
                        {
                            if (Equip.WeaponSecondary != Item.Id)
                            {
                                Equip.WeaponSecondary = Item.Id;
                                Query.AddQuery("weapon_secondary", Equip.WeaponSecondary);
                            }
                            break;
                        }
                        case 2:
                        {
                            if (Equip.WeaponMelee != Item.Id)
                            {
                                Equip.WeaponMelee = Item.Id;
                                Query.AddQuery("weapon_melee", Equip.WeaponMelee);
                            }
                            break;
                        }
                        case 3:
                        {
                            if (Equip.WeaponExplosive != Item.Id)
                            {
                                Equip.WeaponExplosive = Item.Id;
                                Query.AddQuery("weapon_explosive", Equip.WeaponExplosive);
                            }
                            break;
                        }
                        case 4:
                        {
                            if (Equip.WeaponSpecial != Item.Id)
                            {
                                Equip.WeaponSpecial = Item.Id;
                                Query.AddQuery("weapon_special", Equip.WeaponSpecial);
                            }
                            break;
                        }
                        case 5:
                        {
                            if (Equip.PartHead != Item.Id)
                            {
                                Equip.PartHead = Item.Id;
                                Query.AddQuery("part_head", Equip.PartHead);
                            }
                            break;
                        }
                        case 6:
                        {
                            if (Equip.PartFace != Item.Id)
                            {
                                Equip.PartFace = Item.Id;
                                Query.AddQuery("part_face", Equip.PartFace);
                            }
                            break;
                        }
                        case 7:
                        {
                            if (Equip.PartJacket != Item.Id)
                            {
                                Equip.PartJacket = Item.Id;
                                Query.AddQuery("part_jacket", Equip.PartJacket);
                            }
                            break;
                        }
                        case 8:
                        {
                            if (Equip.PartPocket != Item.Id)
                            {
                                Equip.PartPocket = Item.Id;
                                Query.AddQuery("part_pocket", Equip.PartPocket);
                            }
                            break;
                        }
                        case 9:
                        {
                            if (Equip.PartGlove != Item.Id)
                            {
                                Equip.PartGlove = Item.Id;
                                Query.AddQuery("part_glove", Equip.PartGlove);
                            }
                            break;
                        }
                        case 10:
                        {
                            if (Equip.PartBelt != Item.Id)
                            {
                                Equip.PartBelt = Item.Id;
                                Query.AddQuery("part_belt", Equip.PartBelt);
                            }
                            break;
                        }
                        case 11:
                        {
                            if (Equip.PartHolster != Item.Id)
                            {
                                Equip.PartHolster = Item.Id;
                                Query.AddQuery("part_holster", Equip.PartHolster);
                            }
                            break;
                        }
                        case 12:
                        {
                            if (Equip.PartSkin != Item.Id)
                            {
                                Equip.PartSkin = Item.Id;
                                Query.AddQuery("part_skin", Equip.PartSkin);
                            }
                            break;
                        }
                        case 13:
                        {
                            if (Equip.BeretItem != Item.Id)
                            {
                                Equip.BeretItem = Item.Id;
                                Query.AddQuery("beret_item_part", Equip.BeretItem);
                            }
                            break;
                        }
                    }
                }
            }
            ComDiv.UpdateDB("player_equipments", "owner_id", Player.PlayerId, Query.GetTables(), Query.GetValues());
        }
        public static void ValidateItemEquipment(Account Player, SortedList<int, int> Items)
        {
            for (int i = 0; i < Items.Keys.Count; i++)
            {
                int ItemId = GetItemChild(i, Items);
                switch (i)
                {
                    case 0:
                    {
                        if (ItemId != 0 && Player.Equipment.DinoItem != ItemId)
                        {
                            Player.Equipment.DinoItem = ValidateItemId(Player, ItemId);
                            ComDiv.UpdateDB("player_equipments", "dino_item_chara", Player.Equipment.DinoItem, "owner_id", Player.PlayerId);
                        }
                        break;
                    }
                    case 1:
                    {
                        if (Player.Equipment.SprayId != ItemId)
                        {
                            Player.Equipment.SprayId = ValidateItemId(Player, ItemId);
                            ComDiv.UpdateDB("player_equipments", "spray_id", Player.Equipment.SprayId, "owner_id", Player.PlayerId);
                        }
                        break;
                    }
                    case 2:
                    {
                        if (Player.Equipment.NameCardId != ItemId)
                        {
                            Player.Equipment.NameCardId = ValidateItemId(Player, ItemId);
                            ComDiv.UpdateDB("player_equipments", "namecard_id", Player.Equipment.NameCardId, "owner_id", Player.PlayerId);
                        }
                        break;
                    }
                }
            }
        }
        public static void ValidateCharacterSlot(Account Player, PlayerEquipment Equip, int FR_SLOT, int CT_SLOT)
        {
            DBQuery Query = new DBQuery();
            CharacterModel RedChara = Player.Character.GetCharacterSlot(FR_SLOT);
            if (RedChara != null && Equip.CharaRedId != RedChara.Id)
            {
                Equip.CharaRedId = ValidateItemId(Player, RedChara.Id);
                Query.AddQuery("chara_red_side", Equip.CharaRedId);
            }
            CharacterModel BlueChara = Player.Character.GetCharacterSlot(CT_SLOT);
            if (BlueChara != null && Equip.CharaBlueId != BlueChara.Id)
            {
                Equip.CharaBlueId = ValidateItemId(Player, BlueChara.Id);
                Query.AddQuery("chara_blue_side", Equip.CharaBlueId);
            }
            ComDiv.UpdateDB("player_equipments", "owner_id", Player.PlayerId, Query.GetTables(), Query.GetValues());
        }
        public static PlayerEquipment ValidateRespawnEQ(Account Player, RoomModel Room, SlotModel Slot, int[] ItemIds)
        {
            PlayerEquipment Cache = new PlayerEquipment()
            {
                WeaponPrimary = ItemIds[0],
                WeaponSecondary = ItemIds[1],
                WeaponMelee = ItemIds[2],
                WeaponExplosive = ItemIds[3],
                WeaponSpecial = ItemIds[4],
                PartHead = ItemIds[6],
                PartFace = ItemIds[7],
                PartJacket = ItemIds[8],
                PartPocket = ItemIds[9],
                PartGlove = ItemIds[10],
                PartBelt = ItemIds[11],
                PartHolster = ItemIds[12],
                PartSkin = ItemIds[13],
                BeretItem = ItemIds[14],
                AccessoryId = ItemIds[15],
                CharaRedId = Player.Equipment.CharaRedId,
                CharaBlueId = Player.Equipment.CharaBlueId,
                DinoItem = Player.Equipment.DinoItem
            };
            if (Room.IsDinoMode())
            {
                if (!Room.SwapRound)
                {
                    if (Slot.Id % 2 == 0)
                    {
                        Cache.DinoItem = ItemIds[5];
                    }
                    else
                    {
                        Cache.CharaBlueId = ItemIds[5];
                    }
                }
                else
                {
                    if (Slot.Id % 2 == 0)
                    {
                        Cache.CharaRedId = ItemIds[5];
                    }
                    else
                    {
                        Cache.DinoItem = ItemIds[5];
                    }
                }
            }
            else
            {
                if (Slot.Id % 2 == 0)
                {
                    Cache.CharaRedId = ItemIds[5];
                }
                else
                {
                    Cache.CharaBlueId = ItemIds[5];
                }
            }
            return Cache;
        }
        public static void CheckEquipment(RoomModel Room, PlayerEquipment Equip)
        {
            List<GameRule> Rules = Room.GameRules;
            if (Rules.Count == 0)
            {
                return;
            }
            foreach (GameRule Rule in Rules)
            {
                int ItemClass = ComDiv.GetIdStatics(Rule.ItemId, 1), ClassType = ComDiv.GetIdStatics(Rule.ItemId, 2);
                if (ItemClass == 1)
                {
                    if (Equip.WeaponPrimary == Rule.ItemId)
                    {
                        switch (Room.WeaponsFlag)
                        {
                            case RoomWeaponsFlag.Assault: Equip.WeaponPrimary = 103004; break;
                            case RoomWeaponsFlag.SMG: Equip.WeaponPrimary = 104006; break;
                            case RoomWeaponsFlag.Sniper: Equip.WeaponPrimary = 105003; break;
                            case RoomWeaponsFlag.Shotgun: Equip.WeaponPrimary = 106001; break;
                            case RoomWeaponsFlag.Machinegun: Equip.WeaponPrimary = 110009; break;
                            default: Equip.WeaponPrimary = 103004; break;
                        }
                    }
                }
                if (ItemClass == 2)
                {
                    if (Equip.WeaponSecondary == Rule.ItemId)
                    {
                        Equip.WeaponSecondary = 202003;
                    }
                }
                if (ItemClass == 3)
                {
                    if (Equip.WeaponMelee == Rule.ItemId)
                    {
                        switch (Room.WeaponsFlag)
                        {
                            case RoomWeaponsFlag.Melee: Equip.WeaponMelee = 301001; break;
                            case RoomWeaponsFlag.Barefist: Equip.WeaponMelee = 323001; break;
                            default: Equip.WeaponMelee = 301001; break;
                        }    
                    }
                }
                if (ItemClass == 4)
                {
                    if (Equip.WeaponExplosive == Rule.ItemId)
                    {
                        Equip.WeaponExplosive = 407001;
                    }
                }
                if (ItemClass == 5)
                {
                    if (Equip.WeaponSpecial == Rule.ItemId)
                    {
                        Equip.WeaponSpecial = 508001;
                    }
                }
                if (ItemClass == 6)
                {
                    if (ClassType == 1)
                    {
                        if (Equip.CharaRedId == Rule.ItemId)
                        {
                            Equip.CharaRedId = 601001;
                        }
                    }
                    else if (ClassType == 2)
                    {
                        if (Equip.CharaBlueId == Rule.ItemId)
                        {
                            Equip.CharaBlueId = 602002;
                        }
                    }
                }
                if (ItemClass == 7)
                {
                    if (Equip.PartHead == Rule.ItemId)
                    {
                        Equip.PartHead = 1000700000;
                    }
                }
                if (ItemClass == 8)
                {
                    if (Equip.PartFace == Rule.ItemId)
                    {
                        Equip.PartFace = 1000800000;
                    }
                }
                if (ItemClass == 27)
                {
                    if (Equip.BeretItem == Rule.ItemId)
                    {
                        Equip.BeretItem = 0;
                    }
                }
                if (ItemClass == 30)
                {
                    if (Equip.AccessoryId == Rule.ItemId)
                    {
                        Equip.AccessoryId = 0;
                    }
                }
            }
        }
        public static void InsertItem(int ItemId, SlotModel Slot)
        {
            lock (Slot.ItemUsages)
            {
                if (!Slot.ItemUsages.Contains(ItemId))
                {
                    Slot.ItemUsages.Add(ItemId);
                }
            }
        }
        public static ushort DataFromItem(Account Player, ItemsModel Item, int Type, out List<ItemsModel> Charas, out List<ItemsModel> Weapons, out List<ItemsModel> Coupons)
        {
            Charas = new List<ItemsModel>();
            Weapons = new List<ItemsModel>();
            Coupons = new List<ItemsModel>();
            ItemsModel Model = new ItemsModel(Item);
            if (Model != null)
            {
                if (Type == 1)
                {
                    ComDiv.TryCreateItem(Model, Player.Inventory, Player.PlayerId);
                }
                SendItemInfo.LoadItem(Player, Model);
                if (Model.Category == ItemCategory.Weapon)
                {
                    Weapons.Add(Model);
                }
                else if (Model.Category == ItemCategory.Character)
                {
                    Charas.Add(Model);
                }
                else if (Model.Category == ItemCategory.Coupon)
                {
                    Coupons.Add(Model);
                }
            }
            return (ushort)Player.Inventory.Items.Count;
        }
        public static void ValidateBanPlayer(Account Player, string Message)
        {
            if (ConfigLoader.AutoBan)
            {
                if (ComDiv.UpdateDB("accounts", "access_level", -1, "player_id", Player.PlayerId))
                {
                    DaoManagerSQL.SaveAutoBan(Player.PlayerId, Player.Username, Player.Nickname, $"Cheat {Message})", DateTimeUtil.Now("dd -MM-yyyy HH:mm:ss"), Player.PublicIP.ToString(), "Illegal Program");
                    using (PROTOCOL_LOBBY_CHATTING_ACK packet = new PROTOCOL_LOBBY_CHATTING_ACK("Server", 0, 1, false, $"Permanently ban player [{Player.Nickname}], {Message}"))
                    {
                        GameXender.Client.SendPacketToAllClients(packet);
                    }
                    Player.Access = AccessLevel.BANNED;
                    Player.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                    Player.Close(1000, true);
                }
            }
            CLogger.Print($"Player: {Player.Nickname}; Id: {Player.PlayerId}; User: {Player.Username}; Reason: {Message}", LoggerType.Hack);
        }
        public static bool ServerCommands(Account Player, string Text)
        {
            try
            {
                bool Result = CommandManager.TryParse(Text, Player);
                if (Result)
                {
                    CLogger.Print($"Player '{Player.Nickname}' (UID: {Player.PlayerId}) Running Command '{Text}'", LoggerType.Command);
                }
                return Result;
            }
            catch
            {
                string ErrorMessage = "An unpredicted error while send command was found!, please report to Server Moderator, ASAP!";
                Player.Connection.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK("Server", 0, 5, false, ErrorMessage));
                return true;
            }
        }
        public static bool SlotValidMessage(SlotModel Sender, SlotModel Receiver)
        {
            return ((Sender.State == SlotState.NORMAL || Sender.State == SlotState.READY) && (Receiver.State == SlotState.NORMAL || Receiver.State == SlotState.READY) || (Sender.State >= SlotState.LOAD && Receiver.State >= SlotState.LOAD) && (Receiver.SpecGM || Sender.SpecGM || Sender.DeathState.HasFlag(DeadEnum.UseChat) || Sender.DeathState.HasFlag(DeadEnum.Dead) && Receiver.DeathState.HasFlag(DeadEnum.Dead) || Sender.Spectator && Receiver.Spectator || Sender.DeathState.HasFlag(DeadEnum.Alive) && Receiver.DeathState.HasFlag(DeadEnum.Alive) && (Sender.Spectator && Receiver.Spectator || !Sender.Spectator && !Receiver.Spectator)));
        }
        public static bool PlayerIsBattle(Account Player)
        {
            RoomModel Room = Player.Room;
            return (Room != null && Room.GetSlot(Player.SlotId, out SlotModel Slot) && Slot.State >= SlotState.READY);
        }
        public static void RoomPingSync(RoomModel Room, DateTime LastPingSync = new DateTime())
        {
            double Seconds = ComDiv.GetDuration(Room.LastPingSync);
            if ((ConfigLoader.LimitedPing && Seconds < ConfigLoader.PingUpdateTime) || Seconds < 1)
            {
                return;
            }
            byte[] Pings = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                Pings[i] = (byte)Room.Slots[i].Ping;
            }
            using (PROTOCOL_BATTLE_SENDPING_ACK Packet = new PROTOCOL_BATTLE_SENDPING_ACK(Pings))
            {
                Room.SendPacketToPlayers(Packet, SlotState.BATTLE, 0);
            }
            Room.LastPingSync = DateTimeUtil.Now();
        }
        public static List<ItemsModel> RepairableItems(Account Player, long ObjectId, int State, out int Gold, out int Cash, out uint Error)
        {
            Gold = 0;
            Cash = 0;
            Error = 0;
            List<ItemsModel> Items = new List<ItemsModel>();
            if (State == 1 && ObjectId == 0)
            {
                foreach (int ItemId in CharaEquipments(Player))
                {
                    ItemsModel Item = Player.Inventory.GetItem(ItemId);
                    ItemsRepair Repair = ShopManager.GetRepairItem(Item.Id);
                    if (Item != null && Repair != null)
                    {
                        uint[] Value = ValidateAccountValueable(Player, Item, Repair);
                        Gold += (int)Value[0];
                        Cash += (int)Value[1];
                        Error = Value[2];
                        Items.Add(Item);
                    }
                }
            }
            else if (State == 1 && ObjectId > 0)
            {
                ItemsModel Item = Player.Inventory.GetItem(ObjectId);
                ItemsRepair Repair = ShopManager.GetRepairItem(Item.Id);
                if (Item != null && Repair != null)
                {
                    uint[] Value = ValidateAccountValueable(Player, Item, Repair);
                    Gold += (int)Value[0];
                    Cash += (int)Value[1];
                    Error = Value[2];
                    Items.Add(Item);
                }
            }
            else
            {
                Error = 0x80000110;
            }
            return Items;
        }
        private static List<int> CharaEquipments(Account Player)
        {
            List<int> Equipments = new List<int>();
            PlayerEquipment Equip = Player.Equipment;
            if (Equip != null)
            {
                Equipments.Add(Equip.WeaponPrimary);
                Equipments.Add(Equip.WeaponSecondary);
                Equipments.Add(Equip.WeaponMelee);
                Equipments.Add(Equip.WeaponExplosive);
                Equipments.Add(Equip.WeaponSpecial);
                Equipments.Add(Equip.CharaRedId);
                Equipments.Add(Equip.CharaBlueId);
                Equipments.Add(Equip.PartHead);
                Equipments.Add(Equip.PartFace);
                Equipments.Add(Equip.BeretItem);
                Equipments.Add(Equip.DinoItem);
            }
            return Equipments;
        }
        private static uint[] ValidateAccountValueable(Account Player, ItemsModel Item, ItemsRepair Repair)
        {
            uint[] Cache = new uint[3] { 0, 0, 0 };
            uint Value = (Repair.Quantity - Item.Count), Cash = 0, Gold = 0;
            if (Repair.Point > 0 && Repair.Cash == 0)
            {
                Gold = (uint)ComDiv.Percentage(Repair.Point, (int)Value);
                if (Player.Gold < (Gold + ComDiv.Percentage(Gold, 10)))
                {
                    Cache[2] = 0x80000110;
                    return Cache;
                }
            }
            else if (Repair.Cash > 0 && Repair.Point == 0)
            {
                Cash = (uint)ComDiv.Percentage(Repair.Cash, (int)Value);
                if (Player.Cash < (Cash + ComDiv.Percentage(Cash, 10)))
                {
                    Cache[2] = 0x80000110;
                    return Cache;
                }
            }
            Item.Count = Repair.Quantity;
            ComDiv.UpdateDB("player_items", "count", (long)Item.Count, "owner_id", Player.PlayerId, "id", Item.Id);
            Cache[0] = Gold;
            Cache[1] = Cash;
            Cache[2] = 1;
            return Cache;
        }
        public static bool ChannelRequirementCheck(Account Player, ChannelModel Channel)
        {
            if (Player.IsGM() || Player.HaveAcessLevel())
            {
                return false;
            }
            if (Channel.Type == ChannelType.Clan && Player.ClanId == 0)
            {
                return true;
            }
            else if (Channel.Type == ChannelType.Novice && (Player.Statistic.GetKDRatio() > 40 && Player.Statistic.GetSeasonKDRatio() > 40)) //Canal iniciante1 | KD abaixo de 40%
            {
                return true;
            }
            else if (Channel.Type == ChannelType.Training && Player.Rank >= 4)
            {
                return true;
            }
            else if (Channel.Type == ChannelType.Special && Player.Rank <= 25)
            {
                return true;
            }
            else if (Channel.Type == ChannelType.Blocked)
            {
                return true;
            }
            return false;
        }
    }
}
