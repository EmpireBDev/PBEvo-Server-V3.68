using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GACHA_LEAVE_ACK : GameServerPacket
    {
        public PROTOCOL_GACHA_LEAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(3332);
            WriteH(0);
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}
