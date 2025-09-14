using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Models.Map;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Plugin.Core.XML
{
    public static class SystemMapXML
    {
        public static List<MapRule> Rules = new List<MapRule>();
        public static List<MapMatch> Matches = new List<MapMatch>();
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int limit)
        {
            return list.Select((item, inx) => new { item, inx }).GroupBy(x => x.inx / limit).Select(g => g.Select(x => x.item));
        }
        public static void Load()
        {
            LoadMapRule();
            LoadMapMatch();
        }
        private static void LoadMapRule()
        {
            string Path = "Data/Maps/Rules.xml";
            if (File.Exists(Path))
            {
                ParseMapRule(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Rules.Count} Map Rules", LoggerType.Info);
        }
        private static void LoadMapMatch()
        {
            string Path = "Data/Maps/Matches.xml";
            if (File.Exists(Path))
            {
                ParseMapMatch(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Matches.Count} Map Matches", LoggerType.Info);
        }
        public static MapRule GetMapRule(int RuleId)
        {
            lock (Rules)
            {
                foreach (MapRule Rule in Rules)
                {
                    if (Rule.Id == RuleId)
                    {
                        return Rule;
                    }
                }
                return null;
            }
        }
        public static MapMatch GetMapLimit(int MapId, int RuleId)
        {
            lock (Rules)
            {
                foreach (MapMatch Match in Matches)
                {
                    if (Match.Id == MapId && GetMapRule(Match.Mode).Rule == RuleId)
                    {
                        return Match;
                    }
                }
                return null;
            }
        }
        private static void ParseMapRule(string Path)
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
                                    if ("Mode".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap Xml = Node2.Attributes;
                                        MapRule Rule = new MapRule()
                                        {
                                            Id = int.Parse(Xml.GetNamedItem("Id").Value),
                                            Rule = byte.Parse(Xml.GetNamedItem("Rule").Value),
                                            StageOptions = byte.Parse(Xml.GetNamedItem("StageOptions").Value),
                                            Conditions = byte.Parse(Xml.GetNamedItem("Conditions").Value),
                                            Name = Xml.GetNamedItem("Name").Value
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
        private static void ParseMapMatch(string Path)
        {
            XmlDocument XmlDocument = new XmlDocument();
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
                        XmlDocument.Load(Stream);
                        for (XmlNode NodeList = XmlDocument.FirstChild; NodeList != null; NodeList = NodeList.NextSibling)
                        {
                            if ("List".Equals(NodeList.Name))
                            {
                                for (XmlNode Node = NodeList.FirstChild; Node != null; Node = Node.NextSibling)
                                {
                                    if ("Match".Equals(Node.Name))
                                    {
                                        XmlNamedNodeMap Xml = Node.Attributes;
                                        int RuleId = int.Parse(Xml.GetNamedItem("Rule").Value);
                                        string Mode = Xml.GetNamedItem("Mode").Value;
                                        if (RuleId == 0 || string.IsNullOrEmpty(Mode))
                                        {
                                            CLogger.Print($"Invalid Mode: {RuleId}; Mode Name: {Mode}; Please check and try again!", LoggerType.Warning);
                                            return;
                                        }
                                        ListMaps(Node, RuleId);
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
        private static void ListMaps(XmlNode XmlNode, int RuleId)
        {
            for (XmlNode NodeList = XmlNode.FirstChild; NodeList != null; NodeList = NodeList.NextSibling)
            {
                if ("Count".Equals(NodeList.Name))
                {
                    for (XmlNode Node = NodeList.FirstChild; Node != null; Node = Node.NextSibling)
                    {
                        if ("Map".Equals(Node.Name))
                        {
                            XmlNamedNodeMap Xml = Node.Attributes;
                            MapMatch Match = new MapMatch(RuleId)
                            {
                                Id = byte.Parse(Xml.GetNamedItem("Id").Value),
                                Limit = byte.Parse(Xml.GetNamedItem("Limit").Value),
                                Tag = byte.Parse(Xml.GetNamedItem("Tag").Value),
                                Name = Xml.GetNamedItem("Name").Value
                            };
                            Matches.Add(Match);
                        }
                    }
                }
            }
        }
    }
}