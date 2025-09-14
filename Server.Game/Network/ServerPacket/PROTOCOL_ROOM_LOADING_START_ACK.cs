using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_LOADING_START_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_ROOM_LOADING_START_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(3914);
            WriteD(Error);
        }
    }
}
