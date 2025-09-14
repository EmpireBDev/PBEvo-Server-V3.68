using Plugin.Core.Enums;

namespace Plugin.Core.Models
{
    public class SChannelModel
    {
        public bool State;
        public int Id;
        public int LastPlayers;
        public int MaxPlayers;
        public int ChannelPlayers;
        public SChannelType Type;
        public string Host;
        public ushort Port;
        public bool IsMobile;
        public SChannelModel(string Host, ushort Port)
        {
            this.Host = Host;
            this.Port = Port;
        }
    }
}