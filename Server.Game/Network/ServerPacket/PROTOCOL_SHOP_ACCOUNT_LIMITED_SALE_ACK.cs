namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_ACCOUNT_LIMITED_SALE_ACK : GameServerPacket
    {
        public PROTOCOL_SHOP_ACCOUNT_LIMITED_SALE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1101);
            WriteD(1);
            WriteD(1);
            WriteD(1);
            WriteD(1);
        }
    }
}