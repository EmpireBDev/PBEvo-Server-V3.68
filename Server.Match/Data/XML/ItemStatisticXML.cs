using Plugin.Core;
using Plugin.Core.Enums;
using Server.Match.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Match.Data.XML
{
    public class ItemStatisticXML
    {
        public static List<ItemsStatistic> Stats = new List<ItemsStatistic>();
        public static void Load()
        {
            string Path = "Data/Match/ItemStatistics.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
        }
        public static ItemsStatistic GetItemStats(int ItemId)
        {
            lock (Stats)
            {
                foreach (ItemsStatistic Stat in Stats)
                {
                    if (Stat.Id == ItemId)
                    {
                        return Stat;
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
                                    if ("Statistic".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap Xml = Node2.Attributes;
                                        ItemsStatistic Stat = new ItemsStatistic()
                                        {
                                            Id = int.Parse(Xml.GetNamedItem("Id").Value),
                                            Name = Xml.GetNamedItem("Name").Value,
                                            BulletLoaded = int.Parse(Xml.GetNamedItem("LoadedBullet").Value),
                                            BulletTotal = int.Parse(Xml.GetNamedItem("TotalBullet").Value),
                                            Damage = int.Parse(Xml.GetNamedItem("Damage").Value),
                                            FireDelay = float.Parse(Xml.GetNamedItem("FireDelay").Value),
                                            HelmetPenetrate = int.Parse(Xml.GetNamedItem("HelmetPenetrate").Value),
                                            Range = float.Parse(Xml.GetNamedItem("Range").Value)
                                        };
                                        Stats.Add(Stat);
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
