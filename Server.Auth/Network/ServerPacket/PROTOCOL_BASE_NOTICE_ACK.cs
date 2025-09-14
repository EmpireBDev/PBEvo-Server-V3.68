using Plugin.Core.Models;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_NOTICE_ACK : AuthServerPacket
    {
        private readonly ServerConfig CFG;
        public PROTOCOL_BASE_NOTICE_ACK(ServerConfig CFG)
        {
            this.CFG = CFG;
        }
        public override void Write()
        {
            WriteH(663);
            WriteH(0);
            WriteD(CFG.ChatAnnounceColor);
            WriteD(CFG.ChannelAnnounceColor);
            WriteH(0); // > 1 = WriteN("", Length, "UTF-16LE");
            WriteH((ushort)CFG.ChatAnnounce.Length);
            WriteN(CFG.ChatAnnounce, CFG.ChatAnnounce.Length, "UTF-16LE");
            WriteH((ushort)CFG.ChannelAnnouce.Length);
            WriteN(CFG.ChannelAnnouce, CFG.ChannelAnnouce.Length, "UTF-16LE");
        }
    }
}
