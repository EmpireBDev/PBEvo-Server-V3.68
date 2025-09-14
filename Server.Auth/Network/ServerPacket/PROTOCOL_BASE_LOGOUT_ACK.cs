namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_LOGOUT_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_LOGOUT_ACK()
        {
        }
        public override void Write()
        {
            WriteH(516);
        }
    }
}