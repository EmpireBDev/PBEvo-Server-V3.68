using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_ITEM_RECEIVE_ACK : GameServerPacket
    {
        private readonly uint er;
        public PROTOCOL_SERVER_MESSAGE_ITEM_RECEIVE_ACK(uint er)
        {
            this.er = er;
        }
        public override void Write()
        {
            WriteH(2692);
            WriteD(er);
        }
    }
}