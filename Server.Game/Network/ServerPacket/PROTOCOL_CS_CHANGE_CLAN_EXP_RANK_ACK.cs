using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CHANGE_CLAN_EXP_RANK_ACK : GameServerPacket
    {
        private readonly int Exp;
        public PROTOCOL_CS_CHANGE_CLAN_EXP_RANK_ACK(int Exp)
        {
            this.Exp = Exp;
        }
        public override void Write()
        {
            WriteH(1904);
            WriteC((byte)Exp);
        }
    }
}
