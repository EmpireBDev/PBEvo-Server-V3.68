using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_COMMISSION_MASTER_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CS_COMMISSION_MASTER_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1858);
            WriteD(erro);
        }
    }
}