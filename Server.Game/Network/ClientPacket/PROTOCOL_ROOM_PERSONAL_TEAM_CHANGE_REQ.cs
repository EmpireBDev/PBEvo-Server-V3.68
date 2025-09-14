using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_PERSONAL_TEAM_CHANGE_REQ : GameClientPacket
    {
        private TeamEnum TeamIdx;
        private int SlotIdx;
        public PROTOCOL_ROOM_PERSONAL_TEAM_CHANGE_REQ(GameClient Client, byte[] Data)
        {
            Makeme(Client, Data);
        }
        public override void Read()
        {
            TeamIdx = (TeamEnum)ReadD();
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
                if (TeamIdx != TeamEnum.ALL_TEAM && room != null && !room.ChangingSlots)
                {
                    SlotModel Slot = room.GetSlot(Player.SlotId);
                    if (Slot != null && Slot.State == SlotState.NORMAL)
                    {
                        lock (room.Slots)
                        {
                            room.ChangingSlots = true;
                            List<SlotModel> changeList = new List<SlotModel>();
                            room.SwitchNewSlot(changeList, Player, Slot, TeamIdx, SlotIdx);
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
                CLogger.Print($"PROTOCOL_ROOM_PERSONAL_TEAM_CHANGE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}