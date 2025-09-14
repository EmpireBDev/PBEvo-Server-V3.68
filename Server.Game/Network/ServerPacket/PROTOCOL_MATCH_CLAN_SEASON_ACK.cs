using Plugin.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_MATCH_CLAN_SEASON_ACK : GameServerPacket
    {
        private readonly bool EventEnable;
        public PROTOCOL_MATCH_CLAN_SEASON_ACK(bool EventEnable)
        {
            this.EventEnable = EventEnable;
        }
        public override void Write()
        {
            WriteH(7700);
            WriteH(0);
            WriteD(2);//servers count?
            WriteD(EventEnable ? 1 : 0);//na
            WriteB(ComDiv.AddressBytes("127.0.0.1")); //ip?
            WriteB(ComDiv.AddressBytes("255.255.255.255")); //mask?
            WriteB(new byte[109]);
            WriteB(ComDiv.AddressBytes("127.0.0.1")); //ip?
            WriteB(ComDiv.AddressBytes("127.0.0.1")); //ip?
            WriteB(ComDiv.AddressBytes("255.255.255.255")); //mask?
            WriteB(new byte[103]);
        }
    }
}
