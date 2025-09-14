using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SEND_WHISPER_ACK : GameServerPacket
    {
        private readonly string name, msg;
        private readonly uint erro;
        private readonly int type, bantime;
        public PROTOCOL_AUTH_SEND_WHISPER_ACK(string name, string msg, uint erro)
        {
            this.name = name;
            this.msg = msg;
            this.erro = erro;
        }
        public PROTOCOL_AUTH_SEND_WHISPER_ACK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void Write()
        {
            WriteH(1827);
            WriteC((byte)type);
            if (type == 0)
            {
                WriteD(erro);
                WriteU(name, 66);
                if (erro == 0)
                {
                    WriteH((ushort)(msg.Length + 1));
                    WriteN(msg, msg.Length + 2, "UTF-16LE");
                }
            }
            else
            {
                WriteD(bantime);
            }
            /*
             * 1 = STR_MESSAGE_BLOCK_ING
             * 2 = STR_MESSAGE_BLOCK_START
             */
        }
    }
}