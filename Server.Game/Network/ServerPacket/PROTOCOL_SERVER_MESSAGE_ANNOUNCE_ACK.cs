namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK : GameServerPacket
    {
        private readonly string message;
        public PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(string message)
        {
            this.message = message;
        }
        public override void Write()
        {
            WriteH(2567);
            WriteH(0);
            WriteD(0);
            WriteH((ushort)(message.Length));
            WriteN(message, message.Length, "UTF-16LE");
            WriteD(2);
        }
    }
}