using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_ROOM_INVITED_RESULT_ACK : GameServerPacket
    {
        private readonly long pId;
        public PROTOCOL_CS_ROOM_INVITED_RESULT_ACK(long pId)
        {
            this.pId = pId;
        }
        public override void Write()
        {
            WriteH(1903);
            WriteQ(pId);
        }
    }
}