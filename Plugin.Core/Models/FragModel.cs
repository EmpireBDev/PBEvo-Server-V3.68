using Plugin.Core.Enums;

namespace Plugin.Core.Models
{
    public class FragModel
    {
        public byte VictimWeaponClass;
        public byte HitspotInfo;
        public byte Flag;
        public KillingMessage KillFlag;
        public float X, Y, Z;
        public byte VictimSlot;
        public byte AssistSlot;
        public byte[] Unk;
        public FragModel()
        {
        }
        public FragModel(byte HitspotInfo)
        {
            SetHitspotInfo(HitspotInfo);
        }
        public void SetHitspotInfo(byte Value)
        {
            HitspotInfo = Value;
            VictimSlot = (byte)(Value & 15);
        }
    }
}