namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_TITLE_CHANGE_ACK : GameServerPacket
    {
        private readonly int Slots;
        private readonly uint Error;
        public PROTOCOL_BASE_USER_TITLE_CHANGE_ACK(uint Error, int Slots)
        {
            this.Error = Error;
            this.Slots = Slots;
        }
        public override void Write()
        {
            WriteH(585);
            WriteD(Error);
            WriteD(Slots);
        }
    }
}
