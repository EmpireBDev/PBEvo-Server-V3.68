using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_SHOP_LEAVE_REQ : GameClientPacket
    {
        public PROTOCOL_SHOP_LEAVE_REQ(GameClient client, byte[] data)
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
                if (room != null)
                {
                    room.ChangeSlotState(Player.SlotId, SlotState.NORMAL, true);
                }
                Client.SendPacket(new PROTOCOL_SHOP_LEAVE_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_SHOP_LEAVE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}