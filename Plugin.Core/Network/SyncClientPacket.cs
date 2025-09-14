using Microsoft.Win32.SafeHandles;
using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Core.Network
{
    public class SyncClientPacket : IDisposable
    {
        protected MemoryStream MStream;
        protected BinaryReader BReader;
        protected SafeHandle Handle;
        protected bool Disposed;
        public SyncClientPacket(byte[] Buffer)
        {
            MStream = new MemoryStream(Buffer, 0, Buffer.Length);
            BReader = new BinaryReader(MStream);
            Handle = new SafeFileHandle(IntPtr.Zero, true);
            Disposed = false;
        }
        public byte[] ToArray() => MStream.ToArray();
        public void SetMStream(MemoryStream MStream) => this.MStream = MStream;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool Disposing)
        {
            if (Disposed)
            {
                return;
            }
            MStream.Dispose();
            BReader.Dispose();
            if (Disposing)
            {
                Handle.Dispose();
            }
            Disposed = true;
        }
        public byte[] ReadB(int Length) => BReader.ReadBytes(Length);
        public byte ReadC() => BReader.ReadByte();
        public short ReadH() => BReader.ReadInt16();
        public ushort ReadUH() => BReader.ReadUInt16();
        public int ReadD() => BReader.ReadInt32();
        public uint ReadUD() => BReader.ReadUInt32();
        public float ReadT() => BReader.ReadSingle();
        public double ReadF() => BReader.ReadDouble();
        public long ReadQ() => BReader.ReadInt64();
        public ulong ReadUQ() => BReader.ReadUInt64();
        public string ReadN(int Length, string CodePage)
        {
            string Text = "";
            try
            {
                Text = Encoding.GetEncoding(CodePage).GetString(ReadB(Length));
                int Value = Text.IndexOf(char.MinValue);
                if (Value != -1)
                {
                    Text = Text.Substring(0, Value);
                }
            }
            catch
            {
            }
            return Text;
        }
        public string ReadS(int Length)
        {
            string Text = "";
            try
            {
                Text = Encoding.UTF8.GetString(ReadB(Length));
                int Value = Text.IndexOf(char.MinValue);
                if (Value != -1)
                {
                    Text = Text.Substring(0, Value);
                }
            }
            catch
            {
            }
            return Text;
        }
        public string ReadU(int Length)
        {
            string Text = "";
            try
            {
                Text = Encoding.Unicode.GetString(ReadB(Length));
                int Value = Text.IndexOf(char.MinValue);
                if (Value != -1)
                {
                    Text = Text.Substring(0, Value);
                }
            }
            catch
            {
            }
            return Text;
        }
        public byte ReadC(out bool Exception)
        {
            try
            {
                byte Result = BReader.ReadByte();
                Exception = false;
                return Result;
            }
            catch
            {
                Exception = true;
                return 0;
            }
        }
        public ushort ReadUH(out bool Exception)
        {
            try
            {
                ushort Result = BReader.ReadUInt16();
                Exception = false;
                return Result;
            }
            catch
            {
                Exception = true;
                return 0;
            }
        }
        public void Advance(int Bytes)
        {
            long Offset = BReader.BaseStream.Position += Bytes;
            if (Offset > BReader.BaseStream.Length)
            {
                CLogger.Print("Advance crashed.", LoggerType.Warning);
                throw new Exception("Offset has exceeded the buffer value.");
            }
        }
        public Half3 ReadUHV()
        {
            return new Half3(ReadUH(), ReadUH(), ReadUH());
        }
        public Half3 ReadTV()
        {
            return new Half3(ReadT(), ReadT(), ReadT());
        }
    }
}