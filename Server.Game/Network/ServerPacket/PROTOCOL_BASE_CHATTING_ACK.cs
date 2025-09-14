using Plugin.Core.Network;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_CHATTING_ACK : GameServerPacket
    {
        private readonly int erro, banTime;
        public PROTOCOL_BASE_CHATTING_ACK(int erro, int time = 0)
        {
            this.erro = erro;
            banTime = time;
        }
        public override void Write()
        {
            WriteH(2628);
            WriteC((byte)erro); // Result / Type (0 = Not | 2 = Premium Block | 1 = ?)
            if (erro > 0)
            {
                WriteD(banTime);
            }
            /*
             * 1 = STR_MESSAGE_BLOCK_ING
             * 2 = STR_MESSAGE_BLOCK_START
             */
        }
    }
}