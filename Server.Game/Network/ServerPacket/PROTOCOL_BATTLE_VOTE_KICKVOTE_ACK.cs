using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_VOTE_KICKVOTE_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_BATTLE_VOTE_KICKVOTE_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(4171);
            WriteD(erro);
        }
    }
}