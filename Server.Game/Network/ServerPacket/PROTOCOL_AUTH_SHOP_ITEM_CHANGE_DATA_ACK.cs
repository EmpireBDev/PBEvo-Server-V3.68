using Plugin.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1088);
            WriteD(Error);
        }
    }
}
