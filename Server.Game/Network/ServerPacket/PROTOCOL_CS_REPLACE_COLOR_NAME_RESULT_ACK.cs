using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_COLOR_NAME_RESULT_ACK : GameServerPacket
    {
        private readonly int color;
        public PROTOCOL_CS_REPLACE_COLOR_NAME_RESULT_ACK(int color)
        {
            this.color = color;
        }
        public override void Write()
        {
            WriteH(1926);
            WriteC((byte)color);
        }
    }
}