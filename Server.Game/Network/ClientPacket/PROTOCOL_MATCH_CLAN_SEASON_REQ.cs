using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_MATCH_CLAN_SEASON_REQ : GameClientPacket
    {
        public PROTOCOL_MATCH_CLAN_SEASON_REQ(GameClient Client, byte[] Buffer)
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
                Client.SendPacket(new PROTOCOL_MATCH_CLAN_SEASON_ACK(false));
                Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_USER_EVENT_START_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_MATCH_CLAN_SEASON_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
