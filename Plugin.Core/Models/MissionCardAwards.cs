namespace Plugin.Core.Models
{
    public class MissionCardAwards
    {
        public int Id, Card, Ensign, Medal, Ribbon, Exp, Gold;
        public MissionCardAwards()
        {
        }
        public bool Unusable()
        {
            return (Ensign == 0 && Medal == 0 && Ribbon == 0 && Exp == 0 && Gold == 0);
        }
    }
}
