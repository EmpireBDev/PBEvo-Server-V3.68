namespace Server.Match.Data.Models.Event
{
    public class WeaponRecoilInfo
    {
        public float RecoilHorzAngle, RecoilHorzMax, RecoilVertAngle, RecoilVertMax, Deviation;
        public int WeaponId;
        public byte Extensions, Unk, RecoilHorzCount;
    }
}
