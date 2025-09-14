using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_ACK : GameServerPacket
    {
        private readonly int Slot;
        public PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_ACK(int Slot)
        {
            this.Slot = Slot;
        }
        public override void Write()
        {
            WriteH(4135);
            WriteD(Slot);
        }
    }
}