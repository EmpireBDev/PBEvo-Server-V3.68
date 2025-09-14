namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_SEASON_CHALLENGE_SEASON_CHANGE : AuthServerPacket
    {
        public PROTOCOL_SEASON_CHALLENGE_SEASON_CHANGE()
        {
        }
        public override void Write()
        {
            WriteH(8452);
            WriteH(0);
            WriteC(1);
            WriteC(1);
        }
    }
}