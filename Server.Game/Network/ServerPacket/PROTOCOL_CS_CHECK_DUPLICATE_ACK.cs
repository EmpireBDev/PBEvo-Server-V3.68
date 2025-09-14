using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CHECK_DUPLICATE_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CS_CHECK_DUPLICATE_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1941);
            WriteD(erro);
        }
    }
}