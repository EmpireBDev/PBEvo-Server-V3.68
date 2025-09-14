using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_ENTER_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BASE_USER_ENTER_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(539);
            WriteD(Error);
        }
    }
}