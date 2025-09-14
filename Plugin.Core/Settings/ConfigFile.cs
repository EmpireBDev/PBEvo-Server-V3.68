using Plugin.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace Plugin.Core.Settings
{
    public class ConfigFile
    {
        private readonly FileInfo File;
        private readonly SortedList<string, string> Topics;

        public ConfigFile(string path)
        {
            try
            {
                File = new FileInfo(path);
                Topics = new SortedList<string, string>();
                LoadStrings();
            }
            catch (Exception ex)
            {
                CLogger.Print($"ConfigFile: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void LoadStrings()
        {
            try
            {
                using (StreamReader reader = new StreamReader(File.FullName))
                {
                    while (!reader.EndOfStream)
                    {
                        string str = reader.ReadLine();
                        if (str.Length != 0 && !str.StartsWith(";") && !str.StartsWith("["))
                        {
                            string[] Split = str.Split(new string[] { " = " }, StringSplitOptions.None);
                            Topics.Add(Split[0], Split[1]);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public float ReadT(string value, float defaultprop)
        {
            float str;
            try
            {
                str = float.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public bool ReadX(string value, bool defaultprop)
        {
            bool str;
            try
            {
                str = bool.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public long ReadQ(string value, long defaultprop)
        {
            long str;
            try
            {
                str = long.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public ulong ReadUQ(string value, ulong defaultprop)
        {
            ulong str;
            try
            {
                str = ulong.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public int ReadD(string value, int defaultprop)
        {
            int str;
            try
            {
                str = int.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public uint ReadUD(string value, uint defaultprop)
        {
            uint str;
            try
            {
                str = uint.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public short ReadH(string value, short defaultprop)
        {
            short str;
            try
            {
                str = short.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public ushort ReadUH(string value, ushort defaultprop)
        {
            ushort str;
            try
            {
                str = ushort.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public byte ReadC(string value, byte defaultprop)
        {
            byte str;
            try
            {
                str = byte.Parse(Topics[value]);
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str;
        }
        public string ReadS(string value, string defaultprop)
        {
            string str;
            try
            {
                str = Topics[value];
            }
            catch
            {
                Error(value);
                return defaultprop;
            }
            return str == null ? defaultprop : str;
        }
        private void Error(string parameter)
        {
            CLogger.Print("Parameter failure: " + parameter, LoggerType.Warning);
        }
    }
}