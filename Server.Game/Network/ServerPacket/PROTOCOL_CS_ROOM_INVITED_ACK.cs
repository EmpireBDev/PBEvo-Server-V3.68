using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_ROOM_INVITED_ACK : GameServerPacket
    {
        private readonly int Error;
        public PROTOCOL_CS_ROOM_INVITED_ACK(int Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1902);
            WriteD(Error);
        }
    }
}
