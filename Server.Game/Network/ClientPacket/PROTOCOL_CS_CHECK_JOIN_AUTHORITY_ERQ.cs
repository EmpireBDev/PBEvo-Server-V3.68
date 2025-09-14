using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CHECK_JOIN_AUTHORITY_ERQ : GameClientPacket
    {
        private int clanId;
        private uint erro;
        public PROTOCOL_CS_CHECK_JOIN_AUTHORITY_ERQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            clanId = ReadD();
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
                ClanModel Clan = ClanManager.GetClan(clanId);
                if (Clan.Id == 0)
                {
                    erro = 0x80000000;
                }
                else if (Clan.RankLimit > Player.Rank)
                {
                    erro = 2147487867;
                }
                Client.SendPacket(new PROTOCOL_CS_CHECK_JOIN_AUTHORITY_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_CHECK_JOIN_AUTHORITY_ERQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}