using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_TIMEOUTCLIENT_ACK : GameServerPacket
    {
        public PROTOCOL_BATTLE_TIMEOUTCLIENT_ACK()
        {
        }
        public override void Write()
        {
            WriteH(5144);
        }
    }
}
