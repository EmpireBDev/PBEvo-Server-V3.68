namespace Plugin.Core.Models
{
    public class InternetCafe
    {
        public int ConfigId, BasicExp, BasicGold, PremiumExp, PremiumGold;
        public InternetCafe(int ConfigId)
        {
            this.ConfigId = ConfigId;
        }
    }
}
