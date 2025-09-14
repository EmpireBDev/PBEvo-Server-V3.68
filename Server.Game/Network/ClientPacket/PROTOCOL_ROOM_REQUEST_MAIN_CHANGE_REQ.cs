using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    internal class PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_REQ : GameClientPacket
    {
        private List<SlotModel> slots = new List<SlotModel>();
        public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_REQ(GameClient client, byte[] data)
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
                RoomModel room = Player.Room;
                if (room != null && room.Leader == Player.SlotId && room.State == RoomState.Ready)
                {
                    lock (room.Slots)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            SlotModel slot = room.Slots[i];
                            if (slot.PlayerId > 0 && i != room.Leader)
                            {
                                slots.Add(slot);
                            }
                        }
                    }
                    if (slots.Count > 0)
                    {
                        SlotModel slot = slots[new Random().Next(slots.Count)];
                        Account player = room.GetPlayerBySlot(slot);
                        if (player != null)
                        {
                            room.SetNewLeader(slot.Id, SlotState.EMPTY, room.Leader, false);
                            using (PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_ACK packet = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_ACK(slot.Id))
                            {
                                room.SendPacketToPlayers(packet);
                            }
                            room.UpdateSlotsInfo();
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_ACK(0x80000000));
                        }
                        slots = null;
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_ACK(0x80000000));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"ROOM_RANDOM_HOST2_REC: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}