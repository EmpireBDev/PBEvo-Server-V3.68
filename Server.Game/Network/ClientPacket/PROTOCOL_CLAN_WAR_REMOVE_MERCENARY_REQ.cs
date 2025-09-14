using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_REQ : GameClientPacket
    {
        public PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_REQ(GameClient Client, byte[] Buffer)
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
                Client.SendPacket(new PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
