using System;

namespace Plugin.Core.Models
{
    public class PlayerStatistic
    {
        public long OwnerId;
        public StatBasic Basic;
        public StatSeason Season;
        public StatDaily Daily;
        public StatClan Clan;
        public StatWeapon Weapon;
        public StatAcemode Acemode;
        public StatBattleroyale Battleroyale;
        public int GetKDRatio()
        {
            if (Basic.HeadshotsCount <= 0 && Basic.KillsCount <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((Basic.KillsCount * 100 + 0.5) / (Basic.KillsCount + Basic.DeathsCount));
        }
        public int GetHSRatio()
        {
            if (Basic.KillsCount <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((Basic.HeadshotsCount * 100) / (double)(Basic.KillsCount + 0.5));
        }
        public int GetSeasonKDRatio()
        {
            if (Season.HeadshotsCount <= 0 && Season.KillsCount <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((Season.KillsCount * 100 + 0.5) / (Season.KillsCount + Season.DeathsCount));
        }
        public int GetSeasonHSRatio()
        {
            if (Season.KillsCount <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((Season.HeadshotsCount * 100) / (double)(Season.KillsCount + 0.5));
        }
        public int GetBRWinRatio()
        {
            if (Battleroyale.MatchWins <= 0 && Battleroyale.Matches <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((Battleroyale.MatchWins * 100 + 0.5) / (Battleroyale.MatchWins + Battleroyale.MatchLoses));
        }
        public int GetBRKDRatio()
        {
            if (Battleroyale.HeadshotsCount <= 0 && Battleroyale.KillsCount <= 0)
            {
                return 0;
            }
            return (int)Math.Floor((Battleroyale.KillsCount * 100 + 0.5) / (Battleroyale.KillsCount + Battleroyale.DeathsCount));
        }
    }
}