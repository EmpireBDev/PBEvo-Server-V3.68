using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_DEPORTATION_RESULT_ACK : GameServerPacket
    {
        public PROTOCOL_CS_DEPORTATION_RESULT_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1856);
        }
    }
}