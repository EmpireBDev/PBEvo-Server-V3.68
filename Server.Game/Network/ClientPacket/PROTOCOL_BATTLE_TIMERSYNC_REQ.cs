using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using Server.Game.Data.Models;
using Plugin.Core.Models;
using Plugin.Core.SQL;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_TIMERSYNC_REQ : GameClientPacket
    {
        private float Value;
        private uint TimeRemaining;
        private int Ping, Hack, Latency, Round;
        public PROTOCOL_BATTLE_TIMERSYNC_REQ(GameClient Client, byte[] Buff)
        {
            Makeme(Client, Buff);
        }
        public override void Read()
        {
            TimeRemaining = ReadUD();
            Value = ReadT(); // Value Hack
            Round = ReadC(); // Round
            Ping = ReadC(); // Ping
            Hack = ReadC(); // Hack Type
            Latency = ReadH(); // Latency
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
                RoomModel Room = Player.Room;
                if (Room != null && Room.GetSlot(Player.SlotId, out SlotModel Slot))
                {
                    bool IsBotMode = Room.IsBotMode();
                    if (Slot == null || Slot.State != SlotState.BATTLE)
                    {
                        return;
                    }
                    if (Value != 0 || Hack != 0)
                    {
                        AllUtils.ValidateBanPlayer(Player, $"Using an illegal program! ({Value}/{Hack})");
                    }
                    SyncPlayerPings(Player, Room, Slot, IsBotMode);
                    Room.TimeRoom = TimeRemaining;
                    if ((TimeRemaining == 0 || TimeRemaining > 0x80000000) && !Room.SwapRound && CompareRounds(Room, Round) && Room.State == RoomState.Battle)
                    {
                        EndRound(Room, IsBotMode);
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_TIMERSYNC_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void SyncPlayerPings(Account Player, RoomModel Room, SlotModel Slot, bool IsBotMode)
        {
            if (IsBotMode)
            {
                return;
            }
            Slot.Latency = Latency;
            Slot.Ping = Ping;
            if (Slot.Latency >= ConfigLoader.MaxLatency)
            {
                Slot.FailLatencyTimes++;
            }
            else
            {
                Slot.FailLatencyTimes = 0;
            }
            if (ConfigLoader.IsDebugPing && ComDiv.GetDuration(Player.LastPingDebug) >= ConfigLoader.PingUpdateTime)
            {
                Player.LastPingDebug = DateTimeUtil.Now();
                Player.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK("Server", 0, 5, false, $"{Latency}ms ({Ping} bar)"));
            }
            if (Slot.FailLatencyTimes >= ConfigLoader.MaxRepeatLatency)
            {
                CLogger.Print($"Player: '{Player.Nickname}' (Id: {Slot.PlayerId}) kicked due to high latency. ({Slot.Latency}/{ConfigLoader.MaxLatency}ms)", LoggerType.Warning);
                Client.Close(500, true);
                return;
            }
            else
            {
                AllUtils.RoomPingSync(Room);
            }
        }
        private bool CompareRounds(RoomModel Room, int ExternValue)
        {
            return (Room.Rounds == ExternValue);
        }
        private void EndRound(RoomModel Room, bool IsBotMode)
        {
            try
            {
                Room.SwapRound = true;
                if (Room.IsDinoMode())
                {
                    if (Room.Rounds == 1)
                    {
                        Room.Rounds = 2;
                        foreach (SlotModel Slot in Room.Slots)
                        {
                            if (Slot.State == SlotState.BATTLE)
                            {
                                Slot.KillsOnLife = 0;
                                Slot.LastKillState = 0;
                                Slot.RepeatLastState = false;
                            }
                        }
                        List<int> Dinos = AllUtils.GetDinossaurs(Room, true, -2);
                        using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK Packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Room, 2, RoundEndType.TimeOut))
                        {
                            using (PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK Packet2 = new PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(Room, Dinos, IsBotMode))
                            {
                                Room.SendPacketToPlayers(Packet, Packet2, SlotState.BATTLE, 0);
                            }
                        }
                        Room.StartDinoRound();
                    }
                    else if (Room.Rounds == 2)
                    {
                        AllUtils.EndBattle(Room, IsBotMode);
                    }
                }
                else if (Room.ThisModeHaveRounds())
                {
                    TeamEnum Winner = TeamEnum.TEAM_DRAW;
                    if (Room.RoomType != RoomCondition.Destroy)
                    {
                        Room.CTRounds++;
                        Winner = TeamEnum.CT_TEAM;
                    }
                    else
                    {
                        if (Room.Bar1 > Room.Bar2)
                        {
                            Room.FRRounds++;
                            Winner = TeamEnum.FR_TEAM;
                        }
                        else if (Room.Bar1 < Room.Bar2)
                        {
                            Room.CTRounds++;
                            Winner = TeamEnum.CT_TEAM;
                        }
                        else
                        {
                            Winner = TeamEnum.TEAM_DRAW;
                        }
                    }
                    AllUtils.BattleEndRound(Room, Winner, RoundEndType.TimeOut);
                }
                else if (Room.RoomType == RoomCondition.Ace)
                {
                    SlotModel[] Slots = new SlotModel[2] { Room.GetSlot(0), Room.GetSlot(1) };
                    if ((Slots[0] == null || Slots[0].State != SlotState.BATTLE) || (Slots[1] == null || Slots[1].State != SlotState.BATTLE))
                    {
                        AllUtils.EndBattleNoPoints(Room);
                    }
                }
                else
                {
                    List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
                    if (Players.Count > 0)
                    {
                        TeamEnum WinnerTeam = AllUtils.GetWinnerTeam(Room);
                        Room.CalculateResult(WinnerTeam, IsBotMode);
                        using (PROTOCOL_BATTLE_MISSION_ROUND_END_ACK Packet = new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Room, WinnerTeam, RoundEndType.TimeOut))
                        {
                            AllUtils.GetBattleResult(Room, out ushort MissionCompletes, out ushort InBattle, out byte[] Data);
                            byte[] PacketData = Packet.GetCompleteBytes("PROTOCOL_BATTLE_TIMERSYNC_REQ");
                            foreach (Account Member in Players)
                            {
                                if (Room.Slots[Member.SlotId].State == SlotState.BATTLE)
                                {
                                    Member.SendCompletePacket(PacketData, Packet.GetType().Name);
                                }
                                Member.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Member, WinnerTeam, InBattle, MissionCompletes, IsBotMode, Data));
                            }
                        }
                    }
                    AllUtils.ResetBattleInfo(Room);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}