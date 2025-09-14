using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_ACCEPT_REQUEST_ACK : GameServerPacket
    {
        private readonly uint result;
        public PROTOCOL_CS_ACCEPT_REQUEST_ACK(uint result)
        {
            this.result = result;
        }
        public override void Write()
        {
            WriteH(1847);
            WriteD(result);
        }
    }
}