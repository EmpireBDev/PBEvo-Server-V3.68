using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_ACK : GameServerPacket
    {
        public PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_ACK()
        {
        }
        public override void Write()
        {
            WriteH(6941);
            WriteD(0);
        }
    }
}
