namespace Plugin.Core.Models
{
    public class VisitItemModel
    {
        public int GoodId;
        public bool IsReward;
        public VisitItemModel()
        {
        }
        public void SetGoodId(string text)
        {
            GoodId = int.Parse(text);
            if (GoodId > 0)
            {
                IsReward = true;
            }
        }
    }
}
