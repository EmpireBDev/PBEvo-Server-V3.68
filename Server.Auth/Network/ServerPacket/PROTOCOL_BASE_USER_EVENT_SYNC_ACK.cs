namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_EVENT_SYNC_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_USER_EVENT_SYNC_ACK()
        {
        }
        public override void Write()
        {
            WriteH(681);
        }
    }
}
