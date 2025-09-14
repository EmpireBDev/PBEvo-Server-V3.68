namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_MATCH_SERVER_IDX_ACK : AuthServerPacket
    {
        private readonly short ServerId;
        public PROTOCOL_MATCH_SERVER_IDX_ACK(short ServerId)
        {
            this.ServerId = ServerId;
        }
        public override void Write()
        {
            WriteH(7682);
            WriteH(0);
        }
    }
}
