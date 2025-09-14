using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_ACK : GameServerPacket
    {
        private readonly ClanModel Clan;
        private readonly int Error;
        private readonly Account Player;
        private readonly int Players;
        public PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_ACK(int Error, ClanModel Clan)
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
            WriteH(925);
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
            WriteC((byte)Clan.GetClanUnit());
            WriteD(Clan.Exp);
            WriteD(10);
            WriteQ(Clan.OwnerId);
            WriteU(Player != null ? Player.Nickname : "", 66);
            WriteC((byte)(Player != null ? Player.NickColor : 0));
            WriteC((byte)(Player != null ? Player.Rank : 0));
            WriteU(Clan.Info, 510);
            WriteU("Temp", 42);
            WriteC((byte)Clan.RankLimit);
            WriteC((byte)Clan.MinAgeLimit);
            WriteC((byte)Clan.MaxAgeLimit);
            WriteC((byte)Clan.Authority);
            WriteU(Clan.News, 510);
            WriteD(Clan.Matches);
            WriteD(Clan.MatchWins);
            WriteD(Clan.MatchLoses);
            WriteD(Clan.Matches);
            WriteD(Clan.MatchWins);
            WriteD(Clan.MatchLoses);

            WriteD(0); // ?

            // Old Ranking
            WriteD(Clan.Matches); // Fights
            WriteD(Clan.MatchWins); // Win
            WriteD(Clan.MatchLoses); // Lose
            WriteD(0);
            WriteC(0);
            WriteF(Clan.Points);
            WriteC(0);
            WriteH(0); // ?

            // Last Season
            WriteD(Clan.Matches); // Fights
            WriteD(Clan.MatchWins); // Win
            WriteD(Clan.MatchLoses); // Lose
            WriteD(0); // ?
            WriteD(0); // ?
            WriteC(0);
            WriteF(Clan.Points);
            WriteC(0);
            WriteH(0); // ?

            WriteQ(Clan.BestPlayers.Exp.PlayerId);
            WriteQ(Clan.BestPlayers.Exp.PlayerId);
            WriteQ(Clan.BestPlayers.Wins.PlayerId);
            WriteQ(Clan.BestPlayers.Wins.PlayerId);
            WriteQ(Clan.BestPlayers.Kills.PlayerId);
            WriteQ(Clan.BestPlayers.Kills.PlayerId);
            WriteQ(Clan.BestPlayers.Headshots.PlayerId);
            WriteQ(Clan.BestPlayers.Headshots.PlayerId);
            WriteQ(Clan.BestPlayers.Participation.PlayerId);
            WriteQ(Clan.BestPlayers.Participation.PlayerId);
            WriteB(new byte[34]);
        }
    }
}
