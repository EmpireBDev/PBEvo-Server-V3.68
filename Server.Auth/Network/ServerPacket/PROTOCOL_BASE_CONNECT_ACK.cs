using Plugin.Core.Utility;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_CONNECT_ACK : AuthServerPacket
    {
        private readonly int SessionId;
        private readonly ushort SessionSeed;
        private readonly List<byte[]> RSA_KEY;
        public PROTOCOL_BASE_CONNECT_ACK(AuthClient Client)
        {
            SessionId = Client.SessionId;
            SessionSeed = Client.SessionSeed;
            RSA_KEY = Bitwise.GenerateRSAKeyPair(SessionId, SECURITY_KEY, SEED_LENGTH);
        }
        public override void Write()
        {
            WriteH(514);
            WriteH(0);
            WriteC(11);
            WriteB(Bitwise.HexStringToByteArray("00 00 00 00 00 00 00 00 00 00 00"));
            WriteH((ushort)(RSA_KEY[0].Length + RSA_KEY[1].Length + 2));
            WriteH((ushort)(RSA_KEY[0].Length));
            WriteB(RSA_KEY[0]);
            WriteB(RSA_KEY[1]);
            WriteC(3);
            WriteH(68);
            WriteH(SessionSeed);
            WriteD(SessionId);
        }
    }
}
