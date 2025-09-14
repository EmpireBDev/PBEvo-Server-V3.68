using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_ACCOUNT_KICK_ACK : GameServerPacket
    {
        private readonly int type;
        public PROTOCOL_AUTH_ACCOUNT_KICK_ACK(int type)
        {
            this.type = type;
        }
        public override void Write()
        {
            WriteH(965);
            WriteC((byte)type);
        }
    }
}