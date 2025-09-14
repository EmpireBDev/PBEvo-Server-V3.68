using Plugin.Core.Enums;
using Plugin.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Plugin.Core.XML
{
    public class PermissionXML
    {
        private static readonly SortedList<int, string> Permissions = new SortedList<int, string>();
        private static readonly SortedList<AccessLevel, List<string>> LevelsPermissions = new SortedList<AccessLevel, List<string>>();
        private static readonly SortedList<int, int> LevelsRanks = new SortedList<int, int>();
        public static void Load()
        {
            LoadPermission();
            LoadPermissionLevel();
            LoadPermissionRight();
        }
        private static void LoadPermission()
        {
            string Path = "Data/Access/Permission.xml";
            if (File.Exists(Path))
            {
                ParsePermission(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Permissions.Count} Permissions", LoggerType.Info);
        }
        private static void LoadPermissionLevel()
        {
            string Path = "Data/Access/PermissionLevel.xml";
            if (File.Exists(Path))
            {
                ParsePermissionLevel(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {LevelsPermissions.Count} Permission Ranks", LoggerType.Info);
        }
        private static void LoadPermissionRight()
        {
            string Path = "Data/Access/PermissionRight.xml";
            if (File.Exists(Path))
            {
                ParsePermissionRight(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {LevelsPermissions.Count} Level Permission", LoggerType.Info);
        }
        public static int GetFakeRank(int Level)
        {
            lock (LevelsRanks)
            {
                if (LevelsRanks.ContainsKey(Level))
                {
                    return LevelsRanks[Level];
                }
                return -1;
            }
        }
        public static bool HavePermission(string Permission, AccessLevel Level)
        {
            return LevelsPermissions.ContainsKey(Level) && LevelsPermissions[Level].Contains(Permission);
        }
        private static void ParsePermission(string Path)
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
                                    if ("Permission".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap Xml = Node2.Attributes;
                                        int Key = int.Parse(Xml.GetNamedItem("Key").Value);
                                        string Name = Xml.GetNamedItem("Name").Value;
                                        string Description = Xml.GetNamedItem("Description").Value;
                                        if (!Permissions.ContainsKey(Key))
                                        {
                                            Permissions.Add(Key, Name);
                                        }
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
        private static void ParsePermissionLevel(string Path)
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
                                    if ("Permission".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap Xml = Node2.Attributes;
                                        int Key = int.Parse(Xml.GetNamedItem("Key").Value);
                                        string Name = Xml.GetNamedItem("Name").Value;
                                        string Description = Xml.GetNamedItem("Description").Value;
                                        int FakeRank = int.Parse(Xml.GetNamedItem("FakeRank").Value);
                                        LevelsRanks.Add(Key, FakeRank);
                                        LevelsPermissions.Add((AccessLevel)Key, new List<string>());
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
        private static void ParsePermissionRight(string Path)
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
                                    if ("Access".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap Xml = Node2.Attributes;
                                        AccessLevel Permission = ComDiv.ConvertToEnum<AccessLevel>(Xml.GetNamedItem("Level").Value);
                                        AccessLevelRight(Node2, Permission);
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
        private static void AccessLevelRight(XmlNode xmlNode, AccessLevel Permission)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("Permission".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Right".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap Xml = xmlNode4.Attributes;
                            int PermissionLevel = int.Parse(Xml.GetNamedItem("LevelKey").Value);
                            if (Permissions.ContainsKey(PermissionLevel))
                            {
                                LevelsPermissions[Permission].Add(Permissions[PermissionLevel]);
                            }
                        }
                    }
                }
            }
        }
    }
}
