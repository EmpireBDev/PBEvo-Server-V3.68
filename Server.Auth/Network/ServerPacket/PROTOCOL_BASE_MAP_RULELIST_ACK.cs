using Plugin.Core.Models.Map;
using Plugin.Core.XML;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_MAP_RULELIST_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_MAP_RULELIST_ACK()
        {
        }
        public override void Write()
        {
            WriteH(670);
            WriteH(0);
            WriteH((ushort)SystemMapXML.Rules.Count);
            foreach (MapRule Rule in SystemMapXML.Rules)
            {
                WriteD(Rule.Id);
                WriteC(0);
                WriteC((byte)Rule.Rule);
                WriteC((byte)Rule.StageOptions);
                WriteC((byte)Rule.Conditions);
                WriteC(0);
                WriteS(Rule.Name, 67);
            }
        }
    }
}
