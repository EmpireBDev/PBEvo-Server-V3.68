using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REPLACE_MANAGEMENT_REQ : GameClientPacket
    {
        private int RankLimit, MinAge, MaxAge, Authority;
        private uint erro;
        public PROTOCOL_CS_REPLACE_MANAGEMENT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Authority = ReadC();
            RankLimit = ReadC();
            MinAge = ReadC();
            MaxAge = ReadC();
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
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id > 0 && (Clan.OwnerId == Client.PlayerId) && DaoManagerSQL.UpdateClanInfo(Clan.Id, Authority, RankLimit, MinAge, MaxAge))
                {
                    Clan.Authority = Authority;
                    Clan.RankLimit = RankLimit;
                    Clan.MinAgeLimit = MinAge;
                    Clan.MaxAgeLimit = MaxAge;
                }
                else
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_CS_REPLACE_MANAGEMENT_ACK(erro));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}