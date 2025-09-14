using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK : GameServerPacket
    {
        private readonly int SlotId;
        public PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK(int SlotId)
        {
            this.SlotId = SlotId;
        }
        public override void Write()
        {
            WriteH(4151);
            WriteD(SlotId);
        }
    }
}