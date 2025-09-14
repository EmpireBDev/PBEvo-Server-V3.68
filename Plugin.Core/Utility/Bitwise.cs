using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Plugin.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Plugin.Core.Utility
{
    public static class Bitwise
    {
        private static readonly string NEWLINE = string.Format("\n", new object[0]);
        private static readonly string STRLINE = string.Format("", new object[0]);
        private static readonly char[] BYTE2CHAR = new char[256];
        private static readonly string[] BYTE2HEX = new string[256];
        private static readonly string[] HEXPADDING = new string[16];
        private static readonly string[] BYTEPADDING = new string[16];
        public static readonly int[] CRYPTO = new int[3] { 29890, 32759, 1360 };
        static Bitwise()
        {
            int i;
            for (i = 0; i < 10; i++)
            {
                StringBuilder Builder = new StringBuilder(3);
                Builder.Append(" 0");
                Builder.Append(i);
                BYTE2HEX[i] = Builder.ToString().ToUpper();
            }
            for (; i < 16; i++)
            {
                StringBuilder Builder = new StringBuilder(3);
                Builder.Append(" 0");
                Builder.Append((char)(97 + i - 10));
                BYTE2HEX[i] = Builder.ToString().ToUpper();
            }
            for (; i < BYTE2HEX.Length; i++)
            {
                StringBuilder Builder = new StringBuilder(3);
                Builder.Append(' ');
                Builder.Append(i.ToString("X"));
                BYTE2HEX[i] = Builder.ToString().ToUpper();
            }
            for (i = 0; i < HEXPADDING.Length; i++)
            {
                int Padding = HEXPADDING.Length - i;
                StringBuilder Builder = new StringBuilder(Padding * 3);
                for (int x = 0; x < Padding; x++)
                {
                    Builder.Append("   ");
                }
                HEXPADDING[i] = Builder.ToString().ToUpper();
            }
            for (i = 0; i < BYTEPADDING.Length; i++)
            {
                int Padding = BYTEPADDING.Length - i;
                StringBuilder Builder = new StringBuilder(Padding);
                for (int x = 0; x < Padding; x++)
                {
                    Builder.Append(' ');
                }
                BYTEPADDING[i] = Builder.ToString().ToUpper();
            }
            for (i = 0; i < BYTE2CHAR.Length; i++)
            {
                if (i <= 31 || i >= 127)
                {
                    BYTE2CHAR[i] = '.';
                }
                else
                {
                    BYTE2CHAR[i] = (char)i;
                }
            }
        }
        public static byte[] Decrypt(byte[] Data, int Shift)
        {
            byte[] NewBuffer = new byte[Data.Length];
            Array.Copy(Data, 0, NewBuffer, 0, NewBuffer.Length);
            byte LastElement = NewBuffer[NewBuffer.Length - 1];
            for (int i = NewBuffer.Length - 1; i > 0; --i)
            {
                NewBuffer[i] = (byte)((NewBuffer[i - 1] & 255) << 8 - Shift | (NewBuffer[i] & 255) >> Shift);
            }
            NewBuffer[0] = (byte)(LastElement << 8 - Shift | (NewBuffer[0] & 255) >> Shift);
            return NewBuffer;
        }
        public static byte[] Encrypt(byte[] Data, int Shift)
        {
            byte[] NewBuffer = new byte[Data.Length];
            Array.Copy(Data, 0, NewBuffer, 0, NewBuffer.Length);
            byte FirstElement = NewBuffer[0];
            for (int i = 0; i < NewBuffer.Length - 1; ++i)
            {
                NewBuffer[i] = (byte)((NewBuffer[i + 1] & 255) >> 8 - Shift | (NewBuffer[i] & 255) << Shift);
            }
            NewBuffer[NewBuffer.Length - 1] = (byte)(FirstElement >> 8 - Shift | (NewBuffer[NewBuffer.Length - 1] & 255) << Shift);
            return NewBuffer;
        }
        public static byte[] DEC(byte[] Data, int Shift)
        {
            byte[] Result = new byte[Data.Length];
            Buffer.BlockCopy(Data, 0, Result, 0, Result.Length);
            byte Last = Result[Result.Length - 1];
            for (int i = Result.Length - 1; (i & 0x80000000) == 0; i--)
            {
                int Index = (i <= 0 ? Last : (Result[i - 1] & 255));
                Result[i] = (byte)((Index << (8 - Shift)) | ((Result[i] & 255) >> Shift));
            }
            return Result;
        }
        public static byte[] ENC(byte[] Data, int Shift)
        {
            byte[] Result = new byte[Data.Length];
            Buffer.BlockCopy(Data, 0, Result, 0, Result.Length);
            byte First = Result[0];
            for (int i = 0; i < Result.Length; i++)
            {
                int Current = ((i >= Result.Length - 1) ? First : (Result[i + 1] & 255));
                Result[i] = (byte)((Current >> (8 - Shift)) | ((Result[i] & 255) << Shift));
            }
            return Result;
        }
        public static string ToHexData(string EventName, byte[] BuffData)
        {
            int Length = BuffData.Length, ReaderIndex = 0, WritedIndex = BuffData.Length;
            int Rows = Length / 16 + ((Length % 15 == 0) ? 0 : 1) + 4;
            StringBuilder DumpData = new StringBuilder(Rows * 80 + EventName.Length + 16);
            DumpData.Append($"{STRLINE}+--------+-------------------------------------------------+----------------+");
            DumpData.Append($"{NEWLINE}[!] {EventName}; Length: [{BuffData.Length} Bytes] </>");
            DumpData.Append($"{NEWLINE}         +-------------------------------------------------+");
            DumpData.Append($"{NEWLINE}         |  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F |");
            DumpData.Append($"{NEWLINE}+--------+-------------------------------------------------+----------------+");
            int IndexData;
            for (IndexData = ReaderIndex; IndexData < WritedIndex; IndexData++)
            {
                int RelIndex = IndexData - ReaderIndex;
                int RelIndexMod = RelIndex & 0xF;
                if (RelIndexMod == 0)
                {
                    DumpData.Append(NEWLINE);
                    DumpData.Append((RelIndex & 0xFFFFFFFFL | 0x100000000L).ToString("X"));
                    DumpData[DumpData.Length - 9] = '|';
                    DumpData.Append('|');
                }
                DumpData.Append(BYTE2HEX[BuffData[IndexData]]);
                if (RelIndexMod == 15)
                {
                    DumpData.Append(" |");
                    for (int ChildData = IndexData - 15; ChildData <= IndexData; ChildData++)
                    {
                        DumpData.Append(BYTE2CHAR[BuffData[ChildData]]);
                    }
                    DumpData.Append('|');
                }
            }
            if ((IndexData - ReaderIndex & 0xF) != 0)
            {
                int Remainder = Length & 0xF;
                DumpData.Append(HEXPADDING[Remainder]);
                DumpData.Append(" |");
                for (int ChildData = IndexData - Remainder; ChildData < IndexData; ChildData++)
                {
                    DumpData.Append(BYTE2CHAR[BuffData[ChildData]]);
                }
                DumpData.Append(BYTEPADDING[Remainder]);
                DumpData.Append('|');
            }
            DumpData.Append($"{NEWLINE}+--------+-------------------------------------------------+----------------+");
            return DumpData.ToString();
        }
        public static string HexArrayToString(byte[] Buffer, int Length)
        {
            string Text = "";
            try
            {
                Text = Encoding.Unicode.GetString(Buffer, 0, Length);
                int Index = Text.IndexOf(char.MinValue);
                if (Index != -1)
                {
                    Text = Text.Substring(0, Index);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Text;
        }
        public static byte[] HexStringToByteArray(string HexString)
        {
            string EncText = HexString.Replace(":", "").Replace("-", "").Replace(" ", "");
            byte[] Buffer = new byte[EncText.Length / 2];
            for (int i = 0; i < EncText.Length; i += 2)
            {
                Buffer[i / 2] = (byte)(ToByte(EncText.ElementAt(i)) << 4 | ToByte(EncText.ElementAt(i + 1)));
            }
            return Buffer;
        }
        private static int ToByte(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }
            return 0;
        }
        public static string ToByteString(byte[] Result)
        {
            string BytesData = "";
            foreach (string Byte in BitConverter.ToString(Result).Split('-', ',', '.', ':', '\t'))
            {
                BytesData += $" {Byte}";
            }
            return BytesData;
        }
        public static string HashToSHA(string Text)
        {
            string HashString = "";
            byte[] Keys = Encoding.UTF8.GetBytes("/x!a@r-$r%an¨.&e&+f*f(f(a)");
            using (HMACMD5 HashHMAC = new HMACMD5(Keys))
            {
                byte[] Data = HashHMAC.ComputeHash(Encoding.UTF8.GetBytes(Text));
                StringBuilder Builder = new StringBuilder();
                if (Builder != null)
                {
                    for (int i = 0; i < Data.Length; i++)
                    {
                        Builder.Append(Data[i].ToString("x2"));
                    }
                    HashString = Builder.ToString();
                }
            }
            return HashString;
        }
        public static List<byte[]> GenerateRSAKeyPair(int SessionId, int SecurityKey, int SeedLength)
        {
            List<byte[]> RSA_KEYS = new List<byte[]>();
            RsaKeyPairGenerator RSA_GEN = new RsaKeyPairGenerator();
            RSA_GEN.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), SeedLength));
            RsaKeyParameters RSA_PKEY = (RsaKeyParameters)RSA_GEN.GenerateKeyPair().Public;
            RSA_KEYS.Add(RSA_PKEY.Modulus.ToByteArrayUnsigned());
            RSA_KEYS.Add(RSA_PKEY.Exponent.ToByteArrayUnsigned());
            byte[] DEC_KEYS = BitConverter.GetBytes(SessionId + SecurityKey);
            Array.Copy(DEC_KEYS, 0, RSA_KEYS[0], 0, Math.Min(DEC_KEYS.Length, RSA_KEYS[0].Length));
            return RSA_KEYS;
        }
        public static ushort GenUHRNG()
        {
            using (RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider())
            {
                byte[] Bytes = new byte[2];
                RNG.GetBytes(Bytes);
                return BitConverter.ToUInt16(Bytes, 0);
            }
        }
        public static uint GenUDRNG()
        {
            using (RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider())
            {
                byte[] Bytes = new byte[4];
                RNG.GetBytes(Bytes);
                return BitConverter.ToUInt32(Bytes, 0);
            }
        }
        public static string ReadFile(string Path)
        {
            string Result = "";
            using (MD5 Crypto = MD5.Create())
            {
                FileInfo FI = new FileInfo(Path);
                using (FileStream Stream = FI.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                {
                    Result = BitConverter.ToString(Crypto.ComputeHash(Stream)).Replace("-", string.Empty);
                    Stream.Close();
                }
            }
            return Result;
        }
        public static byte[] ArrayRandomize(byte[] Source)
        {
            if (Source == null)
            {
                return null;
            }
            if (Source.Length < 2)
            {
                return Source;
            }
            int Length = Source.Length;
            int[] Positions = new int[Length];
            byte[] Target = new byte[Length];
            for (int i = 0; i < Positions.Length; i++)
            {
                Positions[i] = -1;
            }
            Random RND = new Random();
            for (int i = 0; i < Positions.Length; i++)
            {
                int Position = -1;
                bool AlreadyAssigned = false;
                bool First = true;
                while (First || AlreadyAssigned)
                {
                    First = AlreadyAssigned = false;
                    Position = RND.Next(Length);
                    foreach (int assigned in Positions)
                    {
                        if (assigned == Position)
                        {
                            AlreadyAssigned = true;
                            break;
                        }
                    }
                }
                Positions[i] = Position;
            }
            for (int i = 0; i < Positions.Length; i++)
            {
                Target[Positions[i]] = Source[i];
            }
            return Target;
        }
    }
}
