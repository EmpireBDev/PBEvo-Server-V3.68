using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_INVENTORY_LEAVE_REQ : GameClientPacket
    {
        public PROTOCOL_INVENTORY_LEAVE_REQ(GameClient client, byte[] data)
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
                if (Room != null)
                {
                    Room.ChangeSlotState(Player.SlotId, SlotState.NORMAL, true);
                }
                Client.SendPacket(new PROTOCOL_INVENTORY_LEAVE_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_INVENTORY_LEAVE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}