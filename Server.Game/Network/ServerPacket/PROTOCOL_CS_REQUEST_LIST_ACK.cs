using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REQUEST_LIST_ACK : GameServerPacket
    {
        private readonly int erro, page, count;
        private readonly byte[] array;
        public PROTOCOL_CS_REQUEST_LIST_ACK(int erro, int count, int page, byte[] array)
        {
            this.erro = erro;
            this.count = count;
            this.page = page;
            this.array = array;
        }
        public PROTOCOL_CS_REQUEST_LIST_ACK(int erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1843);
            WriteD(erro);
            if (erro >= 0)
            {
                WriteC((byte)page);
                WriteC((byte)count);
                WriteB(array);
            }
        }
    }
}