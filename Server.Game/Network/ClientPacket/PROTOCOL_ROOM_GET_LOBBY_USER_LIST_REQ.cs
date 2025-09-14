using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ : GameClientPacket
    {
        private byte page;
        public PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            page = ReadC();
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
                    Client.SendPacket(new PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK(Channel));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
