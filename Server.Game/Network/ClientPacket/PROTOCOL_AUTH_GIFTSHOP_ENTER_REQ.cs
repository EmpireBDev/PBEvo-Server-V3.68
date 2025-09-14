using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_GIFTSHOP_ENTER_REQ : GameClientPacket
    {
        private string md5;
        public PROTOCOL_AUTH_GIFTSHOP_ENTER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            md5 = ReadS(32);
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
                    Room.ChangeSlotState(Player.SlotId, SlotState.GIFTSHOP, false);
                    Room.StopCountDown(Player.SlotId);
                    Room.UpdateSlotsInfo();
                }
                Client.SendPacket(new PROTOCOL_AUTH_GIFTSHOP_ENTER_ACK());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
