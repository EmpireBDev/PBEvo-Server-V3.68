using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class VoteKickModel
    {
        public int CreatorIdx, VictimIdx, Motive, Accept = 1, Denie = 1, Allies, Enemies;
        public List<int> Votes = new List<int>();
        public bool[] TotalArray = new bool[16];
        public VoteKickModel(int creator, int victim)
        {
            CreatorIdx = creator;
            VictimIdx = victim;
            Votes.Add(creator);
            Votes.Add(victim);
        }
        public int GetInGamePlayers()
        {
            int Count = 0;
            for (int i = 0; i < 16; i++)
            {
                if (TotalArray[i])
                {
                    Count++;
                }
            }
            return Count;
        }
    }
}