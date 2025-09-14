using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_NOTE_ACK : GameServerPacket
    {
        private readonly int playersCount;
        public PROTOCOL_CS_NOTE_ACK(int count)
        {
            playersCount = count;
        }
        public override void Write()
        {
            WriteH(1917);
            WriteD(playersCount);
        }
    }
}