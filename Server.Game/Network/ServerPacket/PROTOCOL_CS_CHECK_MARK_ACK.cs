using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CHECK_MARK_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CS_CHECK_MARK_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1881);
            WriteD(erro);
        }
    }
}