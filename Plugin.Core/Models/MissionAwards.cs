namespace Plugin.Core.Models
{
    public class MissionAwards
    {
        public int Id, MasterMedal, Exp, Gold;
        public MissionAwards(int Id, int MasterMedal, int Exp, int Gold)
        {
            this.Id = Id;
            this.MasterMedal = MasterMedal;
            this.Exp = Exp;
            this.Gold = Gold;
        }
    }
}
