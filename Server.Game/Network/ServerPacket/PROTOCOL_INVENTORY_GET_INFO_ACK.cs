using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_INVENTORY_GET_INFO_ACK : GameServerPacket
    {
        private readonly int Type;
        private readonly ushort Total;
        private readonly List<ItemsModel> Charas, Weapons, Coupons;
        public PROTOCOL_INVENTORY_GET_INFO_ACK(int Type, Account Player, ItemsModel Item)
        {
            this.Type = Type;
            Total = AllUtils.DataFromItem(Player, Item, Type, out Charas, out Weapons, out Coupons);
        }
        public override void Write()
        {
            WriteH(3334);
            WriteH(0);
            WriteH(Total);
            WriteC((byte)Type);
            WriteC(0);
            WriteB(InventoryData(Charas));
            WriteB(InventoryData(Weapons));
            WriteB(InventoryData(Coupons));
            WriteC(0);
        }
        private byte[] InventoryData(List<ItemsModel> Items)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                foreach (ItemsModel Item in Items)
                {
                    S.WriteD((uint)Item.ObjectId);
                    S.WriteD(Item.Id);
                    S.WriteC((byte)Item.Equip);
                    S.WriteD(Item.Count);
                }
                return S.ToArray();
            }
        }
    }
}
