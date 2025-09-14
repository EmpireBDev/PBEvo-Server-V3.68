using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK : GameServerPacket
    {
        private readonly uint erro;
        private readonly ClanModel c;
        private readonly Account leader;
        private readonly int players;
        public PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(uint erro, ClanModel c)
        {
            this.erro = erro;
            this.c = c;
            if (this.c != null)
            {
                players = DaoManagerSQL.GetClanPlayers(c.Id);
                leader = AccountManager.GetAccount(c.OwnerId, 31);
                if (leader == null)
                {
                    this.erro = 0x80000000;
                }
            }
        }
        public PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1570);
            WriteD(erro);
            if (erro == 0)
            {
                WriteD(c.Id);
                WriteS(c.Name, 17);
                WriteC((byte)c.Rank);
                WriteC((byte)players);
                WriteC((byte)c.MaxPlayers);
                WriteD(c.CreationDate);
                WriteD(c.Logo);
                WriteC((byte)c.NameColor);
                WriteC((byte)c.GetClanUnit(players));
                WriteD(c.Exp);
                WriteD(0);
                WriteQ(c.OwnerId);
                WriteS(leader.Nickname, 33);
                WriteC((byte)leader.Rank);
                WriteS("", 255);
            }
        }
    }
}