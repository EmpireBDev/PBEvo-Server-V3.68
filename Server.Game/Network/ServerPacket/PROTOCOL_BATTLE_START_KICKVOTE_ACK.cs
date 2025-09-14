using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_START_KICKVOTE_ACK : GameServerPacket
    {
        private readonly VoteKickModel vote;
        public PROTOCOL_BATTLE_START_KICKVOTE_ACK(VoteKickModel vote)
        {
            this.vote = vote;
        }
        public override void Write()
        {
            WriteH(3399);
            WriteC((byte)vote.CreatorIdx);
            WriteC((byte)vote.VictimIdx);
            WriteC((byte)vote.Motive);
        }
    }
}