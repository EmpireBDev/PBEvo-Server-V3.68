using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_PAGE_CHATTING_ACK : GameServerPacket
    {
        private readonly string sender, message;
        private readonly int type, bantime, name_color;
        private readonly bool isGM;
        public PROTOCOL_CS_PAGE_CHATTING_ACK(Account p, string msg)
        {
            sender = p.Nickname;
            message = msg;
            isGM = p.UseChatGM();
            name_color = p.NickColor;
        }
        public PROTOCOL_CS_PAGE_CHATTING_ACK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void Write()
        {
            WriteH(1911);
            WriteC((byte)type);
            if (type == 0)
            {
                WriteC((byte)(sender.Length + 1));
                WriteN(sender, sender.Length + 2, "UTF-16LE");
                WriteC((byte)(isGM ? 1 : 0));
                WriteC((byte)name_color);
                WriteC((byte)(message.Length + 1));
                WriteN(message, message.Length + 2, "UTF-16LE");
            }
            else
            {
                WriteD(bantime);
            }
            /*
             * 1=STR_MESSAGE_BLOCK_ING
             * 2=STR_MESSAGE_BLOCK_START
             */
        }
    }
}