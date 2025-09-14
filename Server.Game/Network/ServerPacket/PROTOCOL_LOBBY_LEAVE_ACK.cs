using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_LEAVE_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_LOBBY_LEAVE_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(3076);
            WriteD(Error);
        }
    }
}