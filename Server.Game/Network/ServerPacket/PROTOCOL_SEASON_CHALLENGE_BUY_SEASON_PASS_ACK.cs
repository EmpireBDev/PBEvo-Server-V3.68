namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SEASON_CHALLENGE_BUY_SEASON_PASS_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_SEASON_CHALLENGE_BUY_SEASON_PASS_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(8454);
            WriteH(0);
            WriteD(Error);
        }
    }
}