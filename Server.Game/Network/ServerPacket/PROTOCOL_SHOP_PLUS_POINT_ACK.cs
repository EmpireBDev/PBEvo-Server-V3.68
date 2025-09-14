using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_PLUS_POINT_ACK : GameServerPacket
    {
        private readonly int Gold, Increase, Type;
        public PROTOCOL_SHOP_PLUS_POINT_ACK(int Increase, int Gold, int Type)
        {
            this.Increase = Increase;
            this.Gold = Gold;
            this.Type = Type;
        }
        public override void Write()
        {
            WriteH(1072);
            WriteH(0);
            WriteC((byte)Type);
            WriteD(Gold);
            WriteD(Increase);
        }
    }
}