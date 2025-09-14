using Plugin.Core.Enums;
using Plugin.Core.Utility;
using System;
using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class SlotModel
    {
        public long PlayerId;
        public TeamEnum Team;
        public SlotState State;
        public ResultIcon BonusFlags;
        public PlayerEquipment Equipment;
        public DeadEnum DeathState = DeadEnum.Alive;
        public bool FirstRespawn = true;
        public bool RepeatLastState;
        public bool Check;
        public bool Spectator;
        public bool SpecGM;
        public bool WithHost;
        public int Id;
        public int Flag;
        public int AiLevel;
        public int Latency;
        public int FailLatencyTimes;
        public int Ping;
        public int PassSequence;
        public int IsPlaying;
        public int EarnedEXP;
        public int SpawnsCount;
        public int AllHeadshots;
        public int LastKillState;
        public int KillsOnLife;
        public int Exp;
        public int Cash;
        public int Gold;
        public int Score;
        public int AllKills;
        public int AllDeaths;
        public int AllAssists;
        public int Objects;
        public int BonusItemExp;
        public int BonusItemPoint;
        public int BonusEventExp;
        public int BonusEventPoint;
        public int BonusCafePoint;
        public int BonusCafeExp;
        public int UnkItem;
        public int Costume;
        public DateTime NextVoteDate;
        public DateTime StartTime;
        public DateTime PreStartDate;
        public DateTime PreLoadDate;
        public ushort DamageBar1;
        public ushort DamageBar2;
        public bool MissionsCompleted;
        public PlayerMissions Missions;
        public SlotModel NewSlot;
        public SlotModel OldSlot;
        public int[] AR = new int[2] { 0, 0 };
        public int[] SMG = new int[2] { 0, 0 };
        public int[] SR = new int[2] { 0, 0 };
        public int[] MG = new int[2] { 0, 0 };
        public int[] SG = new int[2] { 0, 0 };
        public List<int> ItemUsages = new List<int>();
        public TimerState Timing = new TimerState();
        public SlotModel(int slotIdx)
        {
            SetSlotId(slotIdx);
        }
        public SlotModel(SlotModel NewSlot, SlotModel OldSlot)
        {
            this.NewSlot = ObjectCopier.Copy(NewSlot);
            this.OldSlot = ObjectCopier.Copy(OldSlot);
        }
        public void StopTiming()
        {
            if (Timing != null)
            {
                Timing.Timer = null;
            }
        }
        public void SetSlotId(int SlotIdx)
        {
            Id = SlotIdx;
            Team = (TeamEnum)(SlotIdx % 2);
            Flag = 1 << SlotIdx;
        }
        public void ResetSlot()
        {
            RepeatLastState = false;
            DeathState = DeadEnum.Alive;
            StopTiming();
            Check = false;
            Spectator = false;
            SpecGM = false;
            WithHost = false;
            FirstRespawn = true;
            FailLatencyTimes = 0;
            Latency = 0;
            Ping = 5;
            PassSequence = 0;
            AllDeaths = 0;
            AllKills = 0;
            AllAssists = 0;
            BonusFlags = 0;
            KillsOnLife = 0;
            LastKillState = 0;
            Score = 0;
            Gold = 0;
            Exp = 0;
            AllHeadshots = 0;
            Objects = 0;
            BonusItemExp = 0;
            BonusItemPoint = 0;
            BonusCafeExp = 0;
            BonusCafePoint = 0;
            BonusEventExp = 0;
            BonusEventPoint = 0;
            SpawnsCount = 0;
            DamageBar1 = 0;
            DamageBar2 = 0;
            EarnedEXP = 0;
            IsPlaying = 0;
            Cash = 0;
            NextVoteDate = new DateTime();
            AiLevel = 0;
            ItemUsages.Clear();
            MissionsCompleted = false;
            Missions = null;
            Array.Clear(AR, 0, AR.Length);
            Array.Clear(SMG, 0, SMG.Length);
            Array.Clear(SR, 0, SR.Length);
            Array.Clear(SG, 0, SG.Length);
            Array.Clear(MG, 0, MG.Length);
        }
        public void SetMissionsClone(PlayerMissions Missions)
        {
            this.Missions = ObjectCopier.Copy(Missions);
            MissionsCompleted = false;
        }
        public double InBattleTime(DateTime date)
        {
            if (StartTime == new DateTime())// || startTime > date)
            {
                return 0;
            }
            return (date - StartTime).TotalSeconds;
        }
    }
}