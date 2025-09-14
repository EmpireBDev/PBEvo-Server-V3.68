namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_USER_EVENT_START_ACK : AuthServerPacket
    {
        public PROTOCOL_SERVER_MESSAGE_USER_EVENT_START_ACK()
        {
        }
        public override void Write()
        {
            WriteH(2574);
        }
    }
}