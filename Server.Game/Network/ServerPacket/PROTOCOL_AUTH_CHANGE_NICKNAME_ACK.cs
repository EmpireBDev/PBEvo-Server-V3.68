using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_CHANGE_NICKNAME_ACK : GameServerPacket
    {
        private readonly string name;
        public PROTOCOL_AUTH_CHANGE_NICKNAME_ACK(string name)
        {
            this.name = name;
        }
        public override void Write()
        {
            WriteH(812);
            WriteC((byte)name.Length);
            WriteU(name, name.Length * 2);
        }
    }
}