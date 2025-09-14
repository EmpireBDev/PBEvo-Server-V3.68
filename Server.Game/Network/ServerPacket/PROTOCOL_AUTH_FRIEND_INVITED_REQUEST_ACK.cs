using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_FRIEND_INVITED_REQUEST_ACK : GameServerPacket
    {
        private readonly int Index;
        public PROTOCOL_AUTH_FRIEND_INVITED_REQUEST_ACK(int Index)
        {
            this.Index = Index;
        }
        public override void Write()
        {
            WriteH(789);
            WriteC((byte)Index);
        }
    }
}