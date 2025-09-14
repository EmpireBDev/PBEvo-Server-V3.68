namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_COMMUNITY_USER_REPORT_CONDITION_CHECK_ACK : GameServerPacket
    {
        private int ReportLimits;
        public PROTOCOL_COMMUNITY_USER_REPORT_CONDITION_CHECK_ACK(int ReportLimits)
        {
            this.ReportLimits = ReportLimits;
        }
        public override void Write()
        {
            WriteD(2829);
            WriteD(ReportLimits);
        }
    }
}