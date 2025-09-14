using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_TOTAL_TEAM_CHANGE_REQ : GameClientPacket
    {
        public PROTOCOL_ROOM_TOTAL_TEAM_CHANGE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
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
                if (Room != null && Room.Leader == Player.SlotId && Room.State == RoomState.Ready && !Room.ChangingSlots)
                {
                    List<SlotModel> ChangeList = new List<SlotModel>();
                    lock (Room.Slots)
                    {
                        Room.ChangingSlots = true;
                        foreach (int SlotIdx in Room.FR_TEAM)
                        {
                            int NewId = (SlotIdx + 1);
                            if (SlotIdx == Room.Leader)
                            {
                                Room.Leader = NewId;
                            }
                            else if (NewId == Room.Leader)
                            {
                                Room.Leader = SlotIdx;
                            }
                            Room.SwitchSlots(ChangeList, NewId, SlotIdx, true);
                        }
                        if (ChangeList.Count > 0)
                        {
                            using (PROTOCOL_ROOM_TEAM_BALANCE_ACK Packet = new PROTOCOL_ROOM_TEAM_BALANCE_ACK(ChangeList, Room.Leader, 2))
                            {
                                byte[] Data = Packet.GetCompleteBytes("PROTOCOL_ROOM_CHANGE_TEAM_REQ");
                                foreach (Account Member in Room.GetAllPlayers())
                                {
                                    Member.SlotId = AllUtils.GetNewSlotId(Member.SlotId);
                                    Member.SendCompletePacket(Data, Packet.GetType().Name);
                                }
                            }
                        }
                        Room.ChangingSlots = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_CHANGE_TEAM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}