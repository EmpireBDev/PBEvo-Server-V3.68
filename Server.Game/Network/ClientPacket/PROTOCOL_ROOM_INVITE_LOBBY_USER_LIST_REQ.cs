using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_REQ : GameClientPacket
    {
        public PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_REQ(GameClient client, byte[] data)
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
                ChannelModel Channel = Player.GetChannel();
                if (Channel != null)
                {
                    Client.SendPacket(new PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_ACK(Channel));
                }
            }
            catch (Exception ex) 
            { 
                CLogger.Print($"PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ: {ex.Message}", LoggerType.Error, ex); 
            }
        }
    }
}