using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GUIDE_COMPLETE_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_GUIDE_COMPLETE_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_BASE_GUIDE_COMPLETE_ACK());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
