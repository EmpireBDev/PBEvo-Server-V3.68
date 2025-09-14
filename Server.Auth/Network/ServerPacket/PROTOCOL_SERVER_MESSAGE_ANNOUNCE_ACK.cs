namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK : AuthServerPacket
    {
        private readonly string Message;
        public PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(string Message)
        {
            this.Message = Message;
        }
        public override void Write()
        {
            WriteH(2567);
            WriteH(0);
            WriteD(0);
            WriteH((ushort)(Message.Length));
            WriteN(Message, Message.Length, "UTF-16LE");
            WriteD(2);
        }
    }
}