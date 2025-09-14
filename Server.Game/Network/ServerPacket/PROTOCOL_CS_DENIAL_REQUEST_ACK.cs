using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_DENIAL_REQUEST_ACK : GameServerPacket
    {
        private readonly int result;
        public PROTOCOL_CS_DENIAL_REQUEST_ACK(int result)
        {
            this.result = result;
        }
        public override void Write()
        {
            WriteH(1850);
            WriteD(result);
        }
    }
}
