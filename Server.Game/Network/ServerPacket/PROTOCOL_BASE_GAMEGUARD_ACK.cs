namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GAMEGUARD_ACK : GameServerPacket
    {
        private readonly int Error;
        private readonly byte[] Version;
        public PROTOCOL_BASE_GAMEGUARD_ACK(int Error, byte[] Version)
        {
            this.Error = Error;
            this.Version = Version;
        }
        public override void Write()
        {
            WriteH(518);
            WriteB(Version);
            WriteD(Error);
        }
    }
}