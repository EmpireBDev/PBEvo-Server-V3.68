namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1085);
            WriteD(Error);
        }
    }
}
