using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_MYINFO_BASIC_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly ClanModel Clan;
        public PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Clan = ClanManager.GetClan(Player.ClanId);
            }
        }
        public override void Write()
        {
            WriteH(579);
            WriteU(Player.Nickname, 66);
            WriteD(Player.GetRank());
            WriteD(Player.GetRank());
            WriteD(Player.Gold);
            WriteD(Player.Exp);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteC(0);
            WriteH((ushort)(Player.HavePermission("observer_enabled") ? 11111 : 0));
            WriteD(Player.Tags);
            WriteD(Player.Cash);
            WriteD(Clan.Id);
            WriteD(Player.ClanAccess);
            WriteQ(Player.StatusId());
            WriteC((byte)Player.CafePC);
            WriteC((byte)Player.TourneyLevel);
            WriteU(Clan.Name, 34);
            WriteC((byte)Clan.Rank);
            WriteC((byte)Clan.GetClanUnit());
            WriteD(Clan.Logo);
            WriteC((byte)Clan.NameColor);
            WriteC((byte)Clan.Effect);
            WriteD(Player.Statistic.Season.Matches);
            WriteD(Player.Statistic.Season.MatchWins);
            WriteD(Player.Statistic.Season.MatchLoses);
            WriteD(Player.Statistic.Season.MatchDraws);
            WriteD(Player.Statistic.Season.KillsCount);
            WriteD(Player.Statistic.Season.HeadshotsCount);
            WriteD(Player.Statistic.Season.DeathsCount);
            WriteD(Player.Statistic.Season.TotalMatchesCount);
            WriteD(Player.Statistic.Season.TotalKillsCount);
            WriteD(Player.Statistic.Season.EscapesCount);
            WriteD(Player.Statistic.Season.AssistsCount);
            WriteD(Player.Statistic.Season.MvpCount);
            WriteD(Player.Statistic.Basic.Matches);
            WriteD(Player.Statistic.Basic.MatchWins);
            WriteD(Player.Statistic.Basic.MatchLoses);
            WriteD(Player.Statistic.Basic.MatchDraws);
            WriteD(Player.Statistic.Basic.KillsCount);
            WriteD(Player.Statistic.Basic.HeadshotsCount);
            WriteD(Player.Statistic.Basic.DeathsCount);
            WriteD(Player.Statistic.Basic.TotalMatchesCount);
            WriteD(Player.Statistic.Basic.TotalKillsCount);
            WriteD(Player.Statistic.Basic.EscapesCount);
            WriteD(Player.Statistic.Basic.AssistsCount);
            WriteD(Player.Statistic.Basic.MvpCount);
        }
    }
}