namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_CHANNELTYPE_CONDITION_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_CHANNELTYPE_CONDITION_ACK()
        {
        }
        public override void Write()
        {
            WriteH(694);
            WriteB(new byte[888]);
        }
    }
}
