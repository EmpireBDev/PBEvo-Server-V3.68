namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_LEAVE_ACK : AuthServerPacket
    {
        private readonly int erro;
        public PROTOCOL_BASE_USER_LEAVE_ACK(int erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(537);
            WriteD(erro);
            WriteH(0);
        }
    }
}