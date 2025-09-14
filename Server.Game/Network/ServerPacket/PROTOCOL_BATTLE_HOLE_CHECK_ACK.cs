using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_HOLE_CHECK_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BATTLE_HOLE_CHECK_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(4098);
            WriteD(Error);
        }
    }
}