using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class BattleBoxModel
    {
        public int Id, Tags;
        public string Name;
        public List<BattleBoxItem> Items;
    }
}
