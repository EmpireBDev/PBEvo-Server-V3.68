namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_LOGIN_WAIT_ACK : AuthServerPacket
    {
        private readonly int Error;
        public PROTOCOL_BASE_LOGIN_WAIT_ACK(int Errror)
        {
            this.Error = Errror;
        }
        public override void Write()
        {
            WriteH(521);
            WriteC(3);
            WriteH(67);
            WriteD(Error);
        }
    }
}
