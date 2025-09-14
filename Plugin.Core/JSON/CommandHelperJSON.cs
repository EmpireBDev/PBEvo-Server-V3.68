using Plugin.Core.Enums;
using Plugin.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Plugin.Core.JSON
{
    public class CommandHelperJSON
    {
        public static List<CommandHelper> Helpers = new List<CommandHelper>();
        public static void Load()
        {
            string Path = "Data/CommandHelper.json";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Helpers.Count} Command Helpers", LoggerType.Info);
        }
        public static CommandHelper GetTag(string HelperTag)
        {
            lock (Helpers)
            {
                foreach (CommandHelper Helper in Helpers)
                {
                    if (Helper.Tag == HelperTag)
                    {
                        return Helper;
                    }
                }
                return null;
            }
        }
        private static void Parse(string Path)
        {
            using (FileStream STR = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                if (STR.Length == 0)
                {
                    CLogger.Print($"File is empty: {Path}", LoggerType.Warning);
                }
                else
                {
                    try
                    {
                        using (StreamReader Stream = new StreamReader(STR, Encoding.UTF8))
                        {
                            JsonDocument DataDeserialize = JsonDocument.Parse(Stream.ReadToEnd());
                            foreach (JsonElement Element in DataDeserialize.RootElement.GetProperty("Command").EnumerateArray())
                            {
                                string Tag = Element.GetProperty("Tag").GetString();
                                if (string.IsNullOrEmpty(Tag))
                                {
                                    CLogger.Print($"Invalid Command Helper Tag: {Tag}", LoggerType.Warning);
                                    return;
                                }
                                if (Tag.Equals("WeaponsFlag"))
                                {
                                    CommandHelper Helper = new CommandHelper(Tag)
                                    {
                                        AllWeapons = int.Parse(Element.GetProperty("AllWeapons").GetString()),
                                        AssaultRifle = int.Parse(Element.GetProperty("AssaultRifle").GetString()),
                                        SubMachineGun = int.Parse(Element.GetProperty("SubMachineGun").GetString()),
                                        SniperRifle = int.Parse(Element.GetProperty("SniperRifle").GetString()),
                                        ShotGun = int.Parse(Element.GetProperty("ShotGun").GetString()),
                                        MachineGun = int.Parse(Element.GetProperty("MachineGun").GetString()),
                                        Secondary = int.Parse(Element.GetProperty("Secondary").GetString()),
                                        Melee = int.Parse(Element.GetProperty("Melee").GetString()),
                                        Knuckle = int.Parse(Element.GetProperty("Knuckle").GetString()),
                                        RPG7 = int.Parse(Element.GetProperty("RPG7").GetString())
                                    };
                                    Helpers.Add(Helper);
                                }
                                if (Tag.Equals("PlayTime"))
                                {
                                    CommandHelper Helper = new CommandHelper(Tag)
                                    {
                                        Minutes05 = int.Parse(Element.GetProperty("Minutes05").GetString()),
                                        Minutes10 = int.Parse(Element.GetProperty("Minutes10").GetString()),
                                        Minutes15 = int.Parse(Element.GetProperty("Minutes15").GetString()),
                                        Minutes20 = int.Parse(Element.GetProperty("Minutes20").GetString()),
                                        Minutes25 = int.Parse(Element.GetProperty("Minutes25").GetString()),
                                        Minutes30 = int.Parse(Element.GetProperty("Minutes30").GetString())
                                    };
                                    Helpers.Add(Helper);
                                }
                            }
                            Stream.Dispose();
                            Stream.Close();
                        }
                    }
                    catch (Exception Ex)
                    {
                        CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                    }
                }
                STR.Dispose();
                STR.Close();
            }
        }
    }
}
