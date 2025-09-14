using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_GET_USER_EQUIPMENT_REQ : GameClientPacket
    {
        private int TargetSlot;
        public PROTOCOL_ROOM_GET_USER_EQUIPMENT_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            TargetSlot = ReadC();
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
                if (Room != null && Room.GetSlot(TargetSlot, out SlotModel Slot))
                {
                    Account Member = Room.GetPlayerBySlot(Slot);
                    if (Member != null && Member.SlotId != Player.SlotId)
                    {
                        Client.SendPacket(new PROTOCOL_ROOM_GET_USER_EQUIPMENT_ACK(0, Member, Slot));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_GET_USER_EQUIPMENT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
