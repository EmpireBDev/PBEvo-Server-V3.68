using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ : GameClientPacket
    {
        private int RoomId;
        public PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            RoomId = ReadD();
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
                ChannelModel Channel = Player.GetChannel();
                if (Channel != null)
                {
                    RoomModel Room = Channel.GetRoom(RoomId);
                    if (Room != null && Room.GetLeader() != null)
                    {
                        Client.SendPacket(new PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK(Room));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}