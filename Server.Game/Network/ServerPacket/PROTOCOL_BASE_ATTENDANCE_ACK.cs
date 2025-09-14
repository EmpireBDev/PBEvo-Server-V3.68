using Plugin.Core.Enums;
using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_ACK : GameServerPacket
    {
        private readonly EventVisitModel Visit;
        private readonly PlayerEvent Event;
        private readonly EventErrorEnum Error;
        public PROTOCOL_BASE_ATTENDANCE_ACK(EventErrorEnum Error, EventVisitModel Visit, PlayerEvent Event)
        {
            this.Error = Error;
            this.Visit = Visit;
            this.Event = Event;
        }
        public override void Write()
        {
            WriteH(545);
            WriteD((uint)Error);
            if (Error == EventErrorEnum.VISIT_EVENT_SUCCESS)
            {
                WriteD(Visit.Id);
                WriteC((byte)Event.LastVisitSequence1);
                WriteC((byte)Event.LastVisitSequence2);
                WriteH(1);
                WriteC((byte)Event.DayCheckedIdx);
            }
        }
    }
}