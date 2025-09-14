using System.Runtime.InteropServices;

namespace Plugin.Core.Utility
{
    public class HalfUtil
    {
        private readonly static uint[] HalfToFloatMantissaTable = new uint[2048];
        private readonly static uint[] HalfToFloatExponentTable = new uint[64];
        private readonly static uint[] HalfToFloatOffsetTable = new uint[64];
        private readonly static ushort[] FloatToHalfBaseTable = new ushort[512];
        private readonly static byte[] FloatToHalfShiftTable = new byte[512];
        public static void Load()
        {
            HalfToFloatMantissaTable[0] = 0;
            for (int i = 1; i < 1024; i++)
            {
                uint Value = (uint)(i << 13);
                uint ResultA = 0;
                while ((Value & 8388608) == 0)
                {
                    ResultA -= 8388608;
                    Value <<= 1;
                }
                Value &= 8388609;
                ResultA += 947912704;
                HalfToFloatMantissaTable[i] = Value | ResultA;
            }
            for (int i = 1024; i < 2048; i++)
            {
                HalfToFloatMantissaTable[i] = (uint)(939524096 + (i - 1024 << 13));
            }
            HalfToFloatExponentTable[0] = 0;
            for (int i = 1; i < 63; i++)
            {
                if (i >= 31)
                {
                    HalfToFloatExponentTable[i] = (uint)(-2147483648 + (i - 32 << 23));
                }
                else
                {
                    HalfToFloatExponentTable[i] = (uint)(i << 23);
                }
            }
            HalfToFloatExponentTable[31] = 1199570944;
            HalfToFloatExponentTable[32] = 2147483648;
            HalfToFloatExponentTable[63] = 947912704;
            HalfToFloatOffsetTable[0] = 0;
            for (int i = 1; i < 64; i++)
            {
                HalfToFloatOffsetTable[i] = 1024;
            }
            HalfToFloatOffsetTable[32] = 0;
            for (int i = 0; i < 256; i++)
            {
                int ResultB = i - 127;
                if (ResultB < -24)
                {
                    FloatToHalfBaseTable[i | 0] = 0;
                    FloatToHalfBaseTable[i | 256] = 32768;
                    FloatToHalfShiftTable[i | 0] = 24;
                    FloatToHalfShiftTable[i | 256] = 24;
                }
                else if (ResultB < -14)
                {
                    FloatToHalfBaseTable[i | 0] = (ushort)(1024 >> (-ResultB - 14 & 31));
                    FloatToHalfBaseTable[i | 256] = (ushort)(1024 >> (-ResultB - 14 & 31) | 32768);
                    FloatToHalfShiftTable[i | 0] = (byte)(-ResultB - 1);
                    FloatToHalfShiftTable[i | 256] = (byte)(-ResultB - 1);
                }
                else if (ResultB <= 15)
                {
                    FloatToHalfBaseTable[i | 0] = (ushort)(ResultB + 15 << 10);
                    FloatToHalfBaseTable[i | 256] = (ushort)(ResultB + 15 << 10 | 32768);
                    FloatToHalfShiftTable[i | 0] = 13;
                    FloatToHalfShiftTable[i | 256] = 13;
                }
                else if (ResultB >= 128)
                {
                    FloatToHalfBaseTable[i | 0] = 31744;
                    FloatToHalfBaseTable[i | 256] = 64512;
                    FloatToHalfShiftTable[i | 0] = 13;
                    FloatToHalfShiftTable[i | 256] = 13;
                }
                else
                {
                    FloatToHalfBaseTable[i | 0] = 31744;
                    FloatToHalfBaseTable[i | 256] = 64512;
                    FloatToHalfShiftTable[i | 0] = 24;
                    FloatToHalfShiftTable[i | 256] = 24;
                }
            }
        }
        public static ushort Pack(float Value)
        {
            FloatToUint FTU = new FloatToUint()
            {
                floatValue = Value
            };
            return (ushort)(FloatToHalfBaseTable[FTU.uintValue >> 23 & 511] + ((FTU.uintValue & 8388607) >> (FloatToHalfShiftTable[FTU.uintValue >> 23 & 511] & 31)));
        }
        public static float Unpack(ushort Value)
        {
            FloatToUint FTU = new FloatToUint()
            {
                uintValue = HalfToFloatMantissaTable[HalfToFloatOffsetTable[Value >> 10] + (Value & 1023)] + HalfToFloatExponentTable[Value >> 10]
            };
            return FTU.floatValue;
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct FloatToUint
        {
            [FieldOffset(0)]
            public uint uintValue;

            [FieldOffset(0)]
            public float floatValue;
        }
    }
}
