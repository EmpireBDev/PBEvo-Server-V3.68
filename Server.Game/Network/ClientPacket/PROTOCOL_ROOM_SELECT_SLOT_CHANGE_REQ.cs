using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_SELECT_SLOT_CHANGE_REQ : GameClientPacket
    {
        private int SlotIdx;
        public PROTOCOL_ROOM_SELECT_SLOT_CHANGE_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            SlotIdx = ReadC();
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
                RoomModel room = Player.Room;
                if (room != null && !room.ChangingSlots)
                {
                    SlotModel Slot = room.GetSlot(Player.SlotId);
                    if (Slot != null && Slot.State == SlotState.NORMAL)
                    {
                        lock (room.Slots)
                        {
                            room.ChangingSlots = true;
                            List<SlotModel> changeList = new List<SlotModel>();
                            room.SwitchNewSlot(changeList, Player, Slot, TeamEnum.TEAM_DRAW, SlotIdx);
                            if (changeList.Count > 0)
                            {
                                using (PROTOCOL_ROOM_TEAM_BALANCE_ACK packet = new PROTOCOL_ROOM_TEAM_BALANCE_ACK(changeList, room.Leader, 0))
                                {
                                    room.SendPacketToPlayers(packet);
                                }
                            }
                            room.ChangingSlots = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_SELECT_SLOT_CHANGE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
