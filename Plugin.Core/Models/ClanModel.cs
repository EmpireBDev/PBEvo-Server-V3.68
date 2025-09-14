using Plugin.Core.SQL;

namespace Plugin.Core.Models
{
    public class ClanModel
    {
        public int Id, Matches, MatchWins, MatchLoses, Authority, RankLimit, MinAgeLimit, MaxAgeLimit, Exp, Rank, NameColor, MaxPlayers, Effect;
        public string Name, Info, News;
        public long OwnerId;
        public uint Logo, CreationDate;
        public float Points;
        public ClanBestPlayers BestPlayers = new ClanBestPlayers();
        public ClanModel()
        {
            MaxPlayers = 50;
            Logo = 4294967295;
            Name = "";
            Info = "";
            News = "";
            Points = 1000;
        }
        public int GetClanUnit()
        {
            return GetClanUnit(DaoManagerSQL.GetClanPlayers(Id));
        }
        public int GetClanUnit(int count)
        {
            //Possível 8 - "Top"
            if (count >= 250) return 7; //Corpo
            else if (count >= 200) return 6; //Divisão
            else if (count >= 150) return 5; //Brigada
            else if (count >= 100) return 4; //Regimento
            else if (count >= 50) return 3; //Batalhão
            else if (count >= 30) return 2; //Companhia
            else if (count >= 10) return 1; //Pelotão
            else return 0; //Esquadra
        }
    }
}