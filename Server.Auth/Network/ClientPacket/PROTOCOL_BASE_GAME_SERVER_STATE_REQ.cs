using Plugin.Core;
using Plugin.Core.Enums;
using Server.Auth.Network.ServerPacket;
using System;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GAME_SERVER_STATE_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_GAME_SERVER_STATE_REQ(AuthClient Client, byte[] Data)
        {
            Makeme(Client, Data);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_BASE_GAME_SERVER_STATE_ACK());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
