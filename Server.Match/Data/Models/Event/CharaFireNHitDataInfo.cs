using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Match.Data.Models.Event
{
    public class CharaFireNHitDataInfo
    {
        public byte Extensions;
        public ushort X, Y, Z, Unk;
        public uint HitInfo;
        public int WeaponId;
    }
}
