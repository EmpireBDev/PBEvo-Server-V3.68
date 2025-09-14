using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_LEAVE_ACK : GameServerPacket
    {
        private readonly int Error;
        public PROTOCOL_BASE_USER_LEAVE_ACK(int Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(537);
            WriteD(Error);
            WriteH(0);
        }
    }
}