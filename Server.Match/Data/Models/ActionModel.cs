using Server.Match.Data.Enums;

namespace Server.Match.Data.Models
{
    public class ActionModel
    {
        public ushort Slot, Length;
        public UdpGameEvent Flag;
        public UdpSubHead SubHead;
        public byte[] Data;
    }
}