using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_MEMBER_CONTEXT_REQ : GameClientPacket
    {
        public PROTOCOL_CS_MEMBER_CONTEXT_REQ(GameClient client, byte[] data)
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
                if (Player == null)
                {
                    return;
                }
                int ClanId = Player.FindClanId, error, players;
                if (ClanId == 0)
                {
                    error = -1;
                    players = 0;
                }
                else
                {
                    error = 0;
                    players = DaoManagerSQL.GetClanPlayers(ClanId);
                }
                Client.SendPacket(new PROTOCOL_CS_MEMBER_CONTEXT_ACK(error, players));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}