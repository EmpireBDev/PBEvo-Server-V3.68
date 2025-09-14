namespace Plugin.Core.Models
{
    public class PlayerEquipment
    {
        public long OwnerId;
        public int WeaponPrimary;
        public int WeaponSecondary;
        public int WeaponMelee;
        public int WeaponExplosive;
        public int WeaponSpecial;
        public int CharaRedId;
        public int CharaBlueId;
        public int PartHead;
        public int PartFace;
        public int PartJacket;
        public int PartPocket;
        public int PartGlove;
        public int PartBelt;
        public int PartHolster;
        public int PartSkin;
        public int BeretItem;
        public int DinoItem;
        public int AccessoryId;
        public int SprayId;
        public int NameCardId;
        public PlayerEquipment()
        {
            WeaponPrimary = 103004;
            WeaponSecondary = 202003;
            WeaponMelee = 301001;
            WeaponExplosive = 407001;
            WeaponSpecial = 508001;
            CharaRedId = 601001;
            CharaBlueId = 602002;
            PartHead = 1000700000;
            PartFace = 1000800000;
            PartJacket = 1000900000;
            PartPocket = 1001000000;
            PartGlove = 1001100000;
            PartBelt = 1001200000;
            PartHolster = 1001300000;
            PartSkin = 1001400000;
            DinoItem = 1500511;
        }
    }
}
