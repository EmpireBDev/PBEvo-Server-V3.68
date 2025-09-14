namespace Plugin.Core.Models
{
    public class TitleModel
    {
        public int Id, ClassId, Medal, Ribbon, MasterMedal, Ensign, Rank, Slot, Req1, Req2;
        public long Flag;
        public TitleModel()
        {
        }
        public TitleModel(int titleId)
        {
            Id = titleId;
            Flag = ((long)1 << titleId);
        }
    }
}
