using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_FRIEND_ACCEPT_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_AUTH_FRIEND_ACCEPT_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(793);
            WriteD(Error);
        }
    }
}