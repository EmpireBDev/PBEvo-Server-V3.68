using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_TICKET_UPDATE_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_TICKET_UPDATE_REQ(GameClient client, byte[] data)
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
                Client.SendPacket(new PROTOCOL_BASE_TICKET_UPDATE_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_USER_ENTER_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
