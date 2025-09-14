namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_SENDPING_ACK : GameServerPacket
    {
        private readonly byte[] Pings;
        public PROTOCOL_BATTLE_SENDPING_ACK(byte[] Pings)
        {
            this.Pings = Pings;
        }
        public override void Write()
        {
            WriteH(4123);
            WriteB(Pings);
        }
    }
}