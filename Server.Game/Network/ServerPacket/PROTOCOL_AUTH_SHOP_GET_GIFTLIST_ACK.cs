using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_GET_GIFTLIST_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_AUTH_SHOP_GET_GIFTLIST_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1042);
            WriteH(0);
            //WriteD(Error);
            if (Error == 0)
            {
                //TODO Here
            }
        }
    }
}