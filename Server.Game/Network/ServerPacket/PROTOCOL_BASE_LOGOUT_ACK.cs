using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_LOGOUT_ACK : GameServerPacket
    {
        public PROTOCOL_BASE_LOGOUT_ACK()
        {
        }
        public override void Write()
        {
            WriteH(516);
        }
    }
}