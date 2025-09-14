using Plugin.Core.Network;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_MESSENGER_NOTE_DELETE_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly List<object> Objects;
        public PROTOCOL_MESSENGER_NOTE_DELETE_ACK(uint Error, List<object> Objects)
        {
            this.Error = Error;
            this.Objects = Objects;
        }
        public override void Write()
        {
            WriteH(905);
            WriteD(Error);
            WriteC((byte)Objects.Count);
            foreach (long Object in Objects)
            {
                WriteD((uint)Object);
            }
        }
    }
}