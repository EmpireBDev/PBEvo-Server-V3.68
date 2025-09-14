using Plugin.Core.Enums;

namespace Server.Auth.Data.Models
{
    public class ChannelModel
    {
        public int Id;
        public ChannelType Type;
        public int ServerId;
        public int TotalPlayers;
        public int MaxRooms;
        public int ExpBonus;
        public int GoldBonus;
        public int CashBonus;
        public string Password;
        public ChannelModel(int ServerId)
        {
            this.ServerId = ServerId;
        }
    }
}