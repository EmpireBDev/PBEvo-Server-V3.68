using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CLIENT_CLAN_CONTEXT_REQ : GameClientPacket
    {
        public PROTOCOL_CS_CLIENT_CLAN_CONTEXT_REQ(GameClient client, byte[] data)
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
                 Client.SendPacket(new PROTOCOL_CS_CLIENT_CLAN_CONTEXT_ACK(ClanManager.Clans.Count));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}