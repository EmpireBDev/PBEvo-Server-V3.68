using Plugin.Core.Models;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_RECV_GIFT_ACK : GameServerPacket
    {
        private readonly MessageModel gift;
        public PROTOCOL_AUTH_SHOP_RECV_GIFT_ACK(MessageModel gift)
        {
            this.gift = gift;
        }
        public override void Write()
        {
            WriteH(1079);
            WriteD((uint)gift.ObjectId);
            WriteD((uint)gift.SenderId);
            WriteD((int)gift.State);
            WriteD((uint)gift.ExpireDate);
            WriteC((byte)(gift.SenderName.Length + 1));
            WriteS(gift.SenderName, gift.SenderName.Length + 1);
            WriteC(6);
            WriteS("EVENT", 6);
        }
    }
}