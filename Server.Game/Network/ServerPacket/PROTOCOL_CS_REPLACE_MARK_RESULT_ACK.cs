using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_MARK_RESULT_ACK : GameServerPacket
    {
        private readonly uint Logo;
        public PROTOCOL_CS_REPLACE_MARK_RESULT_ACK(uint Logo)
        {
            this.Logo = Logo;
        }
        public override void Write()
        {
            WriteH(1891);
            WriteD(Logo);
        }
    }
}