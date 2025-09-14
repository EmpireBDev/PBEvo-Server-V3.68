using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_RESULT_REQ : GameClientPacket
    {
        public PROTOCOL_CLAN_WAR_RESULT_REQ(GameClient Client, byte[] Buffer)
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
                Client.SendPacket(new PROTOCOL_CLAN_WAR_RESULT_ACK());
            }
            catch (Exception Ex)
            {
                CLogger.Print($"PROTOCOL_CLAN_WAR_RESULT_REQ: {Ex.Message}", LoggerType.Error, Ex);
            }
        }
    }
}
