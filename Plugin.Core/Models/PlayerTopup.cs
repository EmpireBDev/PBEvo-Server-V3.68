namespace Plugin.Core.Models
{
    public class PlayerTopup
    {
        public long ObjectId;
        public long PlayerId;
        public int ItemId;
        public uint Count;
        public int Equip;
        public string ItemName;
    }
}
