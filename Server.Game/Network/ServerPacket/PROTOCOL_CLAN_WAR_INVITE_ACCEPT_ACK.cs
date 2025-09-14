using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_INVITE_ACCEPT_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CLAN_WAR_INVITE_ACCEPT_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1560);
            WriteD(erro);
        }
    }
}