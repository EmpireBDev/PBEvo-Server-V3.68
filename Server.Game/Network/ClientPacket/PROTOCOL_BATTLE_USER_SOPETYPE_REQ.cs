using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_USER_SOPETYPE_REQ : GameClientPacket
    {
        private int Sight;
        public PROTOCOL_BATTLE_USER_SOPETYPE_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            Sight = ReadC();
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
                    Player.Sight = Sight; //WTF is this?
                    using (PROTOCOL_BATTLE_USER_SOPETYPE_ACK Packet = new PROTOCOL_BATTLE_USER_SOPETYPE_ACK(Player))
                    {
                        Room.SendPacketToPlayers(new PROTOCOL_BATTLE_USER_SOPETYPE_ACK(Player));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_USER_SOPETYPE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
