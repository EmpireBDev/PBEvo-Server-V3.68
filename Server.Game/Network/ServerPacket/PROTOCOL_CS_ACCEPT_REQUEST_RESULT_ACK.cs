using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_ACCEPT_REQUEST_RESULT_ACK : GameServerPacket
    {
        private readonly ClanModel clan;
        private readonly Account p;
        private readonly int clanPlayers;
        public PROTOCOL_CS_ACCEPT_REQUEST_RESULT_ACK(ClanModel clan, Account p, int clanPlayers)
        {
            this.clan = clan;
            this.p = p;
            this.clanPlayers = clanPlayers;
        }
        public PROTOCOL_CS_ACCEPT_REQUEST_RESULT_ACK(ClanModel clan, int clanPlayers)
        {
            this.clan = clan;
            p = AccountManager.GetAccount(clan.OwnerId, 31);
            this.clanPlayers = clanPlayers;
        }
        public override void Write()
        {
            WriteH(1848);
            WriteD(clan.Id);
            WriteU(clan.Name, 34);
            WriteC((byte)clan.Rank);
            WriteC((byte)clanPlayers);
            WriteC((byte)clan.MaxPlayers);
            WriteD(clan.CreationDate);
            WriteD(clan.Logo);
            WriteC((byte)clan.NameColor);
            WriteC((byte)clan.Effect);
            WriteC((byte)clan.GetClanUnit());
            WriteD(clan.Exp);
            WriteD(10);
            WriteQ(clan.OwnerId);
            WriteU(p != null ? p.Nickname : "", 66);
            WriteC((byte)(p != null ? p.NickColor : 0));
            WriteC((byte)(p != null ? p.Rank : 0));
            WriteU(clan.Info, 510);
            WriteU("Temp", 42);
            WriteC((byte)clan.RankLimit);
            WriteC((byte)clan.MinAgeLimit);
            WriteC((byte)clan.MaxAgeLimit);
            WriteC((byte)clan.Authority);
            WriteU(clan.News, 510);
            WriteD(clan.Matches);
            WriteD(clan.MatchWins);
            WriteD(clan.MatchLoses);
            WriteD(clan.Matches);
            WriteD(clan.MatchWins);
            WriteD(clan.MatchLoses);
            /*
            WriteD(0); // ?

            // Old Ranking
            WriteD(clan.Matches); // Fights
            WriteD(clan.MatchWins); // Win
            WriteD(clan.MatchLoses); // Lose
            WriteD(0);
            WriteF(clan.Points);
            WriteF(60); // ?

            // Last Season
            WriteD(clan.Matches); // Fights
            WriteD(clan.MatchWins); // Win
            WriteD(clan.MatchLoses); // Lose
            WriteD(0); // ?
            WriteF(clan.Points);
            WriteF(60); // ?

            WriteQ(clan.BestPlayers.Exp.PlayerId);
            WriteQ(clan.BestPlayers.Exp.PlayerId);
            WriteQ(clan.BestPlayers.Wins.PlayerId);
            WriteQ(clan.BestPlayers.Wins.PlayerId);
            WriteQ(clan.BestPlayers.Kills.PlayerId);
            WriteQ(clan.BestPlayers.Kills.PlayerId);
            WriteQ(clan.BestPlayers.Headshots.PlayerId);
            WriteQ(clan.BestPlayers.Headshots.PlayerId);
            WriteQ(clan.BestPlayers.Participation.PlayerId);
            WriteQ(clan.BestPlayers.Participation.PlayerId);
            WriteB(new byte[66]);
            */
        }
    }
}