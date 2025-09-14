namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_COUNT_DOWN_ACK : GameServerPacket
    {
        private readonly int Seconds;
        public PROTOCOL_BATTLE_COUNT_DOWN_ACK(int Seconds)
        {
            this.Seconds = Seconds;
        }
        public override void Write()
        {
            WriteH(4251);
            WriteC((byte)Seconds);
        }
    }
}
