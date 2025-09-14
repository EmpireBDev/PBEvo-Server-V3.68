using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_CHATTING_ACK : GameServerPacket
    {
        private readonly string Sender, Message;
        private readonly int SessionId;
        private readonly int NickColor;
        private readonly bool GMColor;
        public PROTOCOL_LOBBY_CHATTING_ACK(Account Player, string Message, bool GMColor = false)
        {
            if (!GMColor)
            {
                NickColor = Player.NickColor;
                this.GMColor = Player.UseChatGM();
            }
            else
            {
                this.GMColor = true;
            }
            Sender = Player.Nickname;
            SessionId = Player.GetSessionId();
            this.Message = Message;
        }
        public PROTOCOL_LOBBY_CHATTING_ACK(string Sender, int SessionId, int NickColor, bool GMColor, string Message)
        {
            this.Sender = Sender;
            this.SessionId = SessionId;
            this.NickColor = NickColor;
            this.GMColor = GMColor;
            this.Message = Message;
        }
        public override void Write()
        {
            WriteH(3087);
            WriteD(SessionId);
            WriteC((byte)(Sender.Length + 1));
            WriteN(Sender, Sender.Length + 2, "UTF-16LE");
            WriteC((byte)NickColor);
            WriteC((byte)(GMColor ? 1 : 0));
            WriteH((ushort)(Message.Length + 1));
            WriteN(Message, Message.Length + 2, "UTF-16LE");
        }
    }
}