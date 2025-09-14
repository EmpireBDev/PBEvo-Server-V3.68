using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_CHANGE_COSTUME_REQ : GameClientPacket
    {
        private int Team;
        public PROTOCOL_ROOM_CHANGE_COSTUME_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            Team = ReadC();
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
                        Slot.Costume = Team;
                        Client.SendPacket(new PROTOCOL_ROOM_CHANGE_COSTUME_ACK(Slot));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_CHANGE_COSTUME_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
