using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_JOIN_REQUEST_ACK : GameServerPacket
    {
        private readonly int clanId;
        private readonly uint erro;
        public PROTOCOL_CS_JOIN_REQUEST_ACK(uint erro, int clanId)
        {
            this.erro = erro;
            this.clanId = clanId;
        }
        public override void Write()
        {
            WriteH(1837);
            WriteD(erro);
            if (erro == 0)
            {
                WriteD(clanId);
            }
        }
    }
}