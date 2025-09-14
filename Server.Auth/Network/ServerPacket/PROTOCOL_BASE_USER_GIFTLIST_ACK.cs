using Plugin.Core.Models;
using Plugin.Core.Utility;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_GIFTLIST_ACK : AuthServerPacket
    {
        private readonly int Error;
        private readonly List<MessageModel> Gifts;
        public PROTOCOL_BASE_USER_GIFTLIST_ACK(int Error, List<MessageModel> Gifts)
        {
            this.Error = Error;
            this.Gifts = Gifts;
        }
        public override void Write()
        {
            WriteH(684); //TODO
            WriteH(0);
            WriteC((byte)Gifts.Count);
            for (int i = 0; i < Gifts.Count; ++i)
            {
                MessageModel Gift = Gifts[i];
            }
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}
