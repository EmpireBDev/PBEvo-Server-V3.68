using Plugin.Core.Enums;

namespace Server.Match.Data.Models.SubHead
{
    public class GrenadeInfo
    {
        public byte Extensions, Unk7;
        public int WeaponId;
        public ClassType WeaponClass;
        public ushort ObjPos_X, ObjPos_Y, ObjPos_Z, BoomInfo, Unk1, Unk2, Unk3, Unk4, Unk5, Unk6, GrenadesCount;
        public byte[] Unknown;
    }
}
