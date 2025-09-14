using Plugin.Core.Enums;
using System.Collections.Generic;
using System.IO;

namespace Plugin.Core
{
    public static class Translation
    {
        public static SortedList<string, string> Strings = new SortedList<string, string>();
        public static void Load()
        {
            string[] Lines = File.ReadAllLines("Config/Translate/Strings.ini");
            foreach (string Line in Lines)
            {
                int Index = Line.IndexOf("=");
                if (Index >= 0)
                {
                    string Title = Line.Substring(0, Index);
                    string Text = Line.Substring(Index + 1);
                    Strings.Add(Title, Text);
                }
            }
            CLogger.Print($"Plugin Loaded: {Strings.Count} Translations", LoggerType.Info);
        }
        public static string GetLabel(string Title)
        {
            try
            {
                if (Strings.TryGetValue(Title, out string Value))
                {
                    return Value.Replace("\\n", ((char)0x0A).ToString());
                }
                return Title;
            }
            catch
            {
                return Title;
            }
        }
        public static string GetLabel(string Title, params object[] Argumens)
        {
            string Text = GetLabel(Title);
            return string.Format(Text, Argumens);
        }
    }
}