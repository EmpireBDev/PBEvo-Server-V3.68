using Plugin.Core.Enums;
using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class TicketModel
    {
        public string Token;
        public TicketType Type;
        public int GoldReward, CashReward, TagsReward;
        public uint TicketCount, PlayerRation;
        public List<int> Rewards = new List<int>();
        public TicketModel(TicketType Type, string Token, uint TicketCount, uint PlayerRation)
        {
            this.Type = Type;
            this.Token = Token;
            this.TicketCount = TicketCount;
            this.PlayerRation = PlayerRation;
        }
    }
}
