using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_SUPPLAY_BOX_PRESENT_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly List<ItemsModel> Charas = new List<ItemsModel>(), Weapons = new List<ItemsModel>(), Coupons = new List<ItemsModel>();
        public PROTOCOL_BASE_SUPPLAY_BOX_PRESENT_ACK(uint Error, ItemsModel Item, Account Player)
        {
            this.Error = Error; 
            ItemsModel Model = new ItemsModel(Item);
            if (Model != null)
            {
                ComDiv.TryCreateItem(Model, Player.Inventory, Player.PlayerId);
                SendItemInfo.LoadItem(Player, Model);
                if (Model.Category == ItemCategory.Weapon)
                {
                    Weapons.Add(Model);
                }
                else if (Model.Category == ItemCategory.Character)
                {
                    Charas.Add(Model);
                }
                else if (Model.Category == ItemCategory.Coupon)
                {
                    Coupons.Add(Model);
                }
            }
        }
        public override void Write()
        {
            WriteH(619);
            WriteD(Charas.Count);
            WriteD(Weapons.Count);
            WriteD(Coupons.Count);
            WriteD(0);
            foreach (ItemsModel Item in Charas)
            {
                WriteQ(Item.ObjectId);
                WriteD(Item.Id);
                WriteC((byte)Item.Equip);
                WriteD(Item.Count);
            }
            foreach (ItemsModel Item in Weapons)
            {
                WriteQ(Item.ObjectId);
                WriteD(Item.Id);
                WriteC((byte)Item.Equip);
                WriteD(Item.Count);
            }
            foreach (ItemsModel Item in Coupons)
            {
                WriteQ(Item.ObjectId);
                WriteD(Item.Id);
                WriteC((byte)Item.Equip);
                WriteD(Item.Count);
            }
        }
    }
}
