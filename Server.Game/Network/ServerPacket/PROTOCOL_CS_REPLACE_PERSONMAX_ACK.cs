using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_PERSONMAX_ACK : GameServerPacket
    {
        private readonly int Max;
        public PROTOCOL_CS_REPLACE_PERSONMAX_ACK(int Max)
        {
            this.Max = Max;
        }
        public override void Write()
        {
            WriteH(1897);
            WriteD(0); // ?
            WriteC((byte)Max);
        }
    }
}