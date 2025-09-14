using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Server.Match.Data.Enums;

namespace Server.Match.Data.Models
{
    public class ObjectHitInfo
    {
        public int Type, ObjSyncId, ObjId, ObjLife, WeaponId, KillerId, AnimId1, AnimId2, DestroyState;
        public CharaDeath DeathType = CharaDeath.DEFAULT;
        public CharaHitPart HitPart;
        public byte Extensions;
        public Half3 Position;
        public float SpecialUse;
        public ObjectHitInfo(int Type)
        {
            this.Type = Type;
        }
    }
}