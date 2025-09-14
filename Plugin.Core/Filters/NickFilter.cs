using Plugin.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace Plugin.Core.Filters
{
    public static class NickFilter
    {
        public static List<string> Filters = new List<string>();
        public static void Load()
        {
            string Path = "Config/Filters/Nicks.txt";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Filters.Count} Nick Filters", LoggerType.Info);
        }
        private static void Parse(string Path)
        {
            string Line;
            try
            {
                using (StreamReader File = new StreamReader(Path))
                {
                    while ((Line = File.ReadLine()) != null)
                    {
                        Filters.Add(Line);
                    }
                    File.Close();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"Filter: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}