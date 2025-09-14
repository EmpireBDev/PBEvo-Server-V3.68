using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_LIST_ACK : GameServerPacket
    {
        private readonly byte[] array;
        private readonly int erro, page, count;
        public PROTOCOL_CS_MEMBER_LIST_ACK(int page, int count, byte[] array)
        {
            this.page = page;
            this.count = count;
            this.array = array;
        }
        public PROTOCOL_CS_MEMBER_LIST_ACK(int erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1829);
            WriteD(erro);
            if (erro == 0)
            {
                WriteC((byte)page);
                WriteC((byte)count);
                WriteB(array);
            }
        }
    }
}