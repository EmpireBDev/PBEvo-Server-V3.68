using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Plugin.Core.SQL;
using Plugin.Core.XML;
using Server.Game.Data.Managers;
using Server.Game.Data.Sync.Server;
using Server.Game.Data.Utils;
using Server.Game.Data.XML;
using Server.Game.Network;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Data.Models
{
    public class RoomModel
    {
        public SlotModel[] Slots = new SlotModel[16];
        public int NewInt;
        public ChannelType ChannelType;
        public int Rounds;
        public int TRex;
        public int CTRounds;
        public int CTDino;
        public int FRRounds;
        public int FRDino;
        public int Bar1;
        public int Bar2;
        public int Ping;
        public int FRKills;
        public int FRDeaths;
        public int FRAssists;
        public int CTKills;
        public int CTDeaths;
        public int CTAssists;
        public int SpawnsCount;
        public int KillTime;
        public int RoomId;
        public int ChannelId;
        public int ServerId;
        public int Leader;
        public int CountPlayers;
        public int CountMaxSlots;
        public byte Limit;
        public byte WatchRuleFlag;
        public byte AiCount;
        public byte IngameAiLevel;
        public byte AiLevel;
        public byte AiType;
        public byte CountdownIG;
        public byte KillCam;
        public short BalanceType;
        public readonly int[] TIMES = new int[11] { 3, 3, 3, 5, 7, 5, 10, 15, 20, 25, 30 };
        public readonly int[] KILLS = new int[9] { 15, 30, 50, 60, 80, 100, 120, 140, 160 };
        public readonly int[] ROUNDS = new int[6] { 1, 2, 3, 5, 7, 9 };
        public readonly int[] FR_TEAM = new int[8] { 0, 2, 4, 6, 8, 10, 12, 14 };
        public readonly int[] CT_TEAM = new int[8] { 1, 3, 5, 7, 9, 11, 13, 15 };
        public readonly int[] ALL_TEAM = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        public byte[] RandomMaps;
        public byte[] LeaderAddr = new byte[4];
        public byte[] HitParts = new byte[35] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 };
        public uint TimeRoom;
        public uint StartDate;
        public uint UniqueRoomId;
        public uint Seed;
        public long StartTick;
        public string Name;
        public string Password;
        public string MapName;
        public string LeaderName;
        public VoteKickModel votekick;
        public MapIdEnum MapId;
        public RoomCondition RoomType;
        public RoomState State;
        public MapRules Rule;
        public StageOptions Stage;
        public RoomStageFlag Flag = RoomStageFlag.NONE;
        public RoomWeaponsFlag WeaponsFlag = RoomWeaponsFlag.None;
        public bool ActiveC4;
        public bool SwapRound;
        public bool ChangingSlots;
        public bool BlockedClan;
        public bool PreMatchCD;
        public Synchronize UdpServer;
        public DateTime BattleStart;
        public DateTime LastPingSync = DateTimeUtil.Now();
        public TimerState BombTime = new TimerState();
        public TimerState CountdownTime = new TimerState();
        public TimerState RoundTime = new TimerState();
        public TimerState VoteTime = new TimerState();
        public TimerState PreMatchTime = new TimerState();
        public SafeList<long> KickedPlayers = new SafeList<long>();
        public SafeList<long> RequestHost = new SafeList<long>();
        public List<GameRule> GameRules = new List<GameRule>();
        public RoomModel(int RoomId, ChannelModel Channel)
        {
            this.RoomId = RoomId;
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i] = new SlotModel(i);
            }
            ChannelId = Channel.Id;
            ChannelType = Channel.Type;
            ServerId = Channel.ServerId;
            Rounds = 1;
            AiCount = 1;
            TRex = -1;
            Ping = 5;
            Name = "";
            Password = "";
            MapName = "";
            LeaderName = "";
            SetUniqueId();
            GameRules = GameRuleXML.GetGameRules(ConfigLoader.RuleId);
        }
        public bool ThisModeHaveCD()
        {
            RoomCondition StageType = RoomType;
            return StageType == RoomCondition.Bomb || StageType == RoomCondition.Annihilation || StageType == RoomCondition.Boss || StageType == RoomCondition.CrossCounter || StageType == RoomCondition.Convoy || StageType == RoomCondition.Ace;
        }
        public bool ThisModeHaveRounds()
        {
            RoomCondition StageType = RoomType;
            return StageType == RoomCondition.Bomb || StageType == RoomCondition.Destroy || StageType == RoomCondition.Annihilation || StageType == RoomCondition.Defense || StageType == RoomCondition.Convoy || StageType == RoomCondition.Ace;
        }
        public int GetFlag()
        {
            int Result = 0;
            if (Flag.HasFlag(RoomStageFlag.TEAM_SWAP))
            {
                Result += 1;
            }
            if (Flag.HasFlag(RoomStageFlag.RANDOM_MAP))
            {
                Result += 2;
            }
            if (Flag.HasFlag(RoomStageFlag.PASSWORD) || Password.Length > 0)
            {
                Result += 4;
            }
            if (Flag.HasFlag(RoomStageFlag.OBSERVER_MODE))
            {
                Result += 8;
            }
            if (Flag.HasFlag(RoomStageFlag.REAL_IP))
            {
                Result += 16;
            }
            if (Flag.HasFlag(RoomStageFlag.TEAM_BALANCE) || BalanceType == 1)
            {
                Result += 32;
            }
            if (Flag.HasFlag(RoomStageFlag.OBSERVER))
            {
                Result += 64;
            }
            if (Flag.HasFlag(RoomStageFlag.INTER_ENTER) || (Limit > 0 && State > RoomState.Ready))
            {
                Result += 128;
            }
            Flag = (RoomStageFlag)Result;
            return Result;
        }
        private void SetUniqueId()
        {
            UniqueRoomId = (uint)((ServerId & 0xFF) << 20 | (ChannelId & 0xFF) << 12 | RoomId & 0xFFF);
        }
        public void GenerateSeed()
        {
            Seed = (uint)(((int)MapId & 0xFF) << 20 | ((byte)Rule & 0xFF) << 12 | (int)RoomType & 0xFFF);
        }
        public void SetBotLevel()
        {
            if (!IsBotMode())
            {
                return;
            }
            IngameAiLevel = AiLevel;
            for (int i = 0; i < 16; i++)
            {
                Slots[i].AiLevel = IngameAiLevel;
            }
        }
        public bool IsBotMode()
        {
            return Stage == StageOptions.AI || Stage == StageOptions.DieHard || Stage == StageOptions.Infection;
        }
        private void SetSpecialStage()
        {
            if (RoomType == RoomCondition.Defense)
            {
                if (MapId == MapIdEnum.BlackPanther)
                {
                    Bar1 = 6000;
                    Bar2 = 9000;
                }
            }
            else if (RoomType == RoomCondition.Destroy)
            {
                if (MapId == MapIdEnum.Hospital)
                {
                    Bar1 = 12000;
                    Bar2 = 12000;
                }
                else if (MapId == MapIdEnum.BreakDown)
                {
                    Bar1 = 6000;
                    Bar2 = 6000;
                }
            }
        }
        public int GetInBattleTime()
        {
            int seconds = 0;
            if (BattleStart != new DateTime() && (State == RoomState.Battle || State == RoomState.PreBattle))
            {
                DateTime now = DateTimeUtil.Now();
                seconds = (int)(now - BattleStart).TotalSeconds;
                if (seconds < 0)
                {
                    seconds = 0;
                }
            }
            return seconds;
        }
        public int GetInBattleTimeLeft()
        {
            int remaining = GetInBattleTime();
            return GetTimeByMask() * 60 - remaining;
        }
        public ChannelModel GetChannel()
        {
            return ChannelsXML.GetChannel(ServerId, ChannelId);
        }
        public bool GetChannel(out ChannelModel Channel)
        {
            Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            return Channel != null;
        }
        public bool GetSlot(int slotIdx, out SlotModel slot)
        {
            slot = null;
            lock (Slots)
            {
                if (slotIdx >= 0 && slotIdx <= 15)
                {
                    slot = Slots[slotIdx];
                }
                return slot != null;
            }
        }
        public SlotModel GetSlot(int slotIdx)
        {
            lock (Slots)
            {
                if (slotIdx >= 0 && slotIdx <= 15)
                {
                    return Slots[slotIdx];
                }
                return null;
            }
        }
        public void StartCounter(int type, Account player, SlotModel slot)
        {
            int dueTime = 0;
            EventErrorEnum error = 0;
            if (type == 0)
            {
                error = EventErrorEnum.BATTLE_FIRST_MAINLOAD;
                dueTime = 90000;
            }
            else if (type == 1)
            {
                error = EventErrorEnum.BATTLE_FIRST_HOLE;
                dueTime = 30000;
            }
            else
            {
                return;
            }
            slot.Timing.Start(dueTime, (callbackState) =>
            {
                BaseCounter(error, player, slot);
                lock (callbackState)
                {
                    if (slot != null)
                    {
                        slot.StopTiming();
                    }
                }
            });
        }
        private void BaseCounter(EventErrorEnum error, Account player, SlotModel slot)
        {
            player.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(error));
            player.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(player, 0));
            slot.State = SlotState.NORMAL;
            AllUtils.BattleEndPlayersCount(this, IsBotMode());
            UpdateSlotsInfo();
        }
        public void StartBomb()
        {
            try
            {
                BombTime.Start(42000, (callbackState) =>
                {
                    if (this != null && ActiveC4)
                    {
                        FRRounds++;
                        ActiveC4 = false;
                        AllUtils.BattleEndRound(this, TeamEnum.FR_TEAM, RoundEndType.BombFire);
                    }
                    lock (callbackState)
                    {
                        BombTime.Timer = null;
                    }
                });
            }
            catch (Exception ex)
            {
                CLogger.Print($"StartBomb: {ex.Message}", LoggerType.Error, ex);
            }
        }
        public void StartVote()
        {
            try
            {
                if (votekick == null)
                {
                    return;
                }

                VoteTime.Start(20000, (callbackState) =>
                {
                    AllUtils.VotekickResult(this);
                    lock (callbackState)
                    {
                        VoteTime.Timer = null;
                    }
                });
            }
            catch (Exception ex)
            {
                CLogger.Print($"StartVote: {ex.Message}", LoggerType.Error, ex);
                if (VoteTime.Timer != null)
                {
                    VoteTime.Timer = null;
                }
                votekick = null;
            }
        }
        public void RoundRestart()
        {
            try
            {
                StopBomb();
                foreach (SlotModel Slot in Slots)
                {
                    if (Slot.PlayerId > 0 && Slot.State == SlotState.BATTLE)
                    {
                        if (!Slot.DeathState.HasFlag(DeadEnum.UseChat))
                        {
                            Slot.DeathState |= DeadEnum.UseChat;
                        }
                        if (Slot.Spectator)
                        {
                            Slot.Spectator = false;
                        }
                        if (Slot.KillsOnLife >= 3 && (RoomType == RoomCondition.Annihilation || RoomType == RoomCondition.Ace))
                        {
                            Slot.Objects++;
                        }
                        Slot.KillsOnLife = 0;
                        Slot.LastKillState = 0;
                        Slot.RepeatLastState = false;
                        Slot.DamageBar1 = 0;
                        Slot.DamageBar2 = 0;
                    }
                }
                RoundTime.Start(8000, (callbackState) =>
                {
                    foreach (SlotModel s in Slots)
                    {
                        if (s.PlayerId > 0)
                        {
                            if (!s.DeathState.HasFlag(DeadEnum.UseChat))
                            {
                                s.DeathState |= DeadEnum.UseChat;
                            }
                            if (s.Spectator)
                            {
                                s.Spectator = false;
                            }
                        }
                    }
                    StopBomb();
                    DateTime Now = DateTimeUtil.Now();
                    if (State == RoomState.Battle)
                    {
                        BattleStart = IsDinoMode() ? Now.AddSeconds(5) : Now;
                    }
                    using (PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK packet = new PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(this))
                    {
                        using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK packet2 = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(this))
                        {
                            SendPacketToPlayers(packet, packet2, SlotState.BATTLE, 0);
                        }
                    }
                    StopBomb();
                    SwapRound = false;
                    //AllUtils.LogRoomRoundRestart(this);
                    lock (callbackState)
                    {
                        RoundTime.Timer = null;
                    }
                });
            }
            catch (Exception ex)
            {
                CLogger.Print($"Room.RoundRestart: {ex.Message}", LoggerType.Error, ex);
            }
        }
        public void StopBomb()
        {
            if (!ActiveC4)
            {
                return;
            }
            ActiveC4 = false;
            if (BombTime != null)
            {
                BombTime.Timer = null;
            }
        }
        public void StartBattle(bool updateInfo)
        {
            lock (Slots)
            {
                State = RoomState.Loading;
                RequestHost.Clear();
                UdpServer = SynchronizeXML.GetServer(ConfigLoader.DEFAULT_PORT[2]);
                StartTick = DateTimeUtil.Now().Ticks;
                StartDate = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
                SetBotLevel();
                AllUtils.CheckClanMatchRestrict(this);
                using (PROTOCOL_BATTLE_START_GAME_ACK Packet = new PROTOCOL_BATTLE_START_GAME_ACK(this))
                {
                    byte[] Data = Packet.GetCompleteBytes("Room.StartBattle");
                    foreach (Account Player in GetAllPlayers(SlotState.READY, 0))
                    {
                        SlotModel Slot = GetSlot(Player.SlotId);
                        if (Slot != null)
                        {
                            Slot.WithHost = true;
                            Slot.State = SlotState.LOAD;
                            Slot.SetMissionsClone(Player.Mission);
                            Player.SendCompletePacket(Data, Packet.GetType().Name);
                        }
                    }
                }
                if (updateInfo)
                {
                    UpdateSlotsInfo();
                }
                UpdateRoomInfo();
            }
        }
        public void StartCountDown()
        {
            using (PROTOCOL_BATTLE_START_COUNTDOWN_ACK Packet = new PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum.Start))
            {
                SendPacketToPlayers(Packet);
            }
            CountdownTime.Start(5250, (callbackState) =>
            {
                try
                {
                    if (Slots[Leader].State == SlotState.READY && State == RoomState.CountDown)
                    {
                        StartBattle(true);
                    }
                }
                catch (Exception ex)
                {
                    CLogger.Print($"Room.StartCoundown: {ex.Message}", LoggerType.Error, ex);
                }
                lock (callbackState)
                {
                    CountdownTime.Timer = null;
                }
            });
        }
        public void StopCountDown(CountDownEnum motive, bool refreshRoom = true)
        {
            State = RoomState.Ready;
            if (refreshRoom)
            {
                UpdateRoomInfo();
            }
            CountdownTime.Timer = null;
            using (PROTOCOL_BATTLE_START_COUNTDOWN_ACK packet = new PROTOCOL_BATTLE_START_COUNTDOWN_ACK(motive))
            {
                SendPacketToPlayers(packet);
            }
        }
        public void StopCountDown(int slotId)
        {
            if (State != RoomState.CountDown)
            {
                return;
            }
            if (slotId == Leader)
            {
                StopCountDown(CountDownEnum.StopByHost);
            }
            else if (GetPlayingPlayers((TeamEnum)(Leader % 2 == 0 ? 1 : 0), SlotState.READY, 0) == 0)
            {
                ChangeSlotState(Leader, SlotState.NORMAL, false);
                StopCountDown(CountDownEnum.StopByPlayer);
            }
        }
        public void CalculateResult()
        {
            lock (Slots)
            {
                BaseResultGame(AllUtils.GetWinnerTeam(this), IsBotMode());
            }
        }
        public void CalculateResult(TeamEnum resultType)
        {
            lock (Slots)
            {
                BaseResultGame(resultType, IsBotMode());
            }
        }
        public void CalculateResult(TeamEnum resultType, bool isBotMode)
        {
            lock (Slots)
            {
                BaseResultGame(resultType, isBotMode);
            }
        }
        public void CalculateResultFreeForAll(int SlotWin)
        {
            lock (Slots)
            {
                BaseResultGame((TeamEnum)SlotWin, false);
            }
        }
        private void BaseResultGame(TeamEnum winnerTeam, bool isBotMode)
        {
            ServerConfig cfg = GameXender.Client.Config;
            EventRankUpModel evUp = EventRankUpSync.GetRunningEvent();
            EventMapModel evMap = EventMapSync.GetRunningEvent();
            bool mapEvUse = EventMapSync.EventIsValid(evMap, (int)MapId, (int)RoomType);
            EventPlaytimeModel evPt = EventPlaytimeSync.GetRunningEvent();
            DateTime finishDate = DateTimeUtil.Now();
            int[] Array = new int[16];
            int SlotWin = 0;
            if (cfg == null)
            {
                CLogger.Print("Server Config Null. RoomResult canceled.", LoggerType.Warning);
                return;
            }
            for (int i = 0; i < 16; i++)
            {
                SlotModel Slot = Slots[i];
                if (Slot.PlayerId != 0)
                {
                    Array[i] = Slot.AllKills;
                }
                else
                {
                    Array[i] = 0;
                }
                if (Array[i] > Array[SlotWin])
                {
                    SlotWin = i;
                }
                if (!Slot.Check && Slot.State == SlotState.BATTLE && GetPlayerBySlot(Slot, out Account Player))
                {
                    StatBasic Basic = Player.Statistic.Basic;
                    StatSeason Season = Player.Statistic.Season;
                    StatDaily Dailies = Player.Statistic.Daily;
                    StatWeapon Weapons = Player.Statistic.Weapon;
                    DBQuery AccountQuery = new DBQuery(), TotalQuery = new DBQuery(), SeasonQuery = new DBQuery(), DailyQuery = new DBQuery(), WeaponQuery = new DBQuery();
                    Slot.Check = true;
                    double inBattleTime = Slot.InBattleTime(finishDate);
                    int Gold = Player.Gold, Exp = Player.Exp, Cash = Player.Cash;
                    if (!isBotMode)
                    {
                        if (cfg.Missions)
                        {
                            AllUtils.EndMatchMission(this, Player, Slot, winnerTeam);
                            if (Slot.MissionsCompleted)
                            {
                                Player.Mission = Slot.Missions;
                                DaoManagerSQL.UpdateCurrentPlayerMissionList(Player.PlayerId, Player.Mission);
                            }
                            AllUtils.GenerateMissionAwards(Player, AccountQuery);
                        }
                        int timePlayed = Slot.AllKills == 0 && Slot.AllDeaths == 0 ? (int)(inBattleTime / 3) : (int)inBattleTime;
                        if (RoomType == RoomCondition.Bomb || RoomType == RoomCondition.Annihilation || RoomType == RoomCondition.Ace)
                        {
                            Slot.Exp = (int)(Slot.Score + timePlayed / 2.5 + Slot.AllDeaths * 2.2 + Slot.Objects * 20);
                            Slot.Gold = (int)(Slot.Score + timePlayed / 3.0 + Slot.AllDeaths * 2.2 + Slot.Objects * 20);
                            Slot.Cash = (int)(Slot.Score / 2 + timePlayed / 6.5 + Slot.AllDeaths * 1.5 + Slot.Objects * 10);
                        }
                        else
                        {
                            Slot.Exp = (int)(Slot.Score + timePlayed / 2.5 + Slot.AllDeaths * 1.8 + Slot.Objects * 20);
                            Slot.Gold = (int)(Slot.Score + timePlayed / 3.0 + Slot.AllDeaths * 1.8 + Slot.Objects * 20);
                            Slot.Cash = (int)(Slot.Score / 1.5 + timePlayed / 4.5 + Slot.AllDeaths * 1.1 + Slot.Objects * 20);
                        }
                        bool WonTheMatch = Slot.Team == winnerTeam;
                        if (Rule != MapRules.Chaos && Rule != MapRules.HeadHunter)
                        {
                            Basic.HeadshotsCount += Slot.AllHeadshots;
                            Basic.KillsCount += Slot.AllKills;
                            Basic.TotalKillsCount += Slot.AllKills;
                            Basic.DeathsCount += Slot.AllDeaths;
                            Basic.AssistsCount += Slot.AllAssists;
                            Season.HeadshotsCount += Slot.AllHeadshots;
                            Season.KillsCount += Slot.AllKills;
                            Season.TotalKillsCount += Slot.AllKills;
                            Season.DeathsCount += Slot.AllDeaths;
                            Season.AssistsCount += Slot.AllAssists;
                            AddKDInfosToQuery(Slot, Player.Statistic, TotalQuery, SeasonQuery);
                            if (RoomType == RoomCondition.FreeForAll)
                            {
                                AllUtils.UpdateMatchCountFreeForAll(this, Player, SlotWin, TotalQuery, SeasonQuery);
                            }
                            else
                            {
                                AllUtils.UpdateMatchCount(WonTheMatch, Player, (int)winnerTeam, TotalQuery, SeasonQuery);

                            }
                            Dailies.KillsCount += Slot.AllKills;
                            Dailies.DeathsCount += Slot.AllDeaths;
                            Dailies.HeadshotsCount += Slot.AllHeadshots;
                            AddDailyToQuery(Slot, Player.Statistic, DailyQuery);
                            if (RoomType == RoomCondition.FreeForAll)
                            {
                                AllUtils.UpdateMatchDailyRecordFreeForAll(this, Player, SlotWin, DailyQuery);
                            }
                            else
                            {
                                AllUtils.UpdateDailyRecord(WonTheMatch, Player, (int)winnerTeam, DailyQuery);
                            }
                            Weapons.AssaultKills += Slot.AR[0];
                            Weapons.AssaultDeaths += Slot.AR[1];
                            Weapons.SmgKills += Slot.SMG[0];
                            Weapons.SmgDeaths += Slot.SMG[1];
                            Weapons.SniperKills += Slot.SR[0];
                            Weapons.SniperDeaths += Slot.SR[1];
                            Weapons.ShotgunKills += Slot.SG[0];
                            Weapons.ShotgunDeaths += Slot.SG[1];
                            Weapons.MachinegunKills += Slot.MG[0];
                            Weapons.MachinegunDeaths += Slot.MG[1];
                            AllUtils.UpdateWeaponRecord(Player, Slot, WeaponQuery);
                        }
                        if (WonTheMatch || RoomType == RoomCondition.FreeForAll && (int)winnerTeam == SlotWin)
                        {
                            Slot.Gold += AllUtils.Percentage(Slot.Gold, 15);
                            Slot.Exp += AllUtils.Percentage(Slot.Exp, 20);
                        }
                        if (Slot.EarnedEXP > 0)
                        {
                            Slot.Exp += Slot.EarnedEXP * 5;
                        }
                    }
                    else
                    {
                        Slot.Gold += 300;
                        Slot.Exp += 300;
                    }
                    Slot.Exp = Slot.Exp > ConfigLoader.MaxExpReward ? ConfigLoader.MaxExpReward : Slot.Exp;
                    Slot.Gold = Slot.Gold > ConfigLoader.MaxGoldReward ? ConfigLoader.MaxGoldReward : Slot.Gold;
                    Slot.Cash = Slot.Cash > ConfigLoader.MaxCashReward ? ConfigLoader.MaxCashReward : Slot.Cash;
                    if (RoomType == RoomCondition.Ace)
                    {
                        if (!(Player.SlotId >= 0 && Player.SlotId <= 1))
                        {
                            Slot.Exp = 0;
                            Slot.Gold = 0;
                            Slot.Cash = 0;
                        }
                    }
                    else
                    {
                        if (Slot.Exp < 0 || Slot.Gold < 0 || Slot.Cash < 0)
                        {
                            Slot.Exp = 2;
                            Slot.Gold = 2;
                            Slot.Cash = 2;
                        }
                    }
                    int ItemExp = 0, ItemPoint = 0, CafeExp = 0, CafePoint = 0, EventExp = 0, EventPoint = 0;
                    if (evUp != null || mapEvUse)
                    {
                        if (evUp != null)
                        {
                            EventExp += evUp.PercentExp;
                            EventPoint += evUp.PercentGold;
                        }
                        if (mapEvUse)
                        {
                            EventExp += evMap.PercentExp;
                            EventPoint += evMap.PercentGold;
                        }
                        if (!Slot.BonusFlags.HasFlag(ResultIcon.Event))
                        {
                            Slot.BonusFlags |= ResultIcon.Event;
                        }
                        //slot.BonusEventExp += AllUtils.percentage(EventExp, 100);
                        //slot.BonusEventPoint += AllUtils.percentage(EventPoint, 100);
                    }
                    PlayerBonus Bonus = Player.Bonus;
                    if (Bonus != null && Bonus.Bonuses > 0)
                    {
                        if ((Bonus.Bonuses & 8) == 8)
                        {
                            ItemExp += 100;
                        }
                        if ((Bonus.Bonuses & 128) == 128)
                        {
                            ItemPoint += 100;
                        }
                        if ((Bonus.Bonuses & 4) == 4)
                        {
                            ItemExp += 50;
                        }
                        if ((Bonus.Bonuses & 64) == 64)
                        {
                            ItemPoint += 50;
                        }
                        if ((Bonus.Bonuses & 2) == 2)
                        {
                            ItemExp += 30;
                        }
                        if ((Bonus.Bonuses & 32) == 32)
                        {
                            ItemPoint += 30;
                        }
                        if ((Bonus.Bonuses & 1) == 1)
                        {
                            ItemExp += 10;
                        }
                        if ((Bonus.Bonuses & 16) == 16)
                        {
                            ItemPoint += 10;
                        }
                        if (!Slot.BonusFlags.HasFlag(ResultIcon.Item))
                        {
                            Slot.BonusFlags |= ResultIcon.Item;
                        }
                        Slot.BonusItemExp += ItemExp;
                        Slot.BonusItemPoint += ItemPoint;
                    }
                    InternetCafe CafeBonus = InternetCafeXML.GetICafe(ConfigLoader.InternetCafeId);
                    if (CafeBonus != null && (Player.CafePC == CafeEnum.Gold || Player.CafePC == CafeEnum.Silver))
                    {
                        CafeExp += Player.CafePC == CafeEnum.Gold ? CafeBonus.PremiumExp : CafeBonus.BasicExp; //120 : 60;
                        CafePoint += Player.CafePC == CafeEnum.Gold ? CafeBonus.PremiumGold : CafeBonus.BasicGold; //100 : 40;
                        if (Player.CafePC == CafeEnum.Silver && !Slot.BonusFlags.HasFlag(ResultIcon.Pc))
                        {
                            Slot.BonusFlags |= ResultIcon.Pc;
                        }
                        else if (Player.CafePC == CafeEnum.Gold && !Slot.BonusFlags.HasFlag(ResultIcon.PcPlus))
                        {
                            Slot.BonusFlags |= ResultIcon.PcPlus;
                        }
                        Slot.BonusCafePoint += CafePoint;
                        Slot.BonusCafeExp += CafeExp;
                    }
                    if (isBotMode)
                    {
                        if (Slot.BonusItemExp > 300)
                        {
                            Slot.BonusItemExp = 300;
                        }
                        if (Slot.BonusItemPoint > 300)
                        {
                            Slot.BonusItemPoint = 300;
                        }
                        if (Slot.BonusCafeExp > 300)
                        {
                            Slot.BonusCafeExp = 300;
                        }
                        if (Slot.BonusCafePoint > 300)
                        {
                            Slot.BonusCafePoint = 300;
                        }
                        if (Slot.BonusEventExp > 300)
                        {
                            Slot.BonusEventExp = 300;
                        }
                        if (Slot.BonusEventPoint > 300)
                        {
                            Slot.BonusEventPoint = 300;
                        }
                    }
                    int BonusAllexp = EventExp + Slot.BonusCafeExp + Slot.BonusItemExp;
                    int BonusAllPoint = EventPoint + Slot.BonusCafePoint + Slot.BonusItemPoint;
                    Slot.BonusEventExp = AllUtils.Percentage(Slot.Exp, BonusAllexp);
                    Slot.BonusEventPoint = AllUtils.Percentage(Slot.Gold, BonusAllPoint);
                    Player.Exp += Slot.Exp + Slot.BonusEventExp;
                    Player.Gold += Slot.Gold + Slot.BonusEventPoint;
                    if (Dailies != null)
                    {
                        Dailies.ExpGained += Slot.Exp + Slot.BonusEventExp;
                        Dailies.PointGained += Slot.Gold + Slot.BonusEventPoint;
                        DailyQuery.AddQuery("exp_gained", Dailies.ExpGained);
                        DailyQuery.AddQuery("point_gained", Dailies.PointGained);
                    }
                    if (ConfigLoader.WinCashPerBattle)
                    {
                        Player.Cash += Slot.Cash;
                    }
                    RankModel Rank = PlayerRankXML.GetRank(Player.Rank);
                    if (Rank != null && Player.Exp >= Rank.OnNextLevel + Rank.OnAllExp && Player.Rank <= 50)
                    {
                        List<ItemsModel> Items = PlayerRankXML.GetRewards(Player.Rank);
                        if (Items.Count > 0)
                        {
                            foreach (ItemsModel Item in Items)
                            {
                                Player.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Item));
                            }
                            foreach (ItemsModel Item in GetCharasList(Items))
                            {
                                int Slots = Player.Character.Characters.Count;
                                CharacterModel Character = new CharacterModel()
                                {
                                    Id = Item.Id,
                                    Name = Item.Name,
                                    Slot = Slots++,
                                    CreateDate = uint.Parse(DateTimeUtil.Now("yyMMddHHmm")),
                                    PlayTime = 0
                                };
                                Player.Character.AddCharacter(Character);
                                if (DaoManagerSQL.CreatePlayerCharacter(Character, Player.PlayerId))
                                {
                                    Player.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0, 1, Character, Player));
                                }
                            }
                        }
                        Player.Gold += Rank.OnGoldUp;
                        Player.LastRankUpDate = uint.Parse(finishDate.ToString("yyMMddHHmm"));
                        Player.SendPacket(new PROTOCOL_BASE_RANK_UP_ACK(++Player.Rank, Rank.OnNextLevel));
                        AccountQuery.AddQuery("last_rank_update", (long)Player.LastRankUpDate);
                        AccountQuery.AddQuery("rank", Player.Rank);
                    }
                    if (evPt != null)
                    {
                        AllUtils.PlayTimeEvent((long)inBattleTime, Player, evPt, isBotMode);
                    }
                    AllUtils.DiscountPlayerItems(Slot, Player);
                    if (Exp != Player.Exp)
                    {
                        AccountQuery.AddQuery("experience", Player.Exp);
                    }
                    if (Gold != Player.Gold)
                    {
                        AccountQuery.AddQuery("gold", Player.Gold);
                    }
                    if (Cash != Player.Cash)
                    {
                        AccountQuery.AddQuery("cash", Player.Cash);
                    }
                    ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, AccountQuery.GetTables(), AccountQuery.GetValues());
                    ComDiv.UpdateDB("player_stat_basics", "owner_id", Player.PlayerId, TotalQuery.GetTables(), TotalQuery.GetValues());
                    ComDiv.UpdateDB("player_stat_seasons", "owner_id", Player.PlayerId, SeasonQuery.GetTables(), SeasonQuery.GetValues());
                    ComDiv.UpdateDB("player_stat_dailies", "owner_id", Player.PlayerId, DailyQuery.GetTables(), DailyQuery.GetValues());
                    ComDiv.UpdateDB("player_stat_weapons", "owner_id", Player.PlayerId, WeaponQuery.GetTables(), WeaponQuery.GetValues());
                    if (ConfigLoader.WinCashPerBattle && ConfigLoader.ShowCashReceiveWarn)
                    {
                        Player.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Translation.GetLabel("CashReceived", Slot.Cash)));
                    }
                }
            }
            UpdateSlotsInfo();
            if (RoomType != RoomCondition.FreeForAll)
            {
                CalculateClanMatchResult(winnerTeam);
            }
        }
        private List<ItemsModel> GetCharasList(List<ItemsModel> CharaList)
        {
            List<ItemsModel> Charas = new List<ItemsModel>();
            lock (CharaList)
            {
                foreach (ItemsModel Item in CharaList)
                {
                    if (Item != null && ComDiv.GetIdStatics(Item.Id, 1) == 6)
                    {
                        Charas.Add(Item);
                    }
                }
                return Charas;
            }
        }
        private void AddKDInfosToQuery(SlotModel Slot, PlayerStatistic Stat, DBQuery TotalQuery, DBQuery SeasonQuery)
        {
            if (Slot.AllKills > 0)
            {
                TotalQuery.AddQuery("kills_count", Stat.Basic.KillsCount);
                TotalQuery.AddQuery("total_kills", Stat.Basic.TotalKillsCount);
                SeasonQuery.AddQuery("kills_count", Stat.Season.KillsCount);
                SeasonQuery.AddQuery("total_kills", Stat.Season.TotalKillsCount);
            }
            if (Slot.AllAssists > 0)
            {
                TotalQuery.AddQuery("assists_count", Stat.Basic.AssistsCount);
                SeasonQuery.AddQuery("assists_count", Stat.Season.AssistsCount);
            }
            if (Slot.AllDeaths > 0)
            {
                TotalQuery.AddQuery("deaths_count", Stat.Basic.DeathsCount);
                SeasonQuery.AddQuery("deaths_count", Stat.Season.DeathsCount);
            }
            if (Slot.AllHeadshots > 0)
            {
                TotalQuery.AddQuery("headshots_count", Stat.Basic.HeadshotsCount);
                SeasonQuery.AddQuery("headshots_count", Stat.Season.HeadshotsCount);
            }
        }
        private void AddDailyToQuery(SlotModel Slot, PlayerStatistic Stat, DBQuery Query)
        {
            if (Slot.AllKills > 0)
            {
                Query.AddQuery("kills_count", Stat.Daily.KillsCount);
            }
            if (Slot.AllDeaths > 0)
            {
                Query.AddQuery("deaths_count", Stat.Daily.DeathsCount);
            }
            if (Slot.AllHeadshots > 0)
            {
                Query.AddQuery("headshots_count", Stat.Daily.HeadshotsCount);
            }
        }
        private void CalculateClanMatchResult(TeamEnum winnerTeam)
        {
            if (ChannelType != ChannelType.Clan || BlockedClan)
            {
                return;
            }
            SortedList<int, ClanModel> list = new SortedList<int, ClanModel>();
            foreach (SlotModel slot in Slots)
            {
                if (slot.State == SlotState.BATTLE && GetPlayerBySlot(slot, out Account p))
                {
                    ClanModel clan = ClanManager.GetClan(p.ClanId);
                    if (clan.Id == 0)
                    {
                        continue;
                    }
                    bool WonTheMatch = slot.Team == winnerTeam;
                    clan.Exp += slot.Exp;
                    clan.BestPlayers.SetBestExp(slot);
                    clan.BestPlayers.SetBestKills(slot);
                    clan.BestPlayers.SetBestHeadshot(slot);
                    clan.BestPlayers.SetBestWins(p.Statistic, slot, WonTheMatch);
                    clan.BestPlayers.SetBestParticipation(p.Statistic, slot);
                    if (!list.ContainsKey(p.ClanId))
                    {
                        list.Add(p.ClanId, clan);
                        if (winnerTeam != TeamEnum.TEAM_DRAW)
                        {
                            CalculateSpecialCM(clan, winnerTeam, slot.Team);
                            if (WonTheMatch)
                            {
                                clan.MatchWins++;
                            }
                            else
                            {
                                clan.MatchLoses++;
                            }
                        }
                        clan.Matches++;
                        DaoManagerSQL.UpdateClanBattles(clan.Id, clan.Matches, clan.MatchWins, clan.MatchLoses);
                    }
                }
            }
            foreach (ClanModel clan in list.Values)
            {
                DaoManagerSQL.UpdateClanExp(clan.Id, clan.Exp);
                DaoManagerSQL.UpdateClanPoints(clan.Id, clan.Points);
                DaoManagerSQL.UpdateClanBestPlayers(clan);
                RankModel RankModel = ClanRankXML.GetRank(clan.Rank);
                if (RankModel != null && clan.Exp >= RankModel.OnNextLevel + RankModel.OnAllExp)
                {
                    DaoManagerSQL.UpdateClanRank(clan.Id, ++clan.Rank);
                }
            }
        }
        private void CalculateSpecialCM(ClanModel clan, TeamEnum winnerTeam, TeamEnum teamIdx)
        {
            if (winnerTeam == TeamEnum.TEAM_DRAW)
            {
                return;
            }
            if (winnerTeam == (TeamEnum)teamIdx)
            {
                float morePoints;
                if (RoomType == RoomCondition.DeathMatch)
                {
                    morePoints = (teamIdx == 0 ? FRKills : CTKills) / 20;
                }
                else
                {
                    morePoints = teamIdx == 0 ? FRRounds : CTRounds;
                }
                float POINTS = 25 + morePoints;
                //Logger.warning("Clan: " + clan._id + " Earned Points: " + POINTS);
                clan.Points += POINTS;
                //Logger.warning("Clan: " + clan._id + " Final Points: " + clan._pontos);
            }
            else
            {
                if (clan.Points == 0)
                {
                    //Logger.warning("Clã não perdeu Pontos devido a baixa pontuação.");
                    return;
                }
                float morePoints;
                if (RoomType == RoomCondition.DeathMatch)
                {
                    morePoints = (teamIdx == 0 ? FRKills : CTKills) / 20;
                }
                else
                {
                    morePoints = teamIdx == 0 ? FRRounds : CTRounds;
                }
                float POINTS = 40 - morePoints;
                //Logger.warning("Clan: " + clan._id + " Losed Points: " + POINTS);
                clan.Points -= POINTS;
                //Logger.warning("Clan: " + clan._id + " Final Points: " + clan._pontos);
            }
        }
        public bool IsStartingMatch()
        {
            return State > RoomState.Ready;
        }
        public bool IsPreparing()
        {
            return State >= RoomState.Loading;
        }
        public void UpdateRoomInfo()
        {
            GenerateSeed();
            using (PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK packet = new PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(this))
            {
                SendPacketToPlayers(packet);
            }
        }
        public void SetSlotCount(int Count, bool Update)
        {
            MapMatch Match = SystemMapXML.GetMapLimit((int)MapId, (int)Rule);
            if (Match != null)
            {
                Count = Match.Limit;
            }
            if (RoomType == RoomCondition.Tutorial)
            {
                Count = 1;
            }
            if (IsBotMode())
            {
                Count = 8;
            }
            if (Count <= 0)
            {
                Count = 1;
            }
            for (int i = 0; i < Slots.Length; i++)
            {
                if (i >= Count)
                {
                    Slots[i].State = SlotState.CLOSE;
                }
            }
            if (Update)
            {
                UpdateSlotsInfo();
            }
        }
        public int GetSlotCount()
        {
            lock (Slots)
            {
                int count = 0;
                foreach (SlotModel Slot in Slots)
                {
                    if (Slot.State != SlotState.CLOSE)
                    {
                        ++count;
                    }
                }
                return count;
            }
        }
        public void SwitchNewSlot(List<SlotModel> SlotChanges, Account Player, SlotModel OldSlot, TeamEnum TeamIdx, int SlotIdx)
        {
            if (TeamIdx != TeamEnum.ALL_TEAM && GetSlot(SlotIdx, out SlotModel NewSlot))
            {
                if (NewSlot.PlayerId == 0 && NewSlot.State == SlotState.EMPTY)
                {
                    NewSlot.State = SlotState.NORMAL;
                    NewSlot.PlayerId = Player.PlayerId;
                    NewSlot.Equipment = Player.Equipment;
                    OldSlot.State = SlotState.EMPTY;
                    OldSlot.PlayerId = 0;
                    OldSlot.Equipment = null;
                    if (Player.SlotId == Leader)
                    {
                        LeaderName = Player.Nickname;
                        Leader = SlotIdx;
                    }
                    //Logger.LogProblems("[Room.SwitchNewSlot] PlayerId '" + p.player_id + "' '" + p.player_name + "'; OldSlot: " + p._slotId + "; NewSlot: " + index, "ErrorC");
                    Player.SlotId = SlotIdx;
                    SlotChanges.Add(new SlotModel(NewSlot, OldSlot));
                }
            }
        }
        public void SwitchSlots(List<SlotModel> SlotChanges, int NewSlotId, int OldSlotId, bool ChangeReady)
        {
            SlotModel NewSlot = Slots[NewSlotId], OldSlot = Slots[OldSlotId];
            if (ChangeReady)
            {
                if (NewSlot.State == SlotState.READY)
                {
                    NewSlot.State = SlotState.NORMAL;
                }
                if (OldSlot.State == SlotState.READY)
                {
                    OldSlot.State = SlotState.NORMAL;
                }
            }
            NewSlot.SetSlotId(OldSlotId);
            OldSlot.SetSlotId(NewSlotId);
            Slots[NewSlotId] = OldSlot;
            Slots[OldSlotId] = NewSlot;
            SlotChanges.Add(new SlotModel(OldSlot, NewSlot));
        }
        public void ChangeSlotState(int slotId, SlotState state, bool sendInfo)
        {
            SlotModel slot = GetSlot(slotId);
            ChangeSlotState(slot, state, sendInfo);
        }
        public void ChangeSlotState(SlotModel slot, SlotState state, bool sendInfo)
        {
            if (slot == null || slot.State == state)
            {
                return;
            }
            slot.State = state;
            if (state == SlotState.EMPTY || state == SlotState.CLOSE)
            {
                AllUtils.ResetSlotInfo(this, slot, false);
                slot.PlayerId = 0;
            }
            if (sendInfo)
            {
                UpdateSlotsInfo();
            }
        }
        public Account GetPlayerBySlot(SlotModel Slot)
        {
            try
            {
                long PlayerId = Slot.PlayerId;
                return PlayerId > 0 ? AccountManager.GetAccount(PlayerId, true) : null;
            }
            catch
            {
                return null;
            }
        }
        public Account GetPlayerBySlot(int SlotId)
        {
            try
            {
                long PlayerId = Slots[SlotId].PlayerId;
                return PlayerId > 0 ? AccountManager.GetAccount(PlayerId, true) : null;
            }
            catch
            {
                return null;
            }
        }
        public bool GetPlayerBySlot(int SlotId, out Account Player)
        {
            try
            {
                long PlayerId = Slots[SlotId].PlayerId;
                Player = PlayerId > 0 ? AccountManager.GetAccount(PlayerId, true) : null;
                return Player != null;
            }
            catch
            {
                Player = null;
                return false;
            }
        }
        public bool GetPlayerBySlot(SlotModel Slot, out Account Player)
        {
            try
            {
                long PlayerId = Slot.PlayerId;
                Player = PlayerId > 0 ? AccountManager.GetAccount(PlayerId, true) : null;
                return Player != null;
            }
            catch
            {
                Player = null;
                return false;
            }
        }
        public int GetTimeByMask()
        {
            return TIMES[KillTime >> 4];
        }
        public int GetRoundsByMask()
        {
            return ROUNDS[KillTime & 15];
        }
        public int GetKillsByMask()
        {
            return KILLS[KillTime & 15];
        }
        public void UpdateSlotsInfo()
        {
            using (PROTOCOL_ROOM_GET_SLOTINFO_ACK Packet = new PROTOCOL_ROOM_GET_SLOTINFO_ACK(this))
            {
                SendPacketToPlayers(Packet);
            }
        }
        public bool GetLeader(out Account Player)
        {
            Player = null;
            if (GetCountPlayers() <= 0)
            {
                return false;
            }
            if (Leader == -1)
            {
                SetNewLeader(-1, SlotState.EMPTY, -1, false);
            }
            if (Leader >= 0)
            {
                Player = AccountManager.GetAccount(Slots[Leader].PlayerId, true);
            }
            return Player != null;
        }
        public Account GetLeader()
        {
            if (GetCountPlayers() <= 0)
            {
                return null;
            }
            if (Leader == -1)
            {
                SetNewLeader(-1, SlotState.EMPTY, -1, false);
            }
            return Leader == -1 ? null : AccountManager.GetAccount(Slots[Leader].PlayerId, true);
        }
        public void SetNewLeader(int Leader, SlotState State, int OldLeader, bool UpdateInfo)
        {
            lock (Slots)
            {
                if (Leader == -1)
                {
                    for (int i = 0; i < 16; ++i)
                    {
                        SlotModel Slot = Slots[i];
                        if (i != OldLeader && Slot.PlayerId > 0 && Slot.State > State)
                        {
                            this.Leader = i;
                            break;
                        }
                    }
                }
                else
                {
                    this.Leader = Leader;
                }
                if (this.Leader != -1)
                {
                    SlotModel slot = Slots[this.Leader];
                    if (slot.State == SlotState.READY)
                    {
                        slot.State = SlotState.NORMAL;
                    }
                    if (UpdateInfo)
                    {
                        UpdateSlotsInfo();
                    }
                }
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet)
        {
            List<Account> Players = GetAllPlayers();
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket)");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet, long PlayerId)
        {
            List<Account> Players = GetAllPlayers(PlayerId);
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,long)");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet, SlotState State, int Type)
        {
            List<Account> Players = GetAllPlayers(State, Type);
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int)");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet, GameServerPacket Packet2, SlotState State, int Type)
        {
            List<Account> Players = GetAllPlayers(State, Type);
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-1");
            byte[] Data2 = Packet2.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SendPacket,SLOT_STATE,int)-2");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
                Player.SendCompletePacket(Data2, Packet2.GetType().Name);
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet, SlotState State, int Type, int Exception)
        {
            List<Account> Players = GetAllPlayers(State, Type, Exception);
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int)");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public void SendPacketToPlayers(GameServerPacket Packet, SlotState State, int Type, int Exception, int Exception2)
        {
            List<Account> Players = GetAllPlayers(State, Type, Exception, Exception2);
            if (Players.Count == 0)
            {
                return;
            }
            byte[] Data = Packet.GetCompleteBytes("Room.SendPacketToPlayers(SendPacket,SLOT_STATE,int,int,int)");
            foreach (Account Player in Players)
            {
                Player.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
        public void RemovePlayer(Account player, bool WarnAllPlayers, int quitMotive = 0)
        {
            if (player == null || !GetSlot(player.SlotId, out SlotModel slot))
            {
                return;
            }
            BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
        }
        public void RemovePlayer(Account player, SlotModel slot, bool WarnAllPlayers, int quitMotive = 0)
        {
            if (player == null || slot == null)
            {
                return;
            }
            BaseRemovePlayer(player, slot, WarnAllPlayers, quitMotive);
        }
        private void BaseRemovePlayer(Account player, SlotModel slot, bool WarnAllPlayers, int quitMotive)
        {
            lock (Slots)
            {
                bool useRoomUpdate = false, hostChanged = false;
                if (player != null && slot != null)
                {
                    if (slot.State >= SlotState.LOAD)
                    {
                        if (Leader == slot.Id)
                        {
                            int oldLeader = Leader;
                            SlotState bestState = SlotState.CLOSE;
                            if (State == RoomState.Battle)
                            {
                                bestState = SlotState.BATTLE_READY;
                            }
                            else if (State >= RoomState.Loading)
                            {
                                bestState = SlotState.READY;
                            }
                            if (GetAllPlayers(slot.Id).Count >= 1)
                            {
                                SetNewLeader(-1, bestState, oldLeader, false);
                            }
                            if (GetPlayingPlayers(TeamEnum.TEAM_DRAW, SlotState.READY, 1) >= 2)
                            {
                                using (PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK packet = new PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK(this))
                                {
                                    SendPacketToPlayers(packet, SlotState.RENDEZVOUS, 1, slot.Id);
                                }
                            }
                            hostChanged = true;
                        }
                        using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(player, quitMotive))
                        {
                            SendPacketToPlayers(packet, SlotState.READY, 1, !WarnAllPlayers ? slot.Id : -1);
                        }
                        BattleLeaveSync.SendUDPPlayerLeave(this, slot.Id);
                        slot.ResetSlot();
                        if (votekick != null)
                        {
                            votekick.TotalArray[slot.Id] = false;
                        }
                    }
                    slot.PlayerId = 0;
                    slot.Equipment = null;
                    slot.State = SlotState.EMPTY;
                    if (State == RoomState.CountDown)
                    {
                        if (slot.Id == Leader)
                        {
                            State = RoomState.Ready;
                            useRoomUpdate = true;
                            CountdownTime.Timer = null;
                            using (PROTOCOL_BATTLE_START_COUNTDOWN_ACK packet = new PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum.StopByHost))
                            {
                                SendPacketToPlayers(packet);
                            }
                        }
                        else if (GetPlayingPlayers(slot.Team, SlotState.READY, 0) == 0)
                        {
                            if (slot.Id != Leader)
                            {
                                ChangeSlotState(Leader, SlotState.NORMAL, false);
                            }
                            StopCountDown(CountDownEnum.StopByPlayer, false);
                            useRoomUpdate = true;
                        }
                    }
                    else if (IsPreparing())
                    {
                        AllUtils.BattleEndPlayersCount(this, IsBotMode());
                        if (State == RoomState.Battle)
                        {
                            AllUtils.BattleEndRoundPlayersCount(this);
                        }
                    }
                    CheckToEndWaitingBattle(hostChanged);
                    RequestHost.Remove(player.PlayerId);
                    if (VoteTime.Timer != null && votekick != null && votekick.VictimIdx == player.SlotId && quitMotive != 2)
                    {
                        VoteTime.Timer = null;
                        votekick = null;
                        using (PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK packet = new PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK())
                        {
                            SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                        }
                    }
                    MatchModel match = player.Match;
                    if (match != null && player.MatchSlot >= 0)
                    {
                        match.Slots[player.MatchSlot].State = SlotMatchState.Normal;
                        using (PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK packet = new PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(match))
                        {
                            match.SendPacketToPlayers(packet);
                        }
                    }
                    player.Room = null;
                    //Logger.LogProblems("[Room.RemovePlayer] PlayerId '" + player.player_id + "' '" + player.player_name + "'; OldSlot: " + player._slotId + "; NewSlot: -1", "ErrorC");
                    player.SlotId = -1;
                    player.Status.UpdateRoom(255);
                    AllUtils.SyncPlayerToClanMembers(player);
                    AllUtils.SyncPlayerToFriends(player, false);
                    player.UpdateCacheInfo();
                }
                UpdateSlotsInfo();
                if (useRoomUpdate)
                {
                    UpdateRoomInfo();
                }
            }
        }
        public int AddPlayer(Account Player)
        {
            lock (Slots)
            {
                for (int i = 0; i < 16; i++)
                {
                    SlotModel slot = Slots[i];
                    if (slot.PlayerId == 0 && slot.State == SlotState.EMPTY)
                    {
                        slot.PlayerId = Player.PlayerId;
                        slot.State = SlotState.NORMAL;
                        Player.Room = this;
                        //Logger.LogProblems("[Room.AddPlayer] PlayerId '" + p.player_id + "' '" + p.player_name + "'; OldSlot: " + p._slotId + "; NewSlot: " + i, "ErrorC");
                        Player.SlotId = i;
                        slot.Equipment = Player.Equipment;
                        Player.Status.UpdateRoom((byte)RoomId);
                        AllUtils.SyncPlayerToClanMembers(Player);
                        AllUtils.SyncPlayerToFriends(Player, false);
                        Player.UpdateCacheInfo();
                        return i;
                    }
                }
            }
            return -1;
        }
        public int AddPlayer(Account Player, TeamEnum TeamIdx)
        {
            lock (Slots)
            {
                foreach (int SlotIdx in GetTeamArray(TeamIdx))
                {
                    SlotModel Slot = Slots[SlotIdx];
                    if (Slot.PlayerId == 0 && Slot.State == SlotState.EMPTY)
                    {
                        Slot.PlayerId = Player.PlayerId;
                        Slot.State = SlotState.NORMAL;
                        Player.Room = this;
                        //Logger.LogProblems("[Room.AddPlayer] PlayerId '" + p.player_id + "' '" + p.player_name + "'; OldSlot: " + p._slotId + "; NewSlot: " + i, "ErrorC");
                        Player.SlotId = SlotIdx;
                        Slot.Equipment = Player.Equipment;
                        Player.Status.UpdateRoom((byte)RoomId);
                        AllUtils.SyncPlayerToClanMembers(Player);
                        AllUtils.SyncPlayerToFriends(Player, false);
                        Player.UpdateCacheInfo();
                        return SlotIdx;
                    }
                }
            }
            return -1;
        }
        public int[] GetTeamArray(TeamEnum Team)
        {
            return Team == TeamEnum.FR_TEAM ? FR_TEAM : Team == TeamEnum.CT_TEAM ? CT_TEAM : ALL_TEAM;
        }
        public List<Account> GetAllPlayers(SlotState state, int type)
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                for (int i = 0; i < Slots.Length; ++i)
                {
                    SlotModel slot = Slots[i];
                    if (slot.PlayerId > 0 && (type == 0 && slot.State == state || type == 1 && slot.State > state))
                    {
                        Account player = AccountManager.GetAccount(slot.PlayerId, true);
                        if (player != null && player.SlotId != -1)
                        {
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }
        public List<Account> GetAllPlayers(SlotState state, int type, int exception)
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                for (int i = 0; i < Slots.Length; ++i)
                {
                    SlotModel slot = Slots[i];
                    if (slot.PlayerId > 0 && i != exception && (type == 0 && slot.State == state || type == 1 && slot.State > state))
                    {
                        Account player = AccountManager.GetAccount(slot.PlayerId, true);
                        if (player != null && player.SlotId != -1)
                        {
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }
        public List<Account> GetAllPlayers(SlotState state, int type, int exception, int exception2)
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                for (int i = 0; i < Slots.Length; ++i)
                {
                    SlotModel slot = Slots[i];
                    if (slot.PlayerId > 0 && i != exception && i != exception2 && (type == 0 && slot.State == state || type == 1 && slot.State > state))
                    {
                        Account player = AccountManager.GetAccount(slot.PlayerId, true);
                        if (player != null && player.SlotId != -1)
                        {
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }
        public List<Account> GetAllPlayers(int exception)
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                for (int i = 0; i < Slots.Length; ++i)
                {
                    long id = Slots[i].PlayerId;
                    if (id > 0 && i != exception)
                    {
                        Account player = AccountManager.GetAccount(id, true);
                        if (player != null && player.SlotId != -1)
                        {
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }
        public List<Account> GetAllPlayers(long exception)
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                foreach (SlotModel Slot in Slots)
                {
                    if (Slot.PlayerId > 0 && Slot.PlayerId != exception)
                    {
                        Account player = AccountManager.GetAccount(Slot.PlayerId, true);
                        if (player != null && player.SlotId != -1)
                        {
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }
        public List<Account> GetAllPlayers()
        {
            List<Account> list = new List<Account>();
            lock (Slots)
            {
                foreach (SlotModel Slot in Slots)
                {
                    if (Slot.PlayerId > 0)
                    {
                        Account player = AccountManager.GetAccount(Slot.PlayerId, true);
                        if (player != null && player.SlotId != -1)
                        {
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }
        public int GetCountPlayers()
        {
            int Count = 0;
            lock (Slots)
            {
                foreach (SlotModel Slot in Slots)
                {
                    if (Slot.PlayerId > 0)
                    {
                        Account player = AccountManager.GetAccount(Slot.PlayerId, true);
                        if (player != null && player.SlotId != -1)
                        {
                            ++Count;
                        }
                    }
                }
            }
            return Count;
        }
        public int GetPlayingPlayers(TeamEnum team, bool inBattle)
        {
            int players = 0;
            lock (Slots)
            {
                foreach (SlotModel slot in Slots)
                {
                    if (slot.PlayerId > 0 && (slot.Team == team || team == TeamEnum.TEAM_DRAW) && (inBattle && slot.State == SlotState.BATTLE_LOAD && !slot.Spectator || !inBattle && slot.State >= SlotState.LOAD))
                    {
                        players++;
                    }
                }
            }
            return players;
        }
        public int GetPlayingPlayers(TeamEnum team, SlotState state, int type)
        {
            int players = 0;
            lock (Slots)
            {
                foreach (SlotModel slot in Slots)
                {
                    if (slot.PlayerId > 0 && (type == 0 && slot.State == state || type == 1 && slot.State > state) && (team == TeamEnum.TEAM_DRAW || slot.Team == team))
                    {
                        players++;
                    }
                }
            }
            return players;
        }
        public int GetPlayingPlayers(TeamEnum team, SlotState state, int type, int exception)
        {
            int players = 0;
            lock (Slots)
            {
                for (int i = 0; i < 16; i++)
                {
                    SlotModel slot = Slots[i];
                    if (i != exception && slot.PlayerId > 0 && (type == 0 && slot.State == state || type == 1 && slot.State > state) && (team == TeamEnum.TEAM_DRAW || slot.Team == team))
                    {
                        players++;
                    }
                }
            }
            return players;
        }
        public void GetPlayingPlayers(bool inBattle, out int RedPlayers, out int BluePlayers)
        {
            RedPlayers = 0;
            BluePlayers = 0;
            lock (Slots)
            {
                foreach (SlotModel slot in Slots)
                {
                    if (slot.PlayerId > 0 && (inBattle && slot.State == SlotState.BATTLE && !slot.Spectator || !inBattle && slot.State >= SlotState.RENDEZVOUS))
                    {
                        if (slot.Team == 0)
                        {
                            RedPlayers++;
                        }
                        else
                        {
                            BluePlayers++;
                        }
                    }
                }
            }
        }
        public void GetPlayingPlayers(bool inBattle, out int RedPlayers, out int BluePlayers, out int RedDeaths, out int BlueDeaths)
        {
            RedPlayers = 0;
            BluePlayers = 0;
            RedDeaths = 0;
            BlueDeaths = 0;
            lock (Slots)
            {
                foreach (SlotModel slot in Slots)
                {
                    if (slot.DeathState.HasFlag(DeadEnum.Dead))
                    {
                        if (slot.Team == 0)
                        {
                            RedDeaths++;
                        }
                        else
                        {
                            BlueDeaths++;
                        }
                    }
                    if (slot.PlayerId > 0 && (inBattle && slot.State == SlotState.BATTLE && !slot.Spectator || !inBattle && slot.State >= SlotState.RENDEZVOUS))
                    {
                        if (slot.Team == 0)
                        {
                            RedPlayers++;
                        }
                        else
                        {
                            BluePlayers++;
                        }
                    }
                }
            }
        }
        public void CheckToEndWaitingBattle(bool host)
        {
            //Logger.warning("state: " + _roomState + "; leader: " + _slots[_leader].state);
            if ((State == RoomState.CountDown || State == RoomState.Loading || State == RoomState.Rendezvous) && (host || Slots[Leader].State == SlotState.BATTLE_READY))
            {
                AllUtils.EndBattleNoPoints(this);
            }
        }
        public void SpawnReadyPlayers()
        {
            lock (Slots)
            {
                BaseSpawnReadyPlayers(IsBotMode());
            }
        }
        public void SpawnReadyPlayers(bool isBotMode)
        {
            lock (Slots)
            {
                BaseSpawnReadyPlayers(isBotMode);
            }
        }
        private void BaseSpawnReadyPlayers(bool IsBotMode)
        {
            bool IsValidCD = ThisModeHaveRounds() && (CountdownIG == 3 || CountdownIG == 5 || CountdownIG == 7 || CountdownIG == 9);
            using (PROTOCOL_BATTLE_COUNT_DOWN_ACK CownDownPacket = new PROTOCOL_BATTLE_COUNT_DOWN_ACK(CountdownIG))
            {
                if (State == RoomState.PreBattle && PreMatchCD == false && IsValidCD && IsBotMode == false)
                {
                    PreMatchCD = true;
                    SendPacketToPlayers(CownDownPacket);
                }
            }
            int CountDownTime = CountdownIG == 0 ? 0 : (CountdownIG * 1000) + 250;
            PreMatchTime.Start(CountDownTime, (callbackState) =>
            {
                try
                {
                    DateTime Date = DateTimeUtil.Now();
                    foreach (SlotModel Slot in Slots)
                    {
                        if (Slot.State == SlotState.BATTLE_READY && Slot.IsPlaying == 0 && Slot.PlayerId > 0)
                        {
                            Slot.IsPlaying = 1;
                            Slot.StartTime = Date;
                            Slot.State = SlotState.BATTLE;
                            if (State == RoomState.Battle && (RoomType == RoomCondition.Bomb || RoomType == RoomCondition.Annihilation || RoomType == RoomCondition.Convoy || RoomType == RoomCondition.Ace))
                            {
                                Slot.Spectator = true;
                            }
                        }
                    }
                    UpdateSlotsInfo();
                    List<int> Dinos = AllUtils.GetDinossaurs(this, false, -1);
                    if (State == RoomState.PreBattle)
                    {
                        BattleStart = IsDinoMode() ? Date.AddMinutes(5) : Date;
                        SetSpecialStage();
                    }
                    bool dinoStart = false;
                    using (PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK Packet = new PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(this, Dinos, IsBotMode))
                    {
                        using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK Packet2 = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(this))
                        {
                            using (PROTOCOL_BATTLE_RECORD_ACK Packet3 = new PROTOCOL_BATTLE_RECORD_ACK(this))
                            {
                                byte[] Data = Packet.GetCompleteBytes("Room.BaseSpawnReadyPlayers-1");
                                byte[] Data2 = Packet2.GetCompleteBytes("Room.BaseSpawnReadyPlayers-2");
                                byte[] Data3 = Packet3.GetCompleteBytes("Room.BaseSpawnReadyPlayers-3");
                                foreach (SlotModel Slot in Slots)
                                {
                                    if (Slot.State == SlotState.BATTLE && Slot.IsPlaying == 1 && GetPlayerBySlot(Slot, out Account Player))
                                    {
                                        Slot.IsPlaying = 2;
                                        if (State == RoomState.PreBattle)
                                        {
                                            using (PROTOCOL_BATTLE_STARTBATTLE_ACK Packet4 = new PROTOCOL_BATTLE_STARTBATTLE_ACK(Slot, Player, Dinos, IsBotMode, true))
                                            {
                                                SendPacketToPlayers(Packet4, SlotState.READY, 1);
                                            }
                                            Player.SendCompletePacket(Data, Packet.GetType().Name);
                                            if (IsDinoMode())
                                            {
                                                dinoStart = true;
                                            }
                                            else
                                            {
                                                Player.SendCompletePacket(Data2, Packet2.GetType().Name);
                                            }
                                        }
                                        else if (State == RoomState.Battle)
                                        {
                                            using (PROTOCOL_BATTLE_STARTBATTLE_ACK Packet4 = new PROTOCOL_BATTLE_STARTBATTLE_ACK(Slot, Player, Dinos, IsBotMode, false))
                                            {
                                                SendPacketToPlayers(Packet4, SlotState.READY, 1);
                                            }
                                            if (RoomType == RoomCondition.Bomb || RoomType == RoomCondition.Annihilation || RoomType == RoomCondition.Convoy)
                                            {
                                                EquipmentSync.SendUDPPlayerSync(this, Slot, 0, 1);
                                            }
                                            else
                                            {
                                                Player.SendCompletePacket(Data, Packet.GetType().Name);
                                            }
                                            Player.SendCompletePacket(Data2, Packet2.GetType().Name);
                                            Player.SendCompletePacket(Data3, Packet3.GetType().Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (State == RoomState.PreBattle)
                    {
                        State = RoomState.Battle;
                        UpdateRoomInfo();
                    }
                    if (dinoStart)
                    {
                        StartDinoRound();
                    }
                }
                catch (Exception ex)
                {
                    CLogger.Print($"Room.StartCountDown: {ex.Message}", LoggerType.Error, ex);
                }
                lock (callbackState)
                {
                    PreMatchTime.Timer = null;
                }
            });
        }
        public void StartDinoRound()
        {
            RoundTime.Start(5250, (callbackState) =>
            {
                if (State == RoomState.Battle)
                {
                    BattleStart = DateTimeUtil.Now().AddSeconds(5);
                    using (PROTOCOL_BATTLE_MISSION_ROUND_START_ACK packet = new PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(this))
                    {
                        SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                    }
                    SwapRound = false;
                }
                lock (callbackState)
                {
                    RoundTime.Timer = null;
                }
            });
        }
        public bool IsDinoMode(string Dino = "")
        {
            if (Dino.Equals("DE"))
            {
                return (RoomType == RoomCondition.Boss);
            }
            else if (Dino.Equals("CC"))
            {
                return (RoomType == RoomCondition.CrossCounter);
            }
            return (RoomType == RoomCondition.Boss || RoomType == RoomCondition.CrossCounter);
        }
        public int GetReadyPlayers()
        {
            int CountPlayers = 0;
            for (int i = 0; i < Slots.Length; i++)
            {
                SlotModel Slot = Slots[i];
                if (Slot != null && Slot.State >= SlotState.READY && Slot.Equipment != null)
                {
                    Account Player = GetPlayerBySlot(Slot);
                    if (Player != null && Player.SlotId == i)
                    {
                        CountPlayers++;
                    }
                }
            }
            return CountPlayers;
        }
        public int ResetReadyPlayers()
        {
            int UpdatedSlots = 0;
            foreach (SlotModel Slot in Slots)
            {
                if (Slot.State == SlotState.READY)
                {
                    Slot.State = SlotState.NORMAL;
                    UpdatedSlots++;
                }
            }
            return UpdatedSlots;
        }
    }
}
