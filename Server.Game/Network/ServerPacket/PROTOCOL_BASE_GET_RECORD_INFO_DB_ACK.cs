using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_RECORD_INFO_DB_ACK : GameServerPacket
    {
        private readonly PlayerStatistic Stats;
        public PROTOCOL_BASE_GET_RECORD_INFO_DB_ACK(Account Player)
        {
            Stats = Player.Statistic;
        }
        public override void Write()
        {
            WriteH(559);
            WriteD(Stats.Season.Matches);
            WriteD(Stats.Season.MatchWins);
            WriteD(Stats.Season.MatchLoses);
            WriteD(Stats.Season.MatchDraws);
            WriteD(Stats.Season.KillsCount);
            WriteD(Stats.Season.HeadshotsCount);
            WriteD(Stats.Season.DeathsCount);
            WriteD(Stats.Season.TotalMatchesCount);
            WriteD(Stats.Season.TotalKillsCount);
            WriteD(Stats.Season.EscapesCount);
            WriteD(Stats.Season.AssistsCount);
            WriteD(Stats.Season.MvpCount);
            WriteD(Stats.Basic.Matches);
            WriteD(Stats.Basic.MatchWins);
            WriteD(Stats.Basic.MatchLoses);
            WriteD(Stats.Basic.MatchDraws);
            WriteD(Stats.Basic.KillsCount);
            WriteD(Stats.Basic.HeadshotsCount);
            WriteD(Stats.Basic.DeathsCount);
            WriteD(Stats.Basic.TotalMatchesCount);
            WriteD(Stats.Basic.TotalKillsCount);
            WriteD(Stats.Basic.EscapesCount);
            WriteD(Stats.Basic.AssistsCount);
            WriteD(Stats.Basic.MvpCount);
        }
    }
}