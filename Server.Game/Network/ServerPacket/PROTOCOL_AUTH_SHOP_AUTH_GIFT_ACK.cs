using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly List<ItemsModel> Charas = new List<ItemsModel>(), Weapons = new List<ItemsModel>(), Coupons = new List<ItemsModel>();
        public PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(uint Error, ItemsModel Item = null, Account Player = null)
        {
            this.Error = Error;
            ItemsModel modelo = new ItemsModel(Item);
            if (modelo != null)
            {
                ComDiv.TryCreateItem(modelo, Player.Inventory, Player.PlayerId);
                if (modelo.Category == ItemCategory.Weapon)
                {
                    Weapons.Add(modelo);
                }
                else if (modelo.Category == ItemCategory.Character)
                {
                    Charas.Add(modelo);
                }
                else if (modelo.Category == ItemCategory.Coupon)
                {
                    Coupons.Add(modelo);
                }
            }
        }
        public override void Write()
        {
            WriteH(1054);
            WriteD(Error);
            if (Error == 1)
            {
                WriteH(0);
                WriteH((ushort)(Charas.Count + Weapons.Count + Coupons.Count));
                foreach (ItemsModel Item in Charas)
                {
                    WriteD((uint)Item.ObjectId);
                    WriteD(Item.Id);
                    WriteC((byte)Item.Equip);
                    WriteD(Item.Count);
                }
                foreach (ItemsModel Item in Weapons)
                {
                    WriteD((uint)Item.ObjectId);
                    WriteD(Item.Id);
                    WriteC((byte)Item.Equip);
                    WriteD(Item.Count);
                }
                foreach (ItemsModel Item in Coupons)
                {
                    WriteD((uint)Item.ObjectId);
                    WriteD(Item.Id);
                    WriteC((byte)Item.Equip);
                    WriteD(Item.Count);
                }
            }
        }
    }
}