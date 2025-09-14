namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_NEW_MYINFO_ACK : GameServerPacket
    {
        private readonly long PlayerId;
        public PROTOCOL_LOBBY_NEW_MYINFO_ACK(long PlayerId)
        {
            this.PlayerId = PlayerId;
        }
        public override void Write()
        {
            WriteH(2001);
            WriteD(0);
            WriteQ(PlayerId);
            WriteB(new byte[132]);
        }
    }
}
