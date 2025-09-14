using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class RandomBoxModel
    {
        public int ItemsCount, MinPercent, MaxPercent;
        public List<RandomBoxItem> Items = new List<RandomBoxItem>();
        public RandomBoxModel()
        {
        }
        public List<RandomBoxItem> GetRewardList(List<RandomBoxItem> SortedLists, int RandomId)
        {
            List<RandomBoxItem> Results = new List<RandomBoxItem>();
            if (SortedLists.Count > 0)
            {
                int SortedIndex = SortedLists[RandomId].Index;
                foreach (RandomBoxItem Item in SortedLists)
                {
                    if (Item.Index == SortedIndex)
                    {
                        Results.Add(Item);
                    }
                }
            }
            return Results;
        }
        public List<RandomBoxItem> GetSortedList(int Percent)
        {
            if (Percent < MinPercent)
            {
                Percent = MinPercent;
            }
            List<RandomBoxItem> Results = new List<RandomBoxItem>();
            foreach (RandomBoxItem Item in Items)
            {
                if (Percent <= Item.Percent)
                {
                    Results.Add(Item);
                }
            }
            return Results;
        }
        public void SetTopPercent()
        {
            int MinRecord = 100, MaxRecord = 0;
            foreach (RandomBoxItem Item in Items)
            {
                if (Item.Percent < MinRecord)
                {
                    MinRecord = Item.Percent;
                }
                if (Item.Percent > MaxRecord)
                {
                    MaxRecord = Item.Percent;
                }
            }
            MinPercent = MinRecord;
            MaxPercent = MaxRecord;
        }
    }
}
