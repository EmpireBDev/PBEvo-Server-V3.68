using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_GIVEUPBATTLE_REQ : GameClientPacket
    {
        private bool isFinished;
        private long objId;
        public PROTOCOL_BATTLE_GIVEUPBATTLE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            objId = ReadD(); //The Good Loser (Unit) - If he uses the item returns the OBJ of the item.
        }
        public override void Run()
        {
            try
            {
                Account player = Client.Player;
                if (player == null)
                {
                    return;
                }
                RoomModel room = player.Room;
                if (room != null && room.State >= RoomState.Loading && room.GetSlot(player.SlotId, out SlotModel slot) && slot.State >= SlotState.LOAD)
                {
                    bool isBotMode = room.IsBotMode();
                    FreepassEffect(player, slot, room, isBotMode);
                    if (room.VoteTime.Timer != null && room.votekick != null && room.votekick.VictimIdx == slot.Id)
                    {
                        room.VoteTime.Timer = null;
                        room.votekick = null;
                        using (PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK packet = new PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK())
                        {
                            room.SendPacketToPlayers(packet, SlotState.BATTLE, 0, slot.Id);
                        }
                    }
                    AllUtils.ResetSlotInfo(room, slot, true);
                    int red13 = 0, blue13 = 0, red9 = 0, blue9 = 0;
                    foreach (SlotModel slotR in room.Slots)
                    {
                        if (slotR.State >= SlotState.LOAD)
                        {
                            if (slotR.Team == 0)
                            {
                                red9++;
                            }
                            else
                            {
                                blue9++;
                            }
                            if (slotR.State == SlotState.BATTLE)
                            {
                                if (slotR.Team == 0)
                                {
                                    red13++;
                                }
                                else
                                {
                                    blue13++;
                                }
                            }
                        }
                    }
                    if (slot.Id == room.Leader)
                    {
                        if (isBotMode)
                        {
                            if (red13 > 0 || blue13 > 0)
                            {
                                LeaveHostBOT_GiveBattle(room, player);
                            }
                            else
                            {
                                LeaveHostBOT_EndBattle(room, player);
                            }
                        }
                        else if (room.State == RoomState.Battle && (red13 == 0 || blue13 == 0) || room.State <= RoomState.PreBattle && (red9 == 0 || blue9 == 0))
                        {
                            LeaveHostNoBOT_EndBattle(room, player, red13, blue13);
                        }
                        else
                        {
                            LeaveHostNoBOT_GiveBattle(room, player);
                        }
                    }
                    else if (!isBotMode)
                    {
                        if (room.State == RoomState.Battle && (red13 == 0 || blue13 == 0) || room.State <= RoomState.PreBattle && (red9 == 0 || blue9 == 0))
                        {
                            LeavePlayerNoBOT_EndBattle(room, player, red13, blue13);
                        }
                        else
                        {
                            LeavePlayer_QuitBattle(room, player);
                        }
                    }
                    else
                    {
                        LeavePlayer_QuitBattle(room, player);
                    }
                    Client.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(player, 0));
                    if (!isFinished && room.State == RoomState.Battle)
                    {
                        AllUtils.BattleEndRoundPlayersCount(room);
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_GIVEUPBATTLE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void FreepassEffect(Account Player, SlotModel Slot, RoomModel Room, bool IsBotMode)
        {
            DBQuery AccountQuery = new DBQuery();
            if (Player.Bonus.FreePass == 0 || Player.Bonus.FreePass == 1 && Room.ChannelType == ChannelType.Clan)
            {
                if (IsBotMode || Slot.State < SlotState.BATTLE_READY)
                {
                    return;
                }

                if (Player.Gold > 0)
                {
                    Player.Gold -= 200;
                    if (Player.Gold < 0)
                    {
                        Player.Gold = 0;
                    }
                    AccountQuery.AddQuery("gold", Player.Gold);
                }
                ComDiv.UpdateDB("player_stat_basics", "owner_id", Player.PlayerId, "escapes_count", ++Player.Statistic.Basic.EscapesCount);
                ComDiv.UpdateDB("player_stat_seasons", "owner_id", Player.PlayerId, "escapes_count", ++Player.Statistic.Season.EscapesCount);
            }
            else// if (ch._type != 4)
            {
                if (Room.State != RoomState.Battle)
                {
                    return;
                }
                int EXP = 0, Gold = 0;
                if (IsBotMode)
                {
                    int Level = Room.IngameAiLevel * (150 + Slot.AllDeaths);
                    if (Level == 0)
                    {
                        Level++;
                    }
                    int Reward = (Slot.Score / Level);
                    Gold += Reward;
                    EXP += Reward;
                }
                else
                {
                    int timePlayed = Slot.AllKills == 0 && Slot.AllDeaths == 0 ? 0 : (int)Slot.InBattleTime(DateTimeUtil.Now());
                    if (Room.RoomType == RoomCondition.Bomb || Room.RoomType == RoomCondition.FreeForAll || Room.RoomType == RoomCondition.Convoy) // New Mode
                    {
                        EXP = (int)(Slot.Score + (timePlayed / 2.5) + (Slot.AllDeaths * 2.2) + (Slot.Objects * 20));
                        Gold = (int)(Slot.Score + (timePlayed / 3.0) + (Slot.AllDeaths * 2.2) + (Slot.Objects * 20));
                    }
                    else
                    {
                        EXP = (int)(Slot.Score + (timePlayed / 2.5) + (Slot.AllDeaths * 1.8) + (Slot.Objects * 20));
                        Gold = (int)(Slot.Score + (timePlayed / 3.0) + (Slot.AllDeaths * 1.8) + (Slot.Objects * 20));
                    }
                }
                Player.Exp += ConfigLoader.MaxExpReward < EXP ? ConfigLoader.MaxExpReward : EXP;
                Player.Gold += ConfigLoader.MaxGoldReward < Gold ? ConfigLoader.MaxGoldReward : Gold;
                if (Gold > 0)
                {
                    AccountQuery.AddQuery("gold", Player.Gold);
                }
                if (EXP > 0)
                {
                    AccountQuery.AddQuery("experience", Player.Exp);
                }
            }
            ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, AccountQuery.GetTables(), AccountQuery.GetValues());
        }
        private void LeaveHostBOT_GiveBattle(RoomModel Room, Account Player)
        {
            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
            if (Players.Count == 0)
            {
                return;
            }
            int oldLeader = Room.Leader;
            Room.SetNewLeader(-1, SlotState.BATTLE_READY, oldLeader, true);
            using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK Packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0))
            {
                using (PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK Packet2 = new PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK(Room))
                {
                    byte[] Data = Packet.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-1");
                    byte[] Data2 = Packet2.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-2");
                    foreach (Account Member in Players)
                    {
                        SlotModel Slot = Room.GetSlot(Member.SlotId);
                        if (Slot != null)
                        {
                            if (Slot.State >= SlotState.PRESTART)
                            {
                                Member.SendCompletePacket(Data2, Packet2.GetType().Name);
                            }
                            Member.SendCompletePacket(Data, Packet.GetType().Name);
                        }
                    }
                }
            }
        }
        private void LeaveHostBOT_EndBattle(RoomModel Room, Account Player)
        {
            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
            if (Players.Count > 0)
            {
                using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK Packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0))
                {
                    byte[] Buffer = Packet.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-3");
                    TeamEnum winnerTeam = AllUtils.GetWinnerTeam(Room);
                    AllUtils.GetBattleResult(Room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                    foreach (Account Member in Players)
                    {
                        Member.SendCompletePacket(Buffer, Packet.GetType().Name);
                        Member.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Member, winnerTeam, inBattle, missionCompletes, true, Data));
                    }
                }
            }
            AllUtils.ResetBattleInfo(Room);
        }
        private void LeaveHostNoBOT_EndBattle(RoomModel Room, Account Player, int TeamFR, int TeamCT)
        {
            isFinished = true;
            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
            if (Players.Count > 0)
            {
                TeamEnum WinnerTeam = AllUtils.GetWinnerTeam(Room, TeamFR, TeamCT);
                if (Room.State == RoomState.Battle)
                {
                    Room.CalculateResult(WinnerTeam, false);
                }
                using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK Packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0))
                {
                    byte[] Buffer = Packet.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-4");
                    AllUtils.GetBattleResult(Room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                    foreach (Account Member in Players)
                    {
                        Member.SendCompletePacket(Buffer, Packet.GetType().Name);
                        Member.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Member, WinnerTeam, inBattle, missionCompletes, false, Data));
                    }
                }
            }
            AllUtils.ResetBattleInfo(Room);
        }
        private void LeaveHostNoBOT_GiveBattle(RoomModel Room, Account Player)
        {
            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
            if (Players.Count == 0)
            {
                return;
            }
            int oldLeader = Room.Leader;
            SlotState state = (Room.State == RoomState.Battle ? SlotState.BATTLE_READY : SlotState.READY);
            Room.SetNewLeader(-1, state, oldLeader, true);
            using (PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK Packet = new PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK(Room))
            {
                using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK Packet2 = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0))
                {
                    byte[] Data = Packet.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-6");
                    byte[] Data2 = Packet2.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-7");
                    foreach (Account pR in Players)
                    {
                        if (Room.Slots[pR.SlotId].State >= SlotState.PRESTART)
                        {
                            pR.SendCompletePacket(Data, Packet.GetType().Name);
                        }
                        pR.SendCompletePacket(Data2, Packet2.GetType().Name);
                    }
                }
            }
        }
        private void LeavePlayerNoBOT_EndBattle(RoomModel Room, Account Player, int TeamFR, int TeamCT)
        {
            isFinished = true;
            TeamEnum WinnerTeam = AllUtils.GetWinnerTeam(Room, TeamFR, TeamCT);
            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
            if (Players.Count > 0)
            {
                if (Room.State == RoomState.Battle)
                {
                    Room.CalculateResult(WinnerTeam, false);
                }
                using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK Packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0))
                {
                    byte[] Buffer = Packet.GetCompleteBytes("PROTOCOL_BATTLE_GIVEUPBATTLE_REQ-8");
                    AllUtils.GetBattleResult(Room, out ushort missionCompletes, out ushort inBattle, out byte[] Data);
                    foreach (Account Member in Players)
                    {
                        Member.SendCompletePacket(Buffer, Packet.GetType().Name);
                        Member.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Member, WinnerTeam, inBattle, missionCompletes, false, Data));
                    }
                }
            }
            AllUtils.ResetBattleInfo(Room);
        }
        private void LeavePlayer_QuitBattle(RoomModel Room, Account Player)
        {
            using (PROTOCOL_BATTLE_GIVEUPBATTLE_ACK Packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0))
            {
                Room.SendPacketToPlayers(Packet, SlotState.READY, 1);
            }
        }
    }
}