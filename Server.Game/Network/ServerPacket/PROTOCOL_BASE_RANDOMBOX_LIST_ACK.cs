using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_RANDOMBOX_LIST_ACK : GameServerPacket
    {
        private readonly bool Enable;
        public PROTOCOL_BASE_RANDOMBOX_LIST_ACK(bool Enable)
        {
            this.Enable = Enable;
        }
        public override void Write()
        {
            WriteH(707);
            WriteC((byte)(Enable ? 1 : 0));
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}