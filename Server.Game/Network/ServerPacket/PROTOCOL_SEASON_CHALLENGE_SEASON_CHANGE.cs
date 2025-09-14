namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SEASON_CHALLENGE_SEASON_CHANGE : GameServerPacket
    {
        public PROTOCOL_SEASON_CHALLENGE_SEASON_CHANGE()
        {
        }
        public override void Write()
        {
            WriteH(8450);
            WriteH(0);
            WriteC(1);
            WriteC(0);
        }
    }
}