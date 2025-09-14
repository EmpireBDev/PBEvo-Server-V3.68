using Plugin.Core.Enums;
using Plugin.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class PlayerRankXML
    {
        public static readonly List<RankModel> Ranks = new List<RankModel>();
        public static void Load()
        {
            string Path = "Data/Ranks/Player.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Ranks.Count} Player Ranks", LoggerType.Info);
        }
        public static RankModel GetRank(int Id)
        {
            lock (Ranks)
            {
                foreach (RankModel rank in Ranks)
                {
                    if (rank.Id == Id)
                    {
                        return rank;
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
                                        RankModel Rank = new RankModel(int.Parse(xml.GetNamedItem("Id").Value))
                                        {
                                            Title = xml.GetNamedItem("Title").Value,
                                            OnNextLevel = int.Parse(xml.GetNamedItem("OnNextLevel").Value),
                                            OnGoldUp = int.Parse(xml.GetNamedItem("OnGoldUp").Value),
                                            OnAllExp = int.Parse(xml.GetNamedItem("OnAllExp").Value)
                                        };
                                        RewardXML(Node2, Rank);
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
        public static List<ItemsModel> GetRewards(int RankId)
        {
            SortedList<int, List<ItemsModel>> Rewards = GetRank(RankId).Rewards;
            lock (Rewards)
            {
                if (Rewards.TryGetValue(RankId, out List<ItemsModel> Items))
                {
                    return Items;
                }
            }
            return new List<ItemsModel>();
        }
        private static void RewardXML(XmlNode xmlNode, RankModel Rank)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("Rewards".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Item".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            ItemsModel Item = new ItemsModel(int.Parse(xml4.GetNamedItem("Id").Value))
                            {
                                Name = xml4.GetNamedItem("Name").Value,
                                Count = uint.Parse(xml4.GetNamedItem("Count").Value),
                                Equip = ItemEquipType.Durable,
                            };
                            AddItemToList(Rank, Item);
                        }
                    }
                }
            }
        }
        private static void AddItemToList(RankModel Rank, ItemsModel Item)
        {
            lock (Rank.Rewards)
            {
                if (Rank.Rewards.ContainsKey(Rank.Id))
                {
                    Rank.Rewards[Rank.Id].Add(Item);
                }
                else
                {
                    Rank.Rewards.Add(Rank.Id, new List<ItemsModel> { Item });
                }
            }
        }
    }
}
