using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class RankModel
    {
        public int Id;
        public string Title;
        public int OnNextLevel;
        public int OnGoldUp;
        public int OnAllExp;
        public SortedList<int, List<ItemsModel>> Rewards = new SortedList<int, List<ItemsModel>>();
        public RankModel(int Id)
        {
            this.Id = Id;
        }
    }
}