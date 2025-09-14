using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ : GameClientPacket
    {
        private int SlotId;
        public PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
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
                    if (Player.SlotId == Slot.Id)
                    {
                        Player.SendPacket(new PROTOCOL_BATTLE_TIMEOUTCLIENT_ACK());
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
