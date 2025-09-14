using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_TICKET_UPDATE_ACK : GameServerPacket
    {
        public PROTOCOL_BASE_TICKET_UPDATE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(717);
            WriteH(0);
        }
    }
}