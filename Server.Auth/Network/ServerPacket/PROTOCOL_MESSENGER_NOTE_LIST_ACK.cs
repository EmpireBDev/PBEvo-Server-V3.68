namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_MESSENGER_NOTE_LIST_ACK : AuthServerPacket
    {
        private readonly int PageIndex, MessageCount;
        private readonly byte[] HeaderData, BodyData;
        public PROTOCOL_MESSENGER_NOTE_LIST_ACK(int MessageCount, int PageIndex, byte[] HeaderData, byte[] BodyData)
        {
            this.MessageCount = MessageCount;
            this.PageIndex = PageIndex;
            this.HeaderData = HeaderData;
            this.BodyData = BodyData;
        }
        public override void Write()
        {
            WriteH(901);
            WriteC((byte)PageIndex);
            WriteC((byte)MessageCount);
            WriteB(HeaderData);
            WriteB(BodyData);
        }
    }
}