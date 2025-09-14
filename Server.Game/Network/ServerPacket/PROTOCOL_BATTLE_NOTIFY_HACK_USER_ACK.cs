using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_NOTIFY_HACK_USER_ACK : GameServerPacket
    {
        private readonly int slotId;
        public PROTOCOL_BATTLE_NOTIFY_HACK_USER_ACK(int slot)
        {
            slotId = slot;
        }
        public override void Write()
        {
            WriteH(3413);
            WriteC((byte)slotId); // Slot Hacker
            WriteC(1); //?
            WriteD(1); //?
        }
    }
}