using Plugin.Core;
using Plugin.Core.Enums;
using Server.Auth.Network.ServerPacket;
using System;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GAMEGUARD_REQ : AuthClientPacket
    {
        private byte[] Version;
        public PROTOCOL_BASE_GAMEGUARD_REQ(AuthClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ReadB(48);
            Version = ReadB(3);
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_BASE_GAMEGUARD_ACK(0, Version));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_GAMEGUARD_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
