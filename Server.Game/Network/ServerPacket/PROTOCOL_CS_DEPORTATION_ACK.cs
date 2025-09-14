using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_DEPORTATION_ACK : GameServerPacket
    {
        private readonly uint result;
        public PROTOCOL_CS_DEPORTATION_ACK(uint result)
        {
            this.result = result;
        }
        public override void Write()
        {
            WriteH(1855);
            WriteD(result);
        }
    }
}