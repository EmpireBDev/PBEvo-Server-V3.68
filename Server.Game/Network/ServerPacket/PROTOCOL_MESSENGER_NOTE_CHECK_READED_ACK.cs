using Plugin.Core.Network;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK : GameServerPacket
    {
        private readonly List<int> Messages;
        public PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK(List<int> Messages)
        {
            this.Messages = Messages;
        }
        public override void Write()
        {
            WriteH(903);
            WriteC((byte)Messages.Count);
            for (int i = 0; i < Messages.Count; i++)
            {
                WriteD(Messages[i]);
            }
        }
    }
}