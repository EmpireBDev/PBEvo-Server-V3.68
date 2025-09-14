using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Match.Data.Models.Event
{
    public class WeaponHost
    {
        public byte DeathType, HitPart;
        public ushort X, Y, Z;
        public int WeaponId;
    }
}
