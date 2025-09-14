using Plugin.Core.Models;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_REPAIR_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly List<ItemsModel> Items;
        private readonly Account Player;
        public PROTOCOL_SHOP_REPAIR_ACK(uint Error, List<ItemsModel> Items, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
            this.Items = Items;
        }
        public override void Write()
        {
            WriteH(1077);
            WriteH(0);
            if (Error == 1)
            {
                WriteC((byte)Items.Count);
                foreach (ItemsModel Item in Items)
                {
                    WriteD(0); //object id?
                    WriteD(Item.Id);
                }
                WriteD(Player.Cash);
                WriteD(Player.Gold);
            }
            else
            {
                WriteD(Error);
            }
        }
    }
}
