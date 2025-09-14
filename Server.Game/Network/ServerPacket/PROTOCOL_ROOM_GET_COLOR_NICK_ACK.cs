using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_COLOR_NICK_ACK : GameServerPacket
    {
        private readonly int Slot, Color;
        public PROTOCOL_ROOM_GET_COLOR_NICK_ACK(int Slot, int Color)
        {
            this.Slot = Slot;
            this.Color = Color;
        }
        public override void Write()
        {
            WriteH(3892);
            WriteD(Slot);
            WriteC((byte)Color);
        }
    }
}
