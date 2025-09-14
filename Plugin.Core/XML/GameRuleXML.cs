using Plugin.Core.Enums;
using Plugin.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class GameRuleXML
    {
        public static List<GameRule> Rules = new List<GameRule>();
        public static void Load()
        {
            string Path = "Data/GameRules.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Rules.Count} Game Rules", LoggerType.Info);
        }
        public static void Reload()
        {
            Rules.Clear();
            Load();
        }
        public static List<GameRule> GetGameRules(int WeaponRule)
        {
            List<GameRule> List = new List<GameRule>();
            lock (Rules)
            {
                foreach (GameRule Rule in Rules)
                {
                    if (Rule.ItemId == WeaponRule)
                    {
                        List.Add(Rule);
                    }
                }
                return List;
            }
        }
        private static void Parse(string Path)
        {
            XmlDocument Document = new XmlDocument();
            using (FileStream Stream = new FileStream(Path, FileMode.Open))
            {
                if (Stream.Length == 0)
                {
                    CLogger.Print($"File is empty: {Path}", LoggerType.Warning);
                }
                else
                {
                    try
                    {
                        Document.Load(Stream);
                        for (XmlNode Node1 = Document.FirstChild; Node1 != null; Node1 = Node1.NextSibling)
                        {
                            if ("List".Equals(Node1.Name))
                            {
                                for (XmlNode Node2 = Node1.FirstChild; Node2 != null; Node2 = Node2.NextSibling)
                                {
                                    if ("Rule".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        GameRule Rule = new GameRule()
                                        {
                                            ItemId = int.Parse(xml.GetNamedItem("ItemId").Value),
                                            ItemName = xml.GetNamedItem("ItemName").Value
                                        };
                                        Rules.Add(Rule);
                                    }
                                }
                            }
                        }
                    }
                    catch (XmlException Ex)
                    {
                        CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                    }
                }
                Stream.Dispose();
                Stream.Close();
            }
        }
    }
}
