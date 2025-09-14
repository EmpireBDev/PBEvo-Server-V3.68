using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK : GameServerPacket
    {
        private readonly int Slot;
        private readonly int Color;
        public PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK(int Slot, int Color)
        {
            this.Slot = Slot;
            this.Color = Color;
        }
        public override void Write()
        {
            WriteH(3926);
            WriteD(Slot);
            WriteC((byte)Color);
        }
    }
}
