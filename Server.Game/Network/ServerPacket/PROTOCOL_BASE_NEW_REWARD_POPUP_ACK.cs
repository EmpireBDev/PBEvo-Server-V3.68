using Plugin.Core.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_NEW_REWARD_POPUP_ACK : GameServerPacket
    {
        private readonly List<ItemsModel> Items;
        public PROTOCOL_BASE_NEW_REWARD_POPUP_ACK(List<ItemsModel> Items)
        {
            this.Items = Items;
        }
        public override void Write()
        {
            WriteH(638);
            WriteD(0);
            WriteC((byte)Items.Count);
            foreach (ItemsModel Item in Items)
            {
                WriteD(Item.Id);
                WriteD(Item.Count);
            }
        }
    }
}
