using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_USER_ITEM_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly List<ItemsModel> Coupons;
        public PROTOCOL_ROOM_GET_USER_ITEM_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Coupons = Player.Inventory.GetItemsByType(ItemCategory.NewItem);
            }
        }
        public override void Write()
        {
            WriteH(3900);
            WriteH(0);
            WriteH((short)Coupons.Count);
            for (int i = 0; i < Coupons.Count; i++)
            {
                WriteD(Coupons[i].Id);
            }
            WriteD(Player.Equipment.DinoItem);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.DinoItem).ObjectId);
            WriteD(Player.Equipment.WeaponPrimary);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponPrimary).ObjectId);
            WriteD(Player.Equipment.WeaponSecondary);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponSecondary).ObjectId);
            WriteD(Player.Equipment.WeaponMelee);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponMelee).ObjectId);
            WriteD(Player.Equipment.WeaponExplosive);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponExplosive).ObjectId);
            WriteD(Player.Equipment.WeaponSpecial);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponSpecial).ObjectId);
            WriteD(Player.SlotId % 2 == 0 ? Player.Equipment.CharaRedId : Player.Equipment.CharaBlueId);
            WriteD((uint)Player.Inventory.GetItem(Player.SlotId % 2 == 0 ? Player.Equipment.CharaRedId : Player.Equipment.CharaBlueId).ObjectId);
            WriteD(Player.Equipment.PartHead);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartHead).ObjectId);
            WriteD(Player.Equipment.PartFace);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartFace).ObjectId);
            WriteD(Player.Equipment.PartJacket);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartJacket).ObjectId);
            WriteD(Player.Equipment.PartPocket);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartPocket).ObjectId);
            WriteD(Player.Equipment.PartGlove);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartGlove).ObjectId);
            WriteD(Player.Equipment.PartBelt);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartBelt).ObjectId);
            WriteD(Player.Equipment.PartHolster);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartHolster).ObjectId);
            WriteD(Player.Equipment.PartSkin);
            WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartSkin).ObjectId);
            WriteD(Player.Equipment.BeretItem);
            WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.BeretItem) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.BeretItem).ObjectId));
        }
    }
}