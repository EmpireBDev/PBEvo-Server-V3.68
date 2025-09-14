namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_FILTER_CLAN_LIST_ACK : GameServerPacket
    {
        private readonly uint Page;
        private readonly int Count;
        private readonly byte[] Array;
        public PROTOCOL_CS_FILTER_CLAN_LIST_ACK(uint Page, int Count, byte[] Array)
        {
            this.Page = Page;
            this.Count = Count;
            this.Array = Array;
        }
        public override void Write()
        {
            WriteH(2022);
            WriteD(Page);
            WriteH((ushort)Count);
            WriteD(Count);
            WriteB(Array);
        }
    }
}