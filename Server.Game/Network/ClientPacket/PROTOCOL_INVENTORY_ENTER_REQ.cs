using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_INVENTORY_ENTER_REQ : GameClientPacket
    {
        public PROTOCOL_INVENTORY_ENTER_REQ(GameClient client, byte[] data)
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
                    Room.ChangeSlotState(Player.SlotId, SlotState.INVENTORY, false);
                    Room.StopCountDown(Player.SlotId);
                    Room.UpdateSlotsInfo();
                }
                Client.SendPacket(new PROTOCOL_INVENTORY_ENTER_ACK());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
