using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_ENTER_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_LOBBY_ENTER_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(3074);
            WriteH(0);
            WriteD(Error);
            WriteC(0);
            WriteQ(0);
        }
    }
}