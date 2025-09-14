using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.SQL;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CREATE_CLAN_ACK : GameServerPacket
    {
        private readonly Account player;
        private readonly ClanModel clan;
        private readonly uint erro;
        public PROTOCOL_CS_CREATE_CLAN_ACK(uint erro, ClanModel clan, Account player)
        {
            this.erro = erro;
            this.clan = clan;
            this.player = player;
        }
        public override void Write()
        {
            WriteH(1831);
            WriteD(erro);
            if (erro == 0)
            {
                WriteD(clan.Id);
                WriteU(clan.Name, 34);
                WriteC((byte)clan.Rank);
                WriteC((byte)DaoManagerSQL.GetClanPlayers(clan.Id));
                WriteC((byte)clan.MaxPlayers);
                WriteD(clan.CreationDate);
                WriteD(clan.Logo);
                WriteB(new byte[11]);
                WriteQ(clan.OwnerId);
                WriteS(player.Nickname, 66);
                WriteC((byte)player.NickColor);
                WriteC((byte)player.Rank);
                WriteU(clan.Info, 510);
                WriteU("Temp", 42);
                WriteC((byte)clan.RankLimit);
                WriteC((byte)clan.MinAgeLimit);
                WriteC((byte)clan.MaxAgeLimit);
                WriteC((byte)clan.Authority);
                WriteU("", 510);
                WriteB(new byte[44]);
                WriteF(clan.Points);
                WriteF(60);
                WriteB(new byte[16]);
                WriteF(clan.Points);
                WriteF(60);
                WriteB(new byte[80]);
                WriteB(new byte[66]);
                WriteD(player.Gold);
            }
        }
    }
}
