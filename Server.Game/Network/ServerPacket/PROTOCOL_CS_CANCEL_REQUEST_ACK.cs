using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CANCEL_REQUEST_ACK : GameServerPacket
    {
        private readonly uint error;
        public PROTOCOL_CS_CANCEL_REQUEST_ACK(uint error)
        {
            this.error = error;
        }
        public override void Write()
        {
            WriteH(1839);
            WriteD(error);
        }
    }
}