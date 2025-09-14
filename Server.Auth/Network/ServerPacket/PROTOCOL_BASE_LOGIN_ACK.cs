using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Auth.Data.Utils;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_LOGIN_ACK : AuthServerPacket
    {
        private readonly EventErrorEnum Result;
        private readonly string Username;
        private readonly long PlayerId;
        public PROTOCOL_BASE_LOGIN_ACK(EventErrorEnum Result, string Username, long PlayerId)
        {
            this.Result = Result;
            this.Username = Username;
            this.PlayerId = PlayerId;
        }
        public override void Write()
        {
            WriteH(259);
            WriteH(0);
            WriteB(new byte[12]);
            WriteD(AllUtils.GetFeatures());
            WriteH(1402);
            WriteB(new byte[16]);
            WriteB(Result == EventErrorEnum.SUCCESS ? UserData(PlayerId, Username) : Bitwise.HexStringToByteArray("00 00 00 00 00 00 00 00 00 00 00"));
            WriteD((uint)Result);
        }
        private byte[] UserData(long PlayerId, string Username)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)$"{PlayerId}".Length);
                S.WriteS($"{PlayerId}", $"{PlayerId}".Length);
                S.WriteC(0);
                S.WriteC((byte)Username.Length);
                S.WriteS(Username, Username.Length);
                S.WriteQ(PlayerId);
                return S.ToArray();
            }
        }
    }
}