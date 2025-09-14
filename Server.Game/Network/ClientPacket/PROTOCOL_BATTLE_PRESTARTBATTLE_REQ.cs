using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_PRESTARTBATTLE_REQ : GameClientPacket
    {
        private StageOptions Stage;
        private MapRules Rule;
        private MapIdEnum MapId;
        private RoomCondition RoomType;
        public PROTOCOL_BATTLE_PRESTARTBATTLE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MapId = (MapIdEnum)ReadC();
            Rule = (MapRules)ReadC();
            Stage = (StageOptions)ReadC();
            RoomType = (RoomCondition)ReadC();
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
                if (Room != null && (Room.Stage == Stage && Room.RoomType == RoomType && Room.MapId == MapId && Room.Rule == Rule))
                {
                    SlotModel Slot = Room.Slots[Player.SlotId];
                    if (Room.IsPreparing() && Room.UdpServer != null && Slot.State >= SlotState.LOAD)
                    {
                        Account Leader = Room.GetLeader();
                        if (Leader != null)
                        {
                            if (string.IsNullOrEmpty(Player.PublicIP.ToString()))
                            {
                                Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_NO_REAL_IP));
                                Client.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0));
                                Room.ChangeSlotState(Slot, SlotState.NORMAL, true);
                                AllUtils.BattleEndPlayersCount(Room, Room.IsBotMode());
                                Slot.StopTiming();
                                return;
                            }
                            if (Slot.Id == Room.Leader)
                            {
                                Room.State = RoomState.PreBattle;
                                Room.UpdateRoomInfo();
                            }
                            Slot.PreStartDate = DateTimeUtil.Now();
                            Room.StartCounter(1, Player, Slot);
                            Room.ChangeSlotState(Slot, SlotState.PRESTART, true);
                            Client.SendPacket(new PROTOCOL_BATTLE_PRESTARTBATTLE_ACK(Player, true));
                            if (Slot.Id != Room.Leader)
                            {
                                Leader.SendPacket(new PROTOCOL_BATTLE_PRESTARTBATTLE_ACK(Player, false));
                            }
                            //slot.state = SLOT_STATE.BATTLE;
                            //room.updateSlotsInfo();
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_FIRST_HOLE));
                            Client.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0));
                            Room.ChangeSlotState(Slot, SlotState.NORMAL, true);
                            AllUtils.BattleEndPlayersCount(Room, Room.IsBotMode());
                            Slot.StopTiming();
                        }
                    }
                    else
                    {
                        Room.ChangeSlotState(Slot, SlotState.NORMAL, true);
                        Client.SendPacket(new PROTOCOL_BATTLE_STARTBATTLE_ACK());
                        AllUtils.BattleEndPlayersCount(Room, Room.IsBotMode());
                        Slot.StopTiming();
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_FIRST_MAINLOAD));
                    Client.SendPacket(new PROTOCOL_BATTLE_PRESTARTBATTLE_ACK());
                    if (Room != null)
                    {
                        Room.ChangeSlotState(Player.SlotId, SlotState.NORMAL, true);
                        AllUtils.BattleEndPlayersCount(Room, Room.IsBotMode());
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_LOBBY_ENTER_ACK(0));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_PRESTARTBATTLE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}