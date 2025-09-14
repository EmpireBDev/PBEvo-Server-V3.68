using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CANCEL_REQUEST_REQ : GameClientPacket
    {
        private uint erro;
        public PROTOCOL_CS_CANCEL_REQUEST_REQ(GameClient client, byte[] data)
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
                Account Player = Client.Player;
                if (Player == null || !DaoManagerSQL.DeleteClanInviteDB(Client.PlayerId))
                {
                    erro = 2147487835;
                }
                Client.SendPacket(new PROTOCOL_CS_CANCEL_REQUEST_ACK(erro));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}