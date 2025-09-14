using Plugin.Core.Models;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CHAR_CREATE_CHARA_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly CharacterModel Chara;
        private readonly PlayerEquipment Equipment;
        private readonly uint Error;
        private readonly int Type;
        public PROTOCOL_CHAR_CREATE_CHARA_ACK(uint Error, int Type, CharacterModel Chara, Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Equipment = Player.Equipment;
            }
            this.Chara = Chara;
            this.Error = Error;
            this.Type = Type;
        }
        public override void Write()
        {
            WriteH(6146);
            WriteH(0);
            WriteC(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteD(Equipment.WeaponPrimary);
                WriteD((uint)Player.Inventory.GetItem(Equipment.WeaponPrimary).ObjectId);
                WriteD(Equipment.WeaponSecondary);
                WriteD((uint)Player.Inventory.GetItem(Equipment.WeaponSecondary).ObjectId);
                WriteD(Equipment.WeaponMelee);
                WriteD((uint)Player.Inventory.GetItem(Equipment.WeaponMelee).ObjectId);
                WriteD(Equipment.WeaponExplosive);
                WriteD((uint)Player.Inventory.GetItem(Equipment.WeaponExplosive).ObjectId);
                WriteD(Equipment.WeaponSpecial);
                WriteD((uint)Player.Inventory.GetItem(Equipment.WeaponSpecial).ObjectId);
                WriteD(Chara.Id);
                WriteD((uint)Player.Inventory.GetItem(Chara.Id).ObjectId);
                WriteD(Equipment.PartHead);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartHead).ObjectId);
                WriteD(Equipment.PartFace);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartFace).ObjectId);
                WriteD(Equipment.PartJacket);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartJacket).ObjectId);
                WriteD(Equipment.PartPocket);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartPocket).ObjectId);
                WriteD(Equipment.PartGlove);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartGlove).ObjectId);
                WriteD(Equipment.PartBelt);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartBelt).ObjectId);
                WriteD(Equipment.PartHolster);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartHolster).ObjectId);
                WriteD(Equipment.PartSkin);
                WriteD((uint)Player.Inventory.GetItem(Equipment.PartSkin).ObjectId);
                WriteD(Equipment.BeretItem);
                WriteD((uint)(Player.Inventory.GetItem(Equipment.BeretItem) == null ? 0 : Player.Inventory.GetItem(Equipment.BeretItem).ObjectId));
                WriteD(Player.Cash);
                WriteD(Player.Gold);
                WriteC((byte)Type);
                WriteC(20);
                WriteC((byte)Chara.Slot);
                WriteC(1);
            }
        }
    }
}
