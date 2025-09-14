using Microsoft.Win32.SafeHandles;
using Plugin.Core.SharpDX;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Core.Network
{
    public class SyncServerPacket : IDisposable
    {
        protected MemoryStream MStream;
        protected BinaryWriter BWriter;
        protected SafeHandle Handle;
        protected bool Disposed;
        public SyncServerPacket()
        {
            MStream = new MemoryStream();
            BWriter = new BinaryWriter(MStream);
            Handle = new SafeFileHandle(IntPtr.Zero, true);
            Disposed = false;
        }
        public SyncServerPacket(long Length)
        {
            MStream = new MemoryStream();
            MStream.SetLength(Length);
            BWriter = new BinaryWriter(MStream);
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
            BWriter.Dispose();
            if (Disposing)
            {
                Handle.Dispose();
            }
            Disposed = true;
        }
        public void WriteB(byte[] Value, int Offset, int Length) => BWriter.Write(Value, Offset, Length);
        public void WriteB(byte[] Value) => BWriter.Write(Value);
        public void WriteC(byte Value) => BWriter.Write(Value);
        public void WriteH(ushort Value) => BWriter.Write(Value);
        public void WriteH(short Value) => BWriter.Write(Value);
        public void WriteD(uint Value) => BWriter.Write(Value);
        public void WriteD(int Value) => BWriter.Write(Value);
        public void WriteT(float Value) => BWriter.Write(Value);
        public void WriteF(double Value) => BWriter.Write(Value);
        public void WriteQ(ulong Value) => BWriter.Write(Value);
        public void WriteQ(long Value) => BWriter.Write(Value);
        public void WriteN(string Name, int Count, string CodePage)
        {
            if (Name == null)
            {
                return;
            }
            WriteB(Encoding.GetEncoding(CodePage).GetBytes(Name));
            WriteB(new byte[Count - Name.Length]);
        }
        public void WriteS(string Text, int Count)
        {
            if (Text == null)
            {
                return;
            }
            WriteB(Encoding.UTF8.GetBytes(Text));
            WriteB(new byte[Count - Text.Length]);
        }
        public void WriteU(string Text, int Count)
        {
            if (Text == null)
            {
                return;
            }
            WriteB(Encoding.Unicode.GetBytes(Text));
            WriteB(new byte[Count - Text.Length * 2]);
        }
        public void GoBack(int Value)
        {
            BWriter.BaseStream.Position -= Value;
        }
        public void WriteHV(Half3 Half)
        {
            WriteH(Half.X.RawValue);
            WriteH(Half.Y.RawValue);
            WriteH(Half.Z.RawValue);
        }
        public void WriteTV(Half3 Half)
        {
            WriteT(Half.X);
            WriteT(Half.Y);
            WriteT(Half.Z);
        }
    }
}