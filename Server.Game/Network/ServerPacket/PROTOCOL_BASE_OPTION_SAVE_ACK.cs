using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_OPTION_SAVE_ACK : GameServerPacket
    {
        public PROTOCOL_BASE_OPTION_SAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(531);
            WriteD(0);
        }
    }
}
