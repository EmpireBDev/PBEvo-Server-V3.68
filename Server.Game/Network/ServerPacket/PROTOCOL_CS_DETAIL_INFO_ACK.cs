using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_DETAIL_INFO_ACK : GameServerPacket
    {
        private readonly ClanModel Clan;
        private readonly int Error;
        private readonly Account Player;
        private readonly int Players;
        public PROTOCOL_CS_DETAIL_INFO_ACK(int Error, ClanModel Clan)
        {
            this.Error = Error;
            this.Clan = Clan;
            if (Clan != null)
            {
                Player = AccountManager.GetAccount(Clan.OwnerId, 31);
                Players = DaoManagerSQL.GetClanPlayers(Clan.Id);
            }
        }
        public override void Write()
        {
            WriteH(1825);
            WriteD(Error);
            WriteD(Clan.Id);
            WriteU(Clan.Name, 34);
            WriteC((byte)Clan.Rank);
            WriteC((byte)Players);
            WriteC((byte)Clan.MaxPlayers);
            WriteD(Clan.CreationDate);
            WriteD(Clan.Logo);
            WriteC((byte)Clan.NameColor);
            WriteC((byte)Clan.Effect);
            WriteC(0);
            WriteC((byte)Clan.GetClanUnit());
            WriteD(Clan.Exp);
            WriteQ(Clan.OwnerId);
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteU(Player != null ? Player.Nickname : "", 66);
            WriteC((byte)(Player != null ? Player.NickColor : 0));
            WriteC((byte)(Player != null ? Player.Rank : 0));
            WriteU(Clan.Info, 510);
            WriteB(new byte[41]); //Unk
            WriteC(1); //Accept Type (request or free)
            WriteC((byte)Clan.RankLimit);
            WriteC((byte)Clan.MinAgeLimit);
            WriteC((byte)Clan.MaxAgeLimit);
            WriteC((byte)Clan.Authority);
            WriteU(Clan.News, 510);

            WriteD(11111);
            WriteD(22222);
            WriteD(33333);
            WriteD(11111);
            WriteD(22222);
            WriteD(33333);
            WriteD(11111);
            WriteD(22222);
            WriteD(33333);
            WriteD(11111);
            WriteD(22222);
            WriteD(33333);
            WriteD(11111);
            WriteD(22222);
            WriteD(33333);
            WriteD(11111);
            WriteD(22222);
            WriteD(33333);
            WriteD(19);
            WriteD(20);
            WriteD(21);
            WriteD(22);
            WriteD(23);
            WriteD(24);

            //ordinary statistics
            WriteD(6); //clan matches
            WriteD(4); //clan wins
            WriteD(2); //clan loses
            WriteD(44444); //unk
            WriteD(44444); //unk
            WriteD(5); //clan kills
            WriteD(5); //clan assists
            WriteD(5); //clan deaths
            WriteD(5); //clan headshotes
            WriteD(1); //clan escapes

            //rating (current season)
            WriteD(55555); //unk
            WriteD(5); //rating clan medals
            WriteD(9); //rating clan matches
            WriteD(8); //rating clan wins
            WriteD(1); //rating clan loses
            WriteD(5); //rating clan rank
            WriteD(5); //rating clan rank
            WriteD(5); //rating clan kills
            WriteD(5); //rating clan assists
            WriteD(5); //rating clan deathes
            WriteD(5); //rating clan headshots
            WriteD(1); //rating clan escapes

            //rating (previous season)
            WriteD(0); //unk
            WriteD(500); //rating clan medals
            WriteD(301); //rating clan matches
            WriteD(200); //rating clan wins
            WriteD(100); //rating clan loses
            WriteD(30); //rating clan rank
            WriteD(30); //rating clan rank
            WriteD(4000); //rating clan kills
            WriteD(2000); //rating clan assists
            WriteD(1000); //rating clan deathes
            WriteD(3000); //rating clan headshots
            WriteD(1000); //rating clan escapes

            //stage medals
            WriteD(1111); //current week
            WriteD(10000); //previous week
            WriteC(0);
            WriteD(4); //Current stage and rank

            //WriteD(Clan.Matches);
            //WriteD(Clan.MatchWins);
            //WriteD(Clan.MatchLoses);
            //WriteD(Clan.Matches);
            //WriteD(Clan.MatchWins);
            //WriteD(Clan.MatchLoses);

            //WriteD(0); // ?

            //Old Ranking
            //WriteD(Clan.Matches); // Fights
            //WriteD(Clan.MatchWins); // Win
            //WriteD(Clan.MatchLoses); // Lose
            //WriteD(1);
            //WriteC(2);
            //WriteF(Clan.Points);
            //WriteC(3);
            //WriteH(4); // ?

            //Last Season
            //WriteD(Clan.Matches); // Fights
            //WriteD(Clan.MatchWins); // Win
            //WriteD(Clan.MatchLoses); // Lose
            //WriteD(0); // ?
            //WriteD(0); // ?
            //WriteC(0);
            //WriteF(Clan.Points);
            //WriteC(0);
            //WriteH(0); // ?

            //WriteQ(Clan.BestPlayers.Exp.PlayerId);
            //WriteQ(Clan.BestPlayers.Exp.PlayerId);
            //WriteQ(Clan.BestPlayers.Wins.PlayerId);
            //WriteQ(Clan.BestPlayers.Wins.PlayerId);
            //WriteQ(Clan.BestPlayers.Kills.PlayerId);
            //WriteQ(Clan.BestPlayers.Kills.PlayerId);
            //WriteQ(Clan.BestPlayers.Headshots.PlayerId);
            //WriteQ(Clan.BestPlayers.Headshots.PlayerId);
            //WriteQ(Clan.BestPlayers.Participation.PlayerId);
            //WriteQ(Clan.BestPlayers.Participation.PlayerId);

            //WriteD(1);
            //WriteD(2);
            //WriteD(3);
            //WriteD(4);
            //WriteD(5);
            //WriteD(6);
            //WriteD(7);
            //WriteD(8);
            //WriteD(8);
            //WriteD(10);
            //WriteD(11);
            //WriteD(12);
            //WriteD(13);
            //WriteD(14);
            //WriteD(15);
            //WriteD(16);
            //WriteD(17);
            //WriteD(18);
            //WriteD(19);
            //WriteD(20);

            //WriteB(new byte[34]);
        }
    }
}