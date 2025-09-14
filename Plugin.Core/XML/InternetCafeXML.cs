using Plugin.Core.Enums;
using Plugin.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public static class InternetCafeXML
    {
        public static readonly List<InternetCafe> Cafes = new List<InternetCafe>();
        public static void Load()
        {
            string Path = "Data/InternetCafe.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Cafes.Count} PC Cafe Bonuses", LoggerType.Info);
        }
        public static InternetCafe GetICafe(int ConfigId)
        {
            lock (Cafes)
            {
                foreach (InternetCafe Cafe in Cafes)
                {
                    if (Cafe.ConfigId == ConfigId)
                    {
                        return Cafe;
                    }
                }
                return null;
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
                                    if ("Bonus".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        InternetCafe Cafe = new InternetCafe(int.Parse(xml.GetNamedItem("Id").Value))
                                        {
                                            BasicExp = int.Parse(xml.GetNamedItem("BasicExp").Value),
                                            BasicGold = int.Parse(xml.GetNamedItem("BasicGold").Value),
                                            PremiumExp = int.Parse(xml.GetNamedItem("PremiumExp").Value),
                                            PremiumGold = int.Parse(xml.GetNamedItem("PremiumGold").Value)
                                        };
                                        Cafes.Add(Cafe);
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
