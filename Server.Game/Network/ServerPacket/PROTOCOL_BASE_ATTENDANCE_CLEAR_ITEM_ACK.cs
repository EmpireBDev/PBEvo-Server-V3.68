using Plugin.Core.Enums;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK : GameServerPacket
    {
        private readonly EventErrorEnum Error;
        public PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK(EventErrorEnum Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(547);
            WriteD((uint)Error);
        }
    }
}