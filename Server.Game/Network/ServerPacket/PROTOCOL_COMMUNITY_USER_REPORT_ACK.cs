namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_COMMUNITY_USER_REPORT_ACK : GameServerPacket
    {
        private readonly uint Answer;
        public PROTOCOL_COMMUNITY_USER_REPORT_ACK(uint Answer)
        {
            this.Answer = Answer;
        }
        public override void Write()
        {
            WriteD(2827);
            WriteD(Answer);
        }
    }
}