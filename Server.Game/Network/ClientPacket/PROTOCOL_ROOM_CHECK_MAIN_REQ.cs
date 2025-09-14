using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    internal class PROTOCOL_ROOM_CHECK_MAIN_REQ : GameClientPacket
    {
        private List<SlotModel> slots = new List<SlotModel>();
        private uint erro;
        public PROTOCOL_ROOM_CHECK_MAIN_REQ(GameClient client, byte[] data)
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
                if (Room != null && Room.Leader == Player.SlotId && Room.State == RoomState.Ready)
                {
                    lock (Room.Slots)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            SlotModel slot = Room.Slots[i];
                            if (slot.PlayerId > 0 && i != Room.Leader)
                            {
                                slots.Add(slot);
                            }
                        }
                    }
                    if (slots.Count > 0)
                    {
                        int idx = new Random().Next(slots.Count);
                        SlotModel result = slots[idx];
                        erro = Room.GetPlayerBySlot(result) != null ? (uint)result.Id : 0x80000000;
                        slots = null;
                    }
                    else
                    {
                        erro = 0x80000000;
                    }
                }
                else
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_ROOM_CHECK_MAIN_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_CHECK_MAIN_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}