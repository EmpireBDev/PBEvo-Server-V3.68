
using Plugin.Core.Network;

namespace Server.Match.Network.Packets
{
    public class PROTOCOL_CONNECT
    {
        public static byte[] GET_CODE()
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC(66);
                S.WriteC(0);
                S.WriteT(0);
                S.WriteC(0);
                S.WriteH(14);
                S.WriteD(0);
                S.WriteC(8);
                return S.ToArray();
            }
        }
    }
}