using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GAME_SERVER_STATE_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_GAME_SERVER_STATE_REQ(GameClient Client, byte[] Data)
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
