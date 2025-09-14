using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_GET_SAILLIST_ACK : GameServerPacket
    {
        private readonly bool Enable;
        public PROTOCOL_SHOP_GET_SAILLIST_ACK(bool Enable)
        {
            this.Enable = Enable;
        }
        public override void Write()
        {
            WriteH(1030);
            WriteC((byte)(Enable ? 1 : 0));
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}