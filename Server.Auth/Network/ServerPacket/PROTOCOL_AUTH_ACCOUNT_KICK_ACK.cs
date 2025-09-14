namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_AUTH_ACCOUNT_KICK_ACK : AuthServerPacket
    {
        private readonly int Type;
        public PROTOCOL_AUTH_ACCOUNT_KICK_ACK(int Type)
        {
            this.Type = Type;
        }
        public override void Write()
        {
            WriteH(965);
            WriteC((byte)Type);
        }
    }
}