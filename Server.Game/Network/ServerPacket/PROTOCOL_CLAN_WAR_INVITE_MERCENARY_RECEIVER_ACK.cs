using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK : GameServerPacket
    {
        private readonly int formacao;
        private readonly uint erro;
        public PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK(uint erro, int formacao = 0)
        {
            this.erro = erro;
            this.formacao = formacao;
        }
        public override void Write()
        {
            WriteH(1572);
            WriteD(erro);
            if (erro == 0)
            {
                WriteC((byte)formacao);
            }
        }
    }
}