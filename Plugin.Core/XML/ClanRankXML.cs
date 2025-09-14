using Plugin.Core.Enums;
using Plugin.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class ClanRankXML
    {
        private static List<RankModel> Ranks = new List<RankModel>();
        public static void Load()
        {
            string Path = "Data/Ranks/Clan.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Ranks.Count} Clan Ranks", LoggerType.Info);
        }
        public static RankModel GetRank(int Id)
        {
            lock (Ranks)
            {
                foreach (RankModel Rank in Ranks)
                {
                    if (Rank.Id == Id)
                    {
                        return Rank;
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
                                    if ("Rank".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        RankModel Rank = new RankModel(byte.Parse(xml.GetNamedItem("Id").Value))
                                        {
                                            Title = xml.GetNamedItem("Title").Value,
                                            OnNextLevel = int.Parse(xml.GetNamedItem("OnNextLevel").Value),
                                            OnGoldUp = 0,
                                            OnAllExp = int.Parse(xml.GetNamedItem("OnAllExp").Value)
                                        };
                                        Ranks.Add(Rank);
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
