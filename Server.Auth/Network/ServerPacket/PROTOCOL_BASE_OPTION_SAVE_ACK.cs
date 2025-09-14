namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_OPTION_SAVE_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_OPTION_SAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(531);
            WriteD(0);
        }
    }
}