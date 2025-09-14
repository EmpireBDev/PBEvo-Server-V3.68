using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_HOLE_CHECK_REQ : GameClientPacket
    {
        public PROTOCOL_BATTLE_HOLE_CHECK_REQ(GameClient client, byte[] data)
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
                    SlotModel Slot = Room.GetSlot(Player.SlotId);
                    if (Slot != null)
                    {
                        //TODO HERE
                    }
                }
                Client.SendPacket(new PROTOCOL_BATTLE_HOLE_CHECK_ACK(0));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}