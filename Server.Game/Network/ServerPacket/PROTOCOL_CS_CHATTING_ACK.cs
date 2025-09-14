using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CHATTING_ACK : GameServerPacket
    {
        private readonly string text;
        private readonly Account player;
        private readonly int type, bantime;

        public PROTOCOL_CS_CHATTING_ACK(string text, Account player)
        {
            this.text = text;
            this.player = player;
        }
        public PROTOCOL_CS_CHATTING_ACK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void Write()
        {
            WriteH(1879);
            if (type == 0)
            {
                WriteC((byte)(player.Nickname.Length + 1));
                WriteN(player.Nickname, player.Nickname.Length + 2, "UTF-16LE");
                WriteC((byte)(player.UseChatGM() ? 1 : 0));
                WriteC((byte)player.NickColor);
                WriteC((byte)(text.Length + 1));
                WriteN(text, text.Length + 2, "UTF-16LE");
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