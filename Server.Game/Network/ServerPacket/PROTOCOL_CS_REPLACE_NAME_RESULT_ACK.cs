using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_NAME_RESULT_ACK : GameServerPacket
    {
        private readonly string Name;
        public PROTOCOL_CS_REPLACE_NAME_RESULT_ACK(string name)
        {
            Name = name;
        }
        public override void Write()
        {
            WriteH(1888);
            WriteU(Name, 34);
        }
    }
}