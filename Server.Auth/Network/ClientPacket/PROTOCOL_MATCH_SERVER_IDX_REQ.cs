using Plugin.Core;
using Plugin.Core.Enums;
using Server.Auth.Network.ServerPacket;
using System;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_MATCH_SERVER_IDX_REQ : AuthClientPacket
    {
        private short ServerId;
        public PROTOCOL_MATCH_SERVER_IDX_REQ(AuthClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ServerId = ReadH();
            ReadC();
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_MATCH_SERVER_IDX_ACK(ServerId));
                Client.Close(0, false);
            }
            catch (Exception Ex)
            {
                CLogger.Print($"PROTOCOL_MATCH_SERVER_IDX_REQ: {Ex.Message}", LoggerType.Warning);
            }
        }
    }
}
