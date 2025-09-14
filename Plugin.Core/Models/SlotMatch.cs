using Plugin.Core.Enums;

namespace Plugin.Core.Models
{
    public class SlotMatch
    {
        public SlotMatchState State;
        public long PlayerId;
        public int Id;
        public SlotMatch(int Id)
        {
            this.Id = Id;
        }
    }
}