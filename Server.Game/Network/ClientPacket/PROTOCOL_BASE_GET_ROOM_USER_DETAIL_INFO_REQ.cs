using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_ROOM_USER_DETAIL_INFO_REQ : GameClientPacket
    {
        private int SlotId;
        public PROTOCOL_BASE_GET_ROOM_USER_DETAIL_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            SlotId = ReadD();
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
                if (Room != null && Room.GetSlot(SlotId, out SlotModel Slot))
                {
                    Client.SendPacket(new PROTOCOL_BASE_GET_USER_DETAIL_INFO_ACK(0, Room.GetPlayerBySlot(Slot)));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_UNKNOWN_PACKET_631_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
