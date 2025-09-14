using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Server.Match.Data.Enums;
using System.Collections.Generic;

namespace Server.Match.Data.Models.Event
{
    public class GrenadeHitInfo
    {
        public byte Extensions, Unk;
        public ushort BoomInfo, GrenadesCount, ObjectId;
        public uint HitInfo;
        public int WeaponId;
        public List<int> BoomPlayers;
        public CharaDeath DeathType;
        public Half3 FirePos, HitPos, PlayerPos;
        public HitType HitEnum;
        public ClassType WeaponClass;
    }
}
