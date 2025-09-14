using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_MARKEFFECT_RESULT_ACK : GameServerPacket
    {
        private readonly int Effect;
        public PROTOCOL_CS_REPLACE_MARKEFFECT_RESULT_ACK(int Effect)
        {
            this.Effect = Effect;
        }
        public override void Write()
        {
            WriteH(1994);
            WriteC((byte)Effect);
        }
    }
}
