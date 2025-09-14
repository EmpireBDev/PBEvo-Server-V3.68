namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_ENTER_PASS_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BASE_ENTER_PASS_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(610);
            WriteH(0);
            WriteD(Error);
        }
    }
}
