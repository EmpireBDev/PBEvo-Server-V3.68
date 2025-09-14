using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REQUEST_INFO_REQ : GameClientPacket
    {
        private long pId;
        public PROTOCOL_CS_REQUEST_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            pId = ReadQ();
        }
        public override void Run()
        {
            try
            {
                Account player = Client.Player;
                if (player == null)
                {
                    return;
                }
                Client.SendPacket(new PROTOCOL_CS_REQUEST_INFO_ACK(pId, DaoManagerSQL.GetRequestClanInviteText(player.ClanId, pId)));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}