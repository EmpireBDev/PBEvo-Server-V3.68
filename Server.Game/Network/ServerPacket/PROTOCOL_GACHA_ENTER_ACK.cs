using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GACHA_ENTER_ACK : GameServerPacket
    {
        public PROTOCOL_GACHA_ENTER_ACK()
        {
        }
        public override void Write()
        {
            WriteH(5128); //battle opcode from 3.79 :)
            WriteH(0);
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}
