using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_INVITE_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CS_INVITE_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1913);
            WriteD(erro);
        }
    }
}