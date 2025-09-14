using Plugin.Core;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_CONNECT_ACK : GameServerPacket
    {
        private readonly int ServerId;
        private readonly int SessionId;
        private readonly ushort SessionSeed;
        private readonly List<byte[]> RSA_KEY;
        public PROTOCOL_BASE_CONNECT_ACK(GameClient Client)
        {
            ServerId = Client.ServerId;
            SessionId = Client.SessionId;
            SessionSeed = Client.SessionSeed;
            RSA_KEY = Bitwise.GenerateRSAKeyPair(SessionId, SECURITY_KEY, SEED_LENGTH);
        }
        public override void Write()
        {
            WriteH(514);
            WriteH(0);
            WriteC((byte)ChannelsXML.GetChannels(ServerId).Count);
            foreach (ChannelModel Channel in ChannelsXML.GetChannels(ServerId))
            {
                WriteC((byte)Channel.Type);
            }
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