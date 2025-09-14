using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CHAR_CHANGE_EQUIP_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_CHAR_CHANGE_EQUIP_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(6150);
            WriteD(Error);
            WriteH(0);
        }
    }
}
