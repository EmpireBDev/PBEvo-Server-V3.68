using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK : GameServerPacket
    {
        public PROTOCOL_BATTLE_NOTIFY_KICKVOTE_CANCEL_ACK()
        {
        }
        public override void Write()
        {
            WriteH(3405);
        }
    }
}