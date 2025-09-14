using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_DISCONNECTED_HACK : GameServerPacket
    {
        private readonly uint error;
        private readonly bool type;
        public PROTOCOL_SERVER_MESSAGE_DISCONNECTED_HACK(uint error, bool type)
        {
            this.error = error;
            this.type = type;
        }
        public override void Write()
        {
            WriteH(2562);
            WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
            WriteD(error);
            WriteD((byte)(type ? 1 : 0));
            if (type)
            {
                WriteD(0);
            }
        }
    }
}