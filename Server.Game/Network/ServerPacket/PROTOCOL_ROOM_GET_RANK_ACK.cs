using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_RANK_ACK : GameServerPacket
    {
        private readonly int Slot, Rank;
        public PROTOCOL_ROOM_GET_RANK_ACK(int Slot, int Rank)
        {
            this.Slot = Slot;
            this.Rank = Rank;
        }
        public override void Write()
        {
            WriteH(3890);
            WriteD(Slot);
            WriteD(Rank);
        }
    }
}
