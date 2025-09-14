using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class TitleAwardXML
    {
        public static List<TitleAward> Awards = new List<TitleAward>();
        public static void Load()
        {
            string path = "Data/Titles/Rewards.xml";
            if (File.Exists(path))
            {
                Parse(path);
            }
            else
            {
                CLogger.Print($"File not found: {path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Awards.Count} Title Awards", LoggerType.Info);
        }
        public static List<ItemsModel> GetAwards(int titleId)
        {
            List<ItemsModel> items = new List<ItemsModel>();
            lock (Awards)
            {
                foreach (TitleAward title in Awards)
                {
                    if (title.Id == titleId)
                    {
                        items.Add(title.Item);
                    }
                }
            }
            return items;
        }
        public static bool Contains(int TitleId, int ItemId)
        {
            if (ItemId == 0)
            {
                return false;
            }
            foreach (TitleAward Title in Awards)
            {
                if (Title.Id == TitleId && Title.Item.Id == ItemId)
                {
                    return true;
                }
            }
            return false;
        }
        private static void Parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length > 0)
                {
                    try
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("List".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("Award".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        RewardXML(xmlNode2, int.Parse(xml.GetNamedItem("TitleId").Value));
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
                fileStream.Dispose();
                fileStream.Close();
            }
        }
        private static void RewardXML(XmlNode xmlNode, int TitleId)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("Counts".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Item".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            TitleAward Award = new TitleAward() { Id = TitleId };
                            if (Award != null)
                            {
                                int Id = int.Parse(xml4.GetNamedItem("Id").Value);
                                ItemsModel Item = new ItemsModel(Id)
                                {
                                    Name = xml4.GetNamedItem("Name").Value,
                                    Count = uint.Parse(xml4.GetNamedItem("Count").Value),
                                    Equip = (ItemEquipType)int.Parse(xml4.GetNamedItem("Equip").Value)
                                };
                                if (Item.Equip == ItemEquipType.Permanent)
                                {
                                    Item.ObjectId = ComDiv.ValidateStockId(Id);
                                }
                                Award.Item = Item;
                                Awards.Add(Award);
                            }
                        }
                    }
                }
            }
        }
    }
}
