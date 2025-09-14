using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_CHANGE_INVENTORY_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly PlayerEquipment Equip;
        private readonly List<ItemsModel> Coupons;
        public PROTOCOL_SERVER_MESSAGE_CHANGE_INVENTORY_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Coupons = Player.Inventory.GetItemsByType(ItemCategory.NewItem);
                ComDiv.CheckEquipedItems(Player.Equipment, Player.Inventory.Items, false);
                Equip = Player.Equipment;
            }
        }
        public override void Write()
        {
            WriteH(2570);
            WriteH(0);
            WriteC((byte)Coupons.Count);
            for (int i = 0; i < Coupons.Count; i++)
            {
                ItemsModel Coupon = Coupons[i];
                WriteD(Coupon.Id);
            }
            WriteC((byte)Coupons.Count);
            WriteC(0);
            WriteC((byte)Player.Character.Characters.Count);
            foreach (CharacterModel Chara in Player.Character.Characters)
            {
                WriteC((byte)Chara.Slot);
                WriteD(Equip.WeaponPrimary);
                WriteD((uint)Player.Inventory.GetItem(Equip.WeaponPrimary).ObjectId);
                WriteD(Equip.WeaponSecondary);
                WriteD((uint)Player.Inventory.GetItem(Equip.WeaponSecondary).ObjectId);
                WriteD(Equip.WeaponMelee);
                WriteD((uint)Player.Inventory.GetItem(Equip.WeaponMelee).ObjectId);
                WriteD(Equip.WeaponExplosive);
                WriteD((uint)Player.Inventory.GetItem(Equip.WeaponExplosive).ObjectId);
                WriteD(Equip.WeaponSpecial);
                WriteD((uint)Player.Inventory.GetItem(Equip.WeaponSpecial).ObjectId);
                WriteD(Chara.Id);
                WriteD((uint)Player.Inventory.GetItem(Chara.Id).ObjectId);
                WriteD(Equip.PartHead);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartHead).ObjectId);
                WriteD(Equip.PartFace);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartFace).ObjectId);
                WriteD(Equip.PartJacket);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartJacket).ObjectId);
                WriteD(Equip.PartPocket);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartPocket).ObjectId);
                WriteD(Equip.PartGlove);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartGlove).ObjectId);
                WriteD(Equip.PartBelt);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartBelt).ObjectId);
                WriteD(Equip.PartHolster);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartHolster).ObjectId);
                WriteD(Equip.PartSkin);
                WriteD((uint)Player.Inventory.GetItem(Equip.PartSkin).ObjectId);
                WriteD(Equip.BeretItem);
                WriteD((uint)(Equip.BeretItem == 0 ? 0 : Player.Inventory.GetItem(Equip.BeretItem).ObjectId));
            }
            WriteD(Player.Equipment.DinoItem);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.DinoItem).ObjectId);
        }
    }
}