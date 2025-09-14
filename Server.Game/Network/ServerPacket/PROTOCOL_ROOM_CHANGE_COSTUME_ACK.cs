using Plugin.Core.Models;
using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHANGE_COSTUME_ACK : GameServerPacket
    {
        private readonly SlotModel Slot;
        public PROTOCOL_ROOM_CHANGE_COSTUME_ACK(SlotModel Slot)
        {
            this.Slot = Slot;
        }
        public override void Write()
        {
            WriteH(3934);
            WriteD(Slot.Id);
            WriteC((byte)Slot.Costume);
        }
    }
}
