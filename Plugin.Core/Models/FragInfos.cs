using Plugin.Core.Enums;
using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class FragInfos
    {
        public byte KillerIdx, KillsCount, Flag, Unk;
        public CharaKillType KillingType;
        public int Weapon, Score;
        public float X, Y, Z;
        public List<FragModel> Frags = new List<FragModel>();
        public FragInfos()
        {
        }
        public KillingMessage GetAllKillFlags()
        {
            KillingMessage KillMsg = KillingMessage.None;
            foreach (FragModel Frag in Frags)
            {
                if (!KillMsg.HasFlag(Frag.KillFlag))
                {
                    KillMsg |= Frag.KillFlag;
                }
            }
            return KillMsg;
        }
    }
}