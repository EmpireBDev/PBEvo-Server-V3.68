using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_RECV_WHISPER_ACK : GameServerPacket
    {
        private readonly string Sender, Message;
        private readonly bool chatGM;
        public PROTOCOL_AUTH_RECV_WHISPER_ACK(string Sender, string Message, bool chatGM)
        {
            this.Sender = Sender;
            this.Message = Message;
            this.chatGM = chatGM;
        }
        public override void Write()
        {
            WriteH(1830);
            WriteU(Sender, 66);
            WriteC((byte)(chatGM ? 1 : 0));
            WriteC(0); // ?
            WriteH((ushort)(Message.Length + 1));
            WriteN(Message, Message.Length + 2, "UTF-16LE");
        }
    }
}