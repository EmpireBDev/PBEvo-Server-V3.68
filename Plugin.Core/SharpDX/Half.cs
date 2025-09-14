using Plugin.Core.Utility;

namespace Plugin.Core.SharpDX
{
    public struct Half
    {
        public ushort RawValue;
        public Half(float value)
        {
            RawValue = HalfUtil.Pack(value);
        }
        public Half(ushort rawvalue)
        {
            RawValue = rawvalue;
        }
        public static implicit operator float(Half value)
        {
            return HalfUtil.Unpack(value.RawValue);
        }
    }
}
