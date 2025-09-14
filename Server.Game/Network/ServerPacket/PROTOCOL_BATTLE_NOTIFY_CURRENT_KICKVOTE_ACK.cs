using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK : GameServerPacket
    {
        private readonly VoteKickModel VoteKick;
        public PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK(VoteKickModel VoteKick)
        {
            this.VoteKick = VoteKick;
        }
        public override void Write()
        {
            WriteH(3407);
            WriteC((byte)VoteKick.Accept);
            WriteC((byte)VoteKick.Denie);
        }
    }
}