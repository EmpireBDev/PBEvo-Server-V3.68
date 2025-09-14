using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_NOTIFY_BE_KICKED_BY_KICKVOTE_ACK : GameServerPacket
    {
        public PROTOCOL_BATTLE_NOTIFY_BE_KICKED_BY_KICKVOTE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(3409);
        }
    }
}