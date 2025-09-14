namespace Plugin.Core.Models
{
    public class VisitBoxModel
    {
        public VisitItemModel Reward1, Reward2;
        public int RewardCount;
        public VisitBoxModel()
        {
            Reward1 = new VisitItemModel();
            Reward2 = new VisitItemModel();
        }
        public void SetCount()
        {
            if (Reward1 != null && Reward1.GoodId > 0)
            {
                RewardCount++;
            }
            if (Reward2 != null && Reward2.GoodId > 0)
            {
                RewardCount++;
            }
        }
    }
}