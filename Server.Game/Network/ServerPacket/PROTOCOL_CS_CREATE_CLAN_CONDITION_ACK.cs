using Plugin.Core;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CREATE_CLAN_CONDITION_ACK : GameServerPacket
    {
        public PROTOCOL_CS_CREATE_CLAN_CONDITION_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1939);
            WriteC((byte)ConfigLoader.MinCreateRank);
            WriteD(ConfigLoader.MinCreateGold);
        }
    }
}