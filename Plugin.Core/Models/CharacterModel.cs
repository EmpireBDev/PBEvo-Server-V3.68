using Plugin.Core.Utility;

namespace Plugin.Core.Models
{
    public class CharacterModel
    {
        public long ObjectId;
        public int Id;
        public int Slot;
        public string Name;
        public uint CreateDate;
        public uint PlayTime;
        public CharacterModel()
        {
            Name = "";
        }
    }
}
