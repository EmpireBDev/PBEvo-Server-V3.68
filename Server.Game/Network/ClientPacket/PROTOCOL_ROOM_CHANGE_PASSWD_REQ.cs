using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_CHANGE_PASSWD_REQ : GameClientPacket
    {
        private string Password;
        public PROTOCOL_ROOM_CHANGE_PASSWD_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Password = ReadS(4);
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
                if (Room != null && Room.Leader == Player.SlotId && Room.Password != Password)
                {
                    Room.Password = Password;
                    using (PROTOCOL_ROOM_CHANGE_PASSWD_ACK packet = new PROTOCOL_ROOM_CHANGE_PASSWD_ACK(Password))
                    {
                        Room.SendPacketToPlayers(packet);
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}