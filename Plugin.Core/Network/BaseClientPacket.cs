using Plugin.Core.Enums;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Core.Network
{
    public abstract class BaseClientPacket
    {
		protected MemoryStream MStream;
		protected BinaryReader BReader;
		protected SafeHandle Handle;
		protected bool Disposed;
		protected int SECURITY_KEY;
		protected int HASH_CODE;
		protected int SEED_LENGTH;
		protected NationsEnum NATIONS;
		public BaseClientPacket()
		{
		}
		protected internal byte[] ReadB(int Length) => BReader.ReadBytes(Length);
		protected internal byte ReadC() => BReader.ReadByte();
		protected internal short ReadH() => BReader.ReadInt16();
		protected internal ushort ReadUH() => BReader.ReadUInt16();
		protected internal int ReadD() => BReader.ReadInt32();
		protected internal uint ReadUD() => BReader.ReadUInt32();
		protected internal float ReadT() => BReader.ReadSingle();
		protected internal long ReadQ() => BReader.ReadInt64();
		protected internal ulong ReadUQ() => BReader.ReadUInt64();
		protected internal double ReadF() => BReader.ReadDouble();
		protected internal string ReadN(int Length, string CodePage)
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
		protected internal string ReadS(int Length)
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
		protected internal string ReadU(int Length)
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
	}
}
