using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_MATCH_SERVER_IDX_REQ : GameClientPacket
    {
        private short ServerId;
        public PROTOCOL_MATCH_SERVER_IDX_REQ(GameClient Client, byte[] Buffer)
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
