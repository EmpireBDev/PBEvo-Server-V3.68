namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_STEPUP_MODE_INFO_ACK : AuthServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BASE_STEPUP_MODE_INFO_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(692);
            WriteD(Error);
        }
    }
}
