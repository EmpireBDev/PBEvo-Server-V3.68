using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Utils;
using Plugin.Core.Utility;
using Server.Game.Data.XML;
using Server.Game.Data.Models;
using Plugin.Core.Models;
using Server.Game.Data.Sync.Update;

namespace Server.Game.Data.Sync.Client
{
    public class RoomDeath
    {
        public static void Load(SyncClientPacket C)
        {
            int RoomId = C.ReadH();
            int ChannelId = C.ReadH();
            int ServerId = C.ReadH();
            byte KillsCount = C.ReadC();
            byte KillerId = C.ReadC();
            int WeaponId = C.ReadD();
            float KillerX = C.ReadT();
            float KillerY = C.ReadT();
            float KillerZ = C.ReadT();
            byte DieObjectId = C.ReadC();
            byte Unknown = C.ReadC();
            int Length = (KillsCount * 23);
            if (C.ToArray().Length > (28 + Length))
            {
                CLogger.Print($"Invalid Death (Length > 51): {C.ToArray().Length}", LoggerType.Warning);
            }
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return;
            }
            RoomModel Room = Channel.GetRoom(RoomId);
            if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle)
            {
                SlotModel Killer = Room.GetSlot(KillerId);
                if (Killer != null && Killer.State == SlotState.BATTLE)
                {
                    FragInfos Info = new FragInfos()
                    {
                        KillsCount = KillsCount,
                        KillerIdx = KillerId,
                        Weapon = WeaponId,
                        X = KillerX,
                        Y = KillerY,
                        Z = KillerZ,
                        Flag = DieObjectId,
                        Unk = Unknown
                    };
                    bool IsSuicide = false;
                    for (int i = 0; i < KillsCount; i++)
                    {
                        byte WeaponClass = C.ReadC();
                        byte DeathInfo = C.ReadC();
                        float VicX = C.ReadT();
                        float VicY = C.ReadT();
                        float VicZ = C.ReadT();
                        byte AssistId = C.ReadC();
                        byte[] Unk = C.ReadB(8);
                        byte VictimId = (byte)(DeathInfo & 15);
                        SlotModel Victim = Room.GetSlot(VictimId);
                        if (Victim != null && Victim.State == SlotState.BATTLE)
                        {
                            FragModel Frag = new FragModel(DeathInfo)
                            {
                                VictimWeaponClass = WeaponClass,
                                X = VicX,
                                Y = VicY,
                                Z = VicZ,
                                AssistSlot = AssistId,
                                Unk = Unk
                            };
                            if (Info.KillerIdx == VictimId)
                            {
                                IsSuicide = true;
                            }
                            Info.Frags.Add(Frag);
                        }
                    }
                    Info.KillsCount = (byte)Info.Frags.Count;
                    KillFragInfo.GenDeath(Room, Killer, Info, IsSuicide);
                }
            }
        }
        public static void RegistryFragInfos(RoomModel Room, SlotModel Killer, out int Score, bool IsBotMode, bool IsSuicide, FragInfos Kills)
        {
            Score = 0;
            ItemClass WeaponSlot = (ItemClass)ComDiv.GetIdStatics(Kills.Weapon, 1);
            ClassType WeaponClass = (ClassType)ComDiv.GetIdStatics(Kills.Weapon, 2);
            foreach (FragModel Frag in Kills.Frags)
            {
                CharaDeath DeathType = (CharaDeath)(Frag.HitspotInfo >> 4);
                if (Kills.KillsCount - (IsSuicide ? 1 : 0) > 1)
                {
                    Frag.KillFlag |= DeathType == CharaDeath.BOOM || DeathType == CharaDeath.OBJECT_EXPLOSION || DeathType == CharaDeath.POISON || DeathType == CharaDeath.HOWL || DeathType == CharaDeath.TRAMPLED || WeaponClass == ClassType.Shotgun ? KillingMessage.MassKill : KillingMessage.PiercingShot;
                }
                else
                {
                    int Value = 0;
                    if (DeathType == CharaDeath.HEADSHOT)
                    {
                        Value = 4;
                    }
                    else if (DeathType == CharaDeath.DEFAULT && WeaponSlot == ItemClass.Melee)
                    {
                        Value = 6;
                    }
                    if (Value > 0)
                    {
                        int LastState = Killer.LastKillState >> 12;
                        if (Value == 4)
                        {
                            if (LastState != 4)
                            {
                                Killer.RepeatLastState = false;
                            }
                            Killer.LastKillState = Value << 12 | (Killer.KillsOnLife + 1);
                            if (Killer.RepeatLastState)
                            {
                                Frag.KillFlag |= (Killer.LastKillState & 16383) <= 1 ? KillingMessage.Headshot : KillingMessage.ChainHeadshot;
                            }
                            else
                            {
                                Frag.KillFlag |= KillingMessage.Headshot;
                                Killer.RepeatLastState = true;
                            }
                        }
                        else if (Value == 6)
                        {
                            if (LastState != 6)
                            {
                                Killer.RepeatLastState = false;
                            }
                            Killer.LastKillState = Value << 12 | (Killer.KillsOnLife + 1);
                            if (Killer.RepeatLastState && (Killer.LastKillState & 16383) > 1)
                            {
                                Frag.KillFlag |= KillingMessage.ChainSlugger;
                            }
                            else
                            {
                                Killer.RepeatLastState = true;
                            }
                        }
                    }
                    else
                    {
                        Killer.LastKillState = 0;
                        Killer.RepeatLastState = false;
                    }
                }
                byte VictimId = Frag.VictimSlot, AssistId = Frag.AssistSlot;
                SlotModel VictimSlot = Room.Slots[VictimId], AssistSlot = Room.Slots[AssistId];
                if (VictimSlot.KillsOnLife > 3)
                {
                    Frag.KillFlag |= KillingMessage.ChainStopper;
                }
                if (!((Kills.Weapon == 19016 || Kills.Weapon == 19022) && Kills.KillerIdx == VictimId) || !VictimSlot.SpecGM)
                {
                    VictimSlot.AllDeaths++;
                }
                if (Kills.KillerIdx != AssistId)
                {
                    AssistSlot.AllAssists++;
                }
                if (Room.RoomType == RoomCondition.FreeForAll)
                {
                    Killer.AllKills++;
                    if (Killer.DeathState == DeadEnum.Alive)
                    {
                        Killer.KillsOnLife++;
                    }
                }
                else
                {
                    if (Killer.Team != VictimSlot.Team)
                    {
                        Score += AllUtils.GetKillScore(Frag.KillFlag);
                        Killer.AllKills++;
                        if (Killer.DeathState == DeadEnum.Alive)
                        {
                            Killer.KillsOnLife++;
                        }
                        if (VictimSlot.Team == 0)
                        {
                            Room.FRDeaths++;
                            Room.CTKills++;
                        }
                        else
                        {
                            Room.CTDeaths++;
                            Room.FRKills++;
                        }
                        if (Room.IsDinoMode("DE"))
                        {
                            if (Killer.Team == 0)
                            {
                                Room.FRDino += 4;
                            }
                            else
                            {
                                Room.CTDino += 4;
                            }
                        }
                    }
                }
                VictimSlot.LastKillState = 0;
                VictimSlot.KillsOnLife = 0;
                VictimSlot.RepeatLastState = false;
                VictimSlot.PassSequence = 0;
                VictimSlot.DeathState = DeadEnum.Dead;
                if (!IsBotMode)
                {
                    switch (WeaponClass)
                    {
                        case ClassType.Assault:
                        {
                            Killer.AR[0] += 1;
                            VictimSlot.AR[1] += 1;
                            break;
                        }
                        case ClassType.SMG:
                        {
                            Killer.SMG[0] += 1;
                            VictimSlot.SMG[1] += 1;
                            break;
                        }
                        case ClassType.Sniper:
                        {
                            Killer.SR[0] += 1;
                            VictimSlot.SR[1] += 1;
                            break;
                        }
                        case ClassType.Shotgun:
                        {
                            Killer.SG[0] += 1;
                            VictimSlot.SG[1] += 1;
                            break;
                        }
                        case ClassType.Machinegun:
                        {
                            Killer.MG[0] += 1;
                            VictimSlot.MG[1] += 1;
                            break;
                        }
                    }
                    AllUtils.CompleteMission(Room, VictimSlot, MissionType.DEATH, 0);
                }
                if (DeathType == CharaDeath.HEADSHOT)
                {
                    Killer.AllHeadshots++;
                }
            }
        }
        public static void EndBattleByDeath(RoomModel Room, SlotModel Killer, bool IsBotMode, bool IsSuicide)
        {
            if ((Room.RoomType == RoomCondition.DeathMatch) && !IsBotMode)
            {
                AllUtils.BattleEndKills(Room, IsBotMode);
            }
            else if (Room.RoomType == RoomCondition.FreeForAll)
            {
                AllUtils.BattleEndKillsFreeForAll(Room);
            }
            else if (!Killer.SpecGM && (Room.RoomType == RoomCondition.Bomb || Room.RoomType == RoomCondition.Annihilation || Room.RoomType == RoomCondition.Convoy || Room.RoomType == RoomCondition.Ace))
            {
                if (Room.RoomType == RoomCondition.Bomb || Room.RoomType == RoomCondition.Annihilation || Room.RoomType == RoomCondition.Convoy)
                {
                    TeamEnum Winner = TeamEnum.TEAM_DRAW;
                    Room.GetPlayingPlayers(true, out int allRed, out int allBlue, out int redDeaths, out int blueDeaths);
                    if (redDeaths == allRed && Killer.Team == TeamEnum.FR_TEAM && IsSuicide && !Room.ActiveC4)
                    {
                        Winner = TeamEnum.CT_TEAM;
                        Room.CTRounds++;
                        AllUtils.BattleEndRound(Room, Winner, true);
                    }
                    else if (blueDeaths == allBlue && Killer.Team == TeamEnum.CT_TEAM)
                    {
                        Winner = TeamEnum.FR_TEAM;
                        Room.FRRounds++;
                        AllUtils.BattleEndRound(Room, Winner, true);
                    }
                    else if (redDeaths == allRed && Killer.Team == TeamEnum.CT_TEAM)
                    {
                        if (!Room.ActiveC4)
                        {
                            Winner = TeamEnum.CT_TEAM;
                            Room.CTRounds++;
                        }
                        else if (IsSuicide)
                        {
                            Winner = TeamEnum.FR_TEAM;
                            Room.FRRounds++;
                        }
                        AllUtils.BattleEndRound(Room, Winner, false);
                    }
                    else if (blueDeaths == allBlue && Killer.Team == TeamEnum.FR_TEAM)
                    {
                        if (!IsSuicide || !Room.ActiveC4)
                        {
                            Winner = TeamEnum.FR_TEAM;
                            Room.FRRounds++;
                        }
                        else
                        {
                            Winner = TeamEnum.CT_TEAM;
                            Room.CTRounds++;
                        }
                        AllUtils.BattleEndRound(Room, Winner, true);
                    }
                }
                else if (Room.RoomType == RoomCondition.Ace)
                {
                    SlotModel[] Slots = new SlotModel[2] { Room.GetSlot(0), Room.GetSlot(1) };
                    if (Slots[0].DeathState == DeadEnum.Dead)
                    {
                        Room.CTRounds++;
                        AllUtils.BattleEndRound(Room, TeamEnum.CT_TEAM, true);
                    }
                    else if (Slots[1].DeathState == DeadEnum.Dead)
                    {
                        Room.FRRounds++;
                        AllUtils.BattleEndRound(Room, TeamEnum.FR_TEAM, true);
                    }
                }
            }
        }
    }
}
