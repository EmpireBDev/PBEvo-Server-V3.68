using Server.Match.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Match.Data.Models.Event
{
    public class ActionStateInfo
    {
        public ActionFlag Action;
        public byte Value;
        public WeaponSyncType Flag;
    }
}
