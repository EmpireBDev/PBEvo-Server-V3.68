using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_LOGOUT_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_LOGOUT_REQ(GameClient client, byte[] data)
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
                Client.SendPacket(new PROTOCOL_BASE_LOGOUT_ACK());
                Client.Close(1000, true);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                Client.Close(0, true);
            }
        }
    }
}