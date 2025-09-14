namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_JACKPOT_ACK : GameServerPacket
    {
        private readonly string Winner;
        private readonly int CouponId, Random;
        public PROTOCOL_AUTH_SHOP_JACKPOT_ACK(string Winner, int CouponId, int Random)
        {
            this.Winner = Winner;
            this.CouponId = CouponId;
            this.Random = Random;
        }
        public override void Write()
        {
            WriteH(1068);
            WriteH(0);
            WriteC((byte)Random);
            WriteD(CouponId);
            WriteC((byte)Winner.Length);
            WriteN(Winner, Winner.Length, "UTF-16LE");
        }
    }
}