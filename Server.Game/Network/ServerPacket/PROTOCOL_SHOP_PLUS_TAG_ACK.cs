using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_PLUS_TAG_ACK : GameServerPacket
    {
        private readonly int XPEarned, GPEarned;
        public PROTOCOL_SHOP_PLUS_TAG_ACK(int xp_earned, int gp_earned)
        {
            XPEarned = xp_earned;
            GPEarned = gp_earned;
        }
        public override void Write()
        {
            WriteH(3097);
            WriteD(XPEarned);
            WriteD(GPEarned);
        }
    }
}