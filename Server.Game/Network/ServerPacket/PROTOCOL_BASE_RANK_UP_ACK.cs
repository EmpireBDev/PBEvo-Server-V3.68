using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_RANK_UP_ACK : GameServerPacket
    {
        private readonly int rank, allExp;
        public PROTOCOL_BASE_RANK_UP_ACK(int rank, int allExp)
        {
            this.rank = rank;
            this.allExp = allExp;
        }
        public override void Write()
        {
            WriteH(551);
            WriteD(rank);
            WriteD(rank);
            WriteD(allExp); //EXP
        }
    }
}