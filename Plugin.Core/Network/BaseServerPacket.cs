using Plugin.Core.Enums;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Core.Network
{
	public abstract class BaseServerPacket
	{
		protected MemoryStream MStream;
		protected BinaryWriter BWriter;
		protected SafeHandle Handle;
		protected bool Disposed;
		protected int SECURITY_KEY;
		protected int HASH_CODE;
		protected int SEED_LENGTH;
		protected NationsEnum NATIONS;
		public BaseServerPacket()
		{
		}
		protected internal void WriteB(byte[] Value, int Offset, int Length) => BWriter.Write(Value, Offset, Length);
		protected internal void WriteB(byte[] Value) => BWriter.Write(Value);
		protected internal void WriteC(byte Value) => BWriter.Write(Value);
		protected internal void WriteH(ushort Value) => BWriter.Write(Value);
		protected internal void WriteH(short Value) => BWriter.Write(Value);
		protected internal void WriteD(uint Value) => BWriter.Write(Value);
		protected internal void WriteD(int Value) => BWriter.Write(Value);
		protected internal void WriteT(float Value) => BWriter.Write(Value);
		protected internal void WriteF(double Value) => BWriter.Write(Value);
		protected internal void WriteQ(ulong Value) => BWriter.Write(Value);
		protected internal void WriteQ(long Value) => BWriter.Write(Value);
		protected internal void WriteN(string Text, int Count, string CodePage)
		{
			if (Text == null)
			{
				return;
			}
			WriteB(Encoding.GetEncoding(CodePage).GetBytes(Text));
			WriteB(new byte[Count - Text.Length]);
		}
		protected internal void WriteS(string Text, int Count)
		{
			if (Text == null)
			{
				return;
			}
			WriteB(Encoding.UTF8.GetBytes(Text));
			WriteB(new byte[Count - Text.Length]);
		}
		protected internal void WriteU(string Text, int Count)
		{
			if (Text == null)
			{
				return;
			}
			WriteB(Encoding.Unicode.GetBytes(Text));
			WriteB(new byte[Count - (Text.Length * 2)]);
		}
	}
}