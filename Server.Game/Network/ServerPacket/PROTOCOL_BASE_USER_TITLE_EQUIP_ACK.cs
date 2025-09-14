namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_TITLE_EQUIP_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly int SlotIdx, TitleId;
        public PROTOCOL_BASE_USER_TITLE_EQUIP_ACK(uint Error, int SlotIdx, int TitleId)
        {
            this.Error = Error;
            this.SlotIdx = SlotIdx;
            this.TitleId = TitleId;
        }
        public override void Write()
        {
            WriteH(587);
            WriteD(Error);
            WriteC((byte)SlotIdx);
            WriteC((byte)TitleId);
        }
    }
}
