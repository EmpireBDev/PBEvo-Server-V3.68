using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_KICK_PLAYER_ACK : GameServerPacket
    {
        public PROTOCOL_SERVER_MESSAGE_KICK_PLAYER_ACK()
        {
        }
        public override void Write()
        {
            WriteH(2563);
            WriteC(0);
        }
    }
}