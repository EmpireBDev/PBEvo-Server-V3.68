using Plugin.Core.Utility;

namespace Plugin.Core.Models
{
    public class ClanBestPlayers
    {
        public RecordInfo Exp, Participation, Wins, Kills, Headshots;
        public void SetPlayers(string Exp, string Participation, string Wins, string Kills, string Headshots)
        {
            string[] expSplit = Exp.Split('-'), partSplit = Participation.Split('-'), winsSplit = Wins.Split('-'), killsSplit = Kills.Split('-'), hsSplit = Headshots.Split('-');

            this.Exp = new RecordInfo(expSplit);
            this.Participation = new RecordInfo(partSplit);
            this.Wins = new RecordInfo(winsSplit);
            this.Kills = new RecordInfo(killsSplit);
            this.Headshots = new RecordInfo(hsSplit);
        }
        public void SetDefault()
        {
            string[] split = new string[] { "0", "0" };

            Exp = new RecordInfo(split);
            Participation = new RecordInfo(split);
            Wins = new RecordInfo(split);
            Kills = new RecordInfo(split);
            Headshots = new RecordInfo(split);
        }
        public long GetPlayerId(string[] split)
        {
            try
            {
                return long.Parse(split[0]);
            }
            catch
            {
                return 0;
            }
        }
        public int GetPlayerValue(string[] split)
        {
            try
            {
                return int.Parse(split[1]);
            }
            catch
            {
                return 0;
            }
        }
        public void SetBestExp(SlotModel slot)
        {
            if (slot.Exp <= Exp.RecordValue)
            {
                return;
            }

            Exp.PlayerId = slot.PlayerId;
            Exp.RecordValue = slot.Exp;
        }
        public void SetBestHeadshot(SlotModel slot)
        {
            if (slot.AllHeadshots <= Headshots.RecordValue)
            {
                return;
            }

            Headshots.PlayerId = slot.PlayerId;
            Headshots.RecordValue = slot.AllHeadshots;
        }
        public void SetBestKills(SlotModel slot)
        {
            if (slot.AllKills <= Kills.RecordValue)
            {
                return;
            }

            Kills.PlayerId = slot.PlayerId;
            Kills.RecordValue = slot.AllKills;
        }
        public void SetBestWins(PlayerStatistic Stat, SlotModel Slot, bool WonTheMatch)
        {
            if (!WonTheMatch)
            {
                return;
            }
            ComDiv.UpdateDB("player_stat_clans", "clan_match_wins", ++Stat.Clan.MatchWins, "owner_id", Slot.PlayerId);
            if (Stat.Clan.MatchWins <= Wins.RecordValue)
            {
                return;
            }
            Wins.PlayerId = Slot.PlayerId;
            Wins.RecordValue = Stat.Clan.MatchWins;
        }
        public void SetBestParticipation(PlayerStatistic Stat, SlotModel Slot)
        {
            ComDiv.UpdateDB("player_stat_clans", "clan_matches", ++Stat.Clan.Matches, "owner_id", Slot.PlayerId);
            if (Stat.Clan.Matches <= Participation.RecordValue)
            {
                return;
            }

            Participation.PlayerId = Slot.PlayerId;
            Participation.RecordValue = Stat.Clan.Matches;
        }
    }
}