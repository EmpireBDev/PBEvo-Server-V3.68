using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK : GameServerPacket
    {
        private readonly long ObjectId;
        private readonly uint Error;
        public PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK(uint Error, long ObjectId = 0)
        {
            this.Error = Error;
            if (Error == 1)
            {
                this.ObjectId = ObjectId;
            }
        }
        public override void Write()
        {
            WriteH(1056);
            WriteD(Error);
            if (Error == 1)
            {
                WriteD((int)ObjectId);
            }
        }
    }
}