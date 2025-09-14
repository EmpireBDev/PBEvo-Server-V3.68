namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_UNKNOWN_PACKET_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_UNKNOWN_PACKET_REQ(AuthClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
        }
    }
}
