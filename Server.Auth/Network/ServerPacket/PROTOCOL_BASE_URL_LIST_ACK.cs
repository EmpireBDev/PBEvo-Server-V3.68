using Plugin.Core.Models;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_URL_LIST_ACK : AuthServerPacket
    {
        private readonly ServerConfig CFG;
        public PROTOCOL_BASE_URL_LIST_ACK(ServerConfig CFG)
        {
            this.CFG = CFG;
        }
        public override void Write()
        {
            WriteH(674);
            WriteH(514);
            WriteH((ushort)CFG.OfficialBanner.Length);
            WriteQ(0);
            WriteN(CFG.OfficialBanner, CFG.OfficialBanner.Length, "UTF-16LE");
        }
    }
}
