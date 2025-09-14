using Plugin.Core.Enums;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Plugin.Core.Settings
{
    public class ConfigEngine
    {
        #region Attachments
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string FilePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder RetVal, int Size, string FilePath);
        #endregion Attachments
        private readonly FileInfo FileInfo;
        private readonly FileAccess Access;
        private readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name;
        public ConfigEngine(string Path = null, FileAccess Access = FileAccess.ReadWrite)
        {
            this.Access = Access;
            FileInfo = new FileInfo(Path ?? EXE);
        }
        public byte ReadC(string Key, byte Defaultprop, string Section = null)
        {
            byte Value;
            try
            {
                Value = byte.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public short ReadH(string Key, short Defaultprop, string Section = null)
        {
            short Value;
            try
            {
                Value = short.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public ushort ReadUH(string Key, ushort Defaultprop, string Section = null)
        {
            ushort Value;
            try
            {
                Value = ushort.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public int ReadD(string Key, int Defaultprop, string Section = null)
        {
            int Value;
            try
            {
                Value = int.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public uint ReadUD(string Key, uint Defaultprop, string Section = null)
        {
            uint Value;
            try
            {
                Value = uint.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public long ReadQ(string Key, long Defaultprop, string Section = null)
        {
            long Value;
            try
            {
                Value = long.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public ulong ReadUQ(string Key, ulong Defaultprop, string Section = null)
        {
            ulong Value;
            try
            {
                Value = ulong.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public double ReadF(string Key, double Defaultprop, string Section = null)
        {
            double Value;
            try
            {
                Value = double.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public float ReadT(string Key, float Defaultprop, string Section = null)
        {
            float Value;
            try
            {
                Value = float.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public bool ReadX(string Key, bool Defaultprop, string Section = null)
        {
            bool Value;
            try
            {
                Value = bool.Parse(Read(Key, Section));
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        public string ReadS(string Key, string Defaultprop, string Section = null)
        {
            string Value;
            try
            {
                Value = Read(Key, Section);
            }
            catch
            {
                CLogger.Print($"Read Parameter Failure: {Key}", LoggerType.Warning);
                return Defaultprop;
            }
            return Value;
        }
        private string Read(string Key, string Section = null)
        {
            StringBuilder RetVal = new StringBuilder(65025);
            if (Access != FileAccess.Write)
            {
                GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 65025, FileInfo.FullName);
            }
            else
            {
                throw new Exception("Can`t read the file! No access!");
            }
            return RetVal.ToString();
        }
        public void WriteC(string Key, byte Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteH(string Key, short Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteH(string Key, ushort Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteD(string Key, int Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteD(string Key, uint Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteQ(string Key, long Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteQ(string Key, ulong Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteF(string Key, double Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteT(string Key, float Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteX(string Key, bool Value, string Section = null)
        {
            try
            {
                Write(Key, Value.ToString(), Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        public void WriteS(string Key, string Value, string Section = null)
        {
            try
            {
                Write(Key, Value, Section);
            }
            catch
            {
                CLogger.Print($"Write Parameter Failure: {Key}", LoggerType.Warning);
            }
        }
        private void Write(string Key, string Value, string Section = null)
        {
            if (Access != FileAccess.Read)
            {
                WritePrivateProfileString(Section ?? EXE, Key, $" {Value}", FileInfo.FullName);
            }
            else
            {
                throw new Exception("Can`t write to file! No access!");
            }
        }
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }
        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }
        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
