using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Auth.Data.Models;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_CHARA_INFO_ACK : AuthServerPacket
    {
        private readonly Account Player;
        private readonly PlayerEquipment Equip;
        private readonly List<CharacterModel> Characters;
        public PROTOCOL_BASE_GET_CHARA_INFO_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Equip = Player.Equipment;
                Characters = Player.Character.Characters;
            }
        }
        public override void Write()
        {
            WriteH(661);
            WriteH(0);
            WriteC((byte)Characters.Count);
            WriteB(CharacterSlotData(Characters));
            WriteB(CharacterEquipData(Characters));
        }
        private byte[] CharacterSlotData(List<CharacterModel> Characters)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                foreach (CharacterModel Character in Characters)
                {
                    S.WriteC((byte)Character.Slot);
                    S.WriteC(20);
                    S.WriteD(Character.Id);
                    S.WriteD((uint)Player.Inventory.GetItem(Character.Id).ObjectId);
                }
                return S.ToArray();
            }
        }
        private byte[] CharacterEquipData(List<CharacterModel> Characters)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                foreach (CharacterModel Character in Characters)
                {
                    S.WriteD(Equip.WeaponPrimary);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.WeaponPrimary).ObjectId);
                    S.WriteD(Equip.WeaponSecondary);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.WeaponSecondary).ObjectId);
                    S.WriteD(Equip.WeaponMelee);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.WeaponMelee).ObjectId);
                    S.WriteD(Equip.WeaponExplosive);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.WeaponExplosive).ObjectId);
                    S.WriteD(Equip.WeaponSpecial);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.WeaponSpecial).ObjectId);
                    S.WriteD(Character.Id);
                    S.WriteD((uint)Player.Inventory.GetItem(Character.Id).ObjectId);
                    S.WriteD(Equip.PartHead);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartHead).ObjectId);
                    S.WriteD(Equip.PartFace);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartFace).ObjectId);
                    S.WriteD(Equip.PartJacket);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartJacket).ObjectId);
                    S.WriteD(Equip.PartPocket);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartPocket).ObjectId);
                    S.WriteD(Equip.PartGlove);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartGlove).ObjectId);
                    S.WriteD(Equip.PartBelt);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartBelt).ObjectId);
                    S.WriteD(Equip.PartHolster);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartHolster).ObjectId);
                    S.WriteD(Equip.PartSkin);
                    S.WriteD((uint)Player.Inventory.GetItem(Equip.PartSkin).ObjectId);
                    S.WriteD(Equip.BeretItem);
                    S.WriteD((uint)(Player.Inventory.GetItem(Equip.BeretItem) == null ? 0 : Player.Inventory.GetItem(Equip.BeretItem).ObjectId));
                }
                return S.ToArray();
            }
        }
    }
}
