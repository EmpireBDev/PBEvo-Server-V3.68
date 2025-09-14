using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Server.Match.Data.Enums;
using System.Collections.Generic;

namespace Server.Match.Data.Models.Event
{
    public class HitDataInfo
    {
        public byte Extensions, Unk;
        public ushort BoomInfo, ObjectId;
        public uint HitIndex;
        public int WeaponId;
        public Half3 StartBullet, EndBullet, BulletPos;
        public List<int> BoomPlayers;
        public HitType HitEnum;
        public ClassType WeaponClass;
    }
}
