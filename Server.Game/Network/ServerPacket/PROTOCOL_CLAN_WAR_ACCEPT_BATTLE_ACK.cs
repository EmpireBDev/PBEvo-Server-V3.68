using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_ACCEPT_BATTLE_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CLAN_WAR_ACCEPT_BATTLE_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(1559);
            WriteD(erro);
        }
    }
}