using Plugin.Core.Utility;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_DISCONNECTIONSUCCESS_ACK : AuthServerPacket
    {
        private readonly uint Error;
        private readonly bool HackUse;
        public PROTOCOL_SERVER_MESSAGE_DISCONNECTIONSUCCESS_ACK(uint Error, bool HackUse)
        {
            this.Error = Error;
            this.HackUse = HackUse;
        }
        public override void Write()
        {
            WriteH(2562);
            WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
            WriteD(Error);
            WriteD(HackUse ? 1 : 0); //Se for igual a 1, novo writeD (Da DC no cliente, Programa ilegal)
            if (HackUse)
            {
                WriteD(0);
            }
        }
    }
}