using Plugin.Core.Models;
using Plugin.Core.XML;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_MAP_MATCHINGLIST_ACK : AuthServerPacket
    {
        private readonly List<MapMatch> Matchs;
        private readonly int Total;
        public PROTOCOL_BASE_MAP_MATCHINGLIST_ACK(List<MapMatch> Matchs, int Total)
        {
            this.Matchs = Matchs;
            this.Total = Total;
        }
        public override void Write()
        {
            WriteH(672);
            WriteH(0);
            WriteC((byte)Matchs.Count);
            foreach (MapMatch Match in Matchs)
            {
                WriteD(Match.Mode);
                WriteC((byte)Match.Id);
                WriteC((byte)SystemMapXML.GetMapRule(Match.Mode).Rule);
                WriteC((byte)SystemMapXML.GetMapRule(Match.Mode).StageOptions);
                WriteC((byte)SystemMapXML.GetMapRule(Match.Mode).Conditions);
                WriteC((byte)Match.Limit);
                WriteC((byte)Match.Tag);
                WriteC(0);
                WriteC(1);
            }
            WriteD(Matchs.Count != 100 ? 1 : 0);
            WriteH((ushort)(Total - 100));
            WriteH((ushort)SystemMapXML.Matches.Count);
        }
    }
}
