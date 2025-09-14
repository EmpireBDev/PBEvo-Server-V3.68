namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_TAG_INFO_ACK : GameServerPacket
    {
        public PROTOCOL_SHOP_TAG_INFO_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1099);
            WriteH(0);
            WriteC(7);
            WriteC(5);
            WriteH(0);
            WriteC(0);
            WriteD(0);
            WriteH(0);
            WriteC(3);
            WriteQ(0);
            WriteC(0);
            WriteC(4);
            WriteQ(0);
            WriteC(0);
            WriteC(2);
            WriteQ(0);
            WriteC(0);
            WriteC(6);
            WriteQ(0);
            WriteC(0);
            WriteC(1);
            WriteQ(0);
            WriteD(0);
            WriteC(0);
            WriteC(255);
            WriteC(255);
            WriteC(255);
            WriteC(0);
            WriteC(255);
            WriteC(1);
            WriteC(7);
            WriteC(2);
        }
    }
}
