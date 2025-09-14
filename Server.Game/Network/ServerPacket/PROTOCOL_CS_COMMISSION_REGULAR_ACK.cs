using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_COMMISSION_REGULAR_ACK : GameServerPacket
    {
        private readonly uint result;
        public PROTOCOL_CS_COMMISSION_REGULAR_ACK(uint result)
        {
            this.result = result;
        }
        public override void Write()
        {
            WriteH(1864);
            WriteD(result);
        }
    }
}