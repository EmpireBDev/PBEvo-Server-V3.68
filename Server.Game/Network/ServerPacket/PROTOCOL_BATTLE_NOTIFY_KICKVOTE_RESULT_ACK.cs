using Plugin.Core.Models;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_NOTIFY_KICKVOTE_RESULT_ACK : GameServerPacket
    {
        private readonly VoteKickModel vote;
        private readonly uint erro;
        public PROTOCOL_BATTLE_NOTIFY_KICKVOTE_RESULT_ACK(uint erro, VoteKickModel vote)
        {
            this.erro = erro;
            this.vote = vote;
        }
        public override void Write()
        {
            WriteH(3403);
            WriteC((byte)vote.VictimIdx);
            WriteC((byte)vote.Accept);
            WriteC((byte)vote.Denie);
            WriteD(erro);
            //[2147488000] - cancelou a votação
            //[2147488001] - Sem votos aliados
            //[2147488002] - Sem votos adversários
            //[2147488003] - Patente não pode abrir
            //
        }
    }
}