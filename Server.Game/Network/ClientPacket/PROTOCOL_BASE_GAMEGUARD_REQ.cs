using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GAMEGUARD_REQ : GameClientPacket
    {
        private byte[] Version;
        public PROTOCOL_BASE_GAMEGUARD_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
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
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
