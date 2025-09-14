namespace Plugin.Core.Models
{
    public class MapMatch
    {
        public int Mode, Id, Limit, Tag;
        public string Name;
        public MapMatch(int Mode)
        {
            this.Mode = Mode;
        }
    }
}
