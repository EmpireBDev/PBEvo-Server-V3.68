using Plugin.Core.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_TEAM_BALANCE_ACK : GameServerPacket
    {
        private readonly int Type, Leader;
        private readonly List<SlotModel> Slots;
        public PROTOCOL_ROOM_TEAM_BALANCE_ACK(List<SlotModel> Slots, int Leader, int Type)
        {
            this.Slots = Slots;
            this.Leader = Leader;
            this.Type = Type;
        }
        public override void Write()
        {
            WriteH(3886);
            WriteC((byte)Type);
            WriteC((byte)Leader);
            WriteC((byte)Slots.Count);
            foreach (SlotModel Slot in Slots)
            {
                WriteC((byte)Slot.OldSlot.Id);
                WriteC((byte)Slot.NewSlot.Id);
                WriteC((byte)Slot.OldSlot.State);
                WriteC((byte)Slot.NewSlot.State);
            }
        }
    }
}