using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_CONTEXT_ACK : GameServerPacket
    {
        private readonly int erro, playersCount;
        public PROTOCOL_CS_MEMBER_CONTEXT_ACK(int erro, int playersCount)
        {
            this.erro = erro;
            this.playersCount = playersCount;
        }
        public PROTOCOL_CS_MEMBER_CONTEXT_ACK(int erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1827);
            WriteD(erro);
            if (erro == 0)
            {
                WriteC((byte)playersCount);
                WriteC(14);
                WriteC((byte)Math.Ceiling(playersCount / 14d));
                WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
            }
        }
    }
}