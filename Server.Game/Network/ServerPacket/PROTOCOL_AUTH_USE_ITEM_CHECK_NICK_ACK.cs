using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1062);
            WriteD(Error);
        }
    }
}