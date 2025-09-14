using Plugin.Core.Models;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly List<QuickstartModel> Quickstart;
        private readonly QuickstartModel Select;
        private readonly RoomModel Room;
        public PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK(uint Error, List<QuickstartModel> Quickstart, RoomModel Room, QuickstartModel Select)
        {
            this.Error = Error;
            this.Quickstart = Quickstart;
            this.Select = Select;
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(5378);
            WriteD(Error);
            foreach (QuickstartModel Quick in Quickstart)
            {
                WriteC((byte)Quick.MapId);
                WriteC((byte)Quick.Rule);
                WriteC((byte)Quick.StageOptions);
                WriteC((byte)Quick.Type);
            }
            if (Error == 0)
            {
                WriteC((byte)Room.ChannelId);
                WriteD(Room.RoomId);
                WriteH(1);
                if (Error != 0)
                {
                    WriteC((byte)Select.MapId);
                    WriteC((byte)Select.Rule);
                    WriteC((byte)Select.StageOptions);
                    WriteC((byte)Select.Type);
                }
                else
                {
                    WriteC(0);
                    WriteC(0);
                    WriteC(0);
                    WriteC(0);
                }
                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteD(0);
            }
        }
    }
}