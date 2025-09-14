using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ : GameClientPacket
    {
        private int slotId;
        public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            slotId = ReadD();
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
                if (room == null || room.Leader == slotId || room.Slots[slotId].PlayerId == 0)
                {
                    Client.SendPacket(new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(0x80000000));
                }
                else if (room.State == RoomState.Ready && room.Leader == player.SlotId)
                {
                    room.SetNewLeader(slotId, SlotState.EMPTY, room.Leader, false);
                    using (PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK packet = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(slotId))
                    {
                        room.SendPacketToPlayers(packet);
                    }
                    room.UpdateSlotsInfo();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}