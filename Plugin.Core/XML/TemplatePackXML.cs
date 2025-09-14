using Plugin.Core.Models;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class TemplatePackXML
    {
        public static List<ItemsModel> Basics = new List<ItemsModel>();
        public static List<ItemsModel> Awards = new List<ItemsModel>();
        public static List<ItemsModel> CafePCs = new List<ItemsModel>();
        public static void Load()
        {
            LoadBasic();
            LoadCafePC();
            LoadAward();
        }
        private static void LoadBasic()
        {
            string Path = "Data/Templates/Basic.xml";
            if (File.Exists(Path))
            {
                ParseBasicItem(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Basics.Count} Basic Templates", LoggerType.Info);
        }
        private static void LoadCafePC()
        {
            string Path = "Data/Templates/CafePC.xml";
            if (File.Exists(Path))
            {
                ParseCafePCItem(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {CafePCs.Count} PC Cafe Templates", LoggerType.Info);
        }
        private static void LoadAward()
        {
            string Path = "Data/Templates/Award.xml";
            if (File.Exists(Path))
            {
                ParseAwardItem(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Awards.Count} Award Templates", LoggerType.Info);
        }
        private static void ParseBasicItem(string Path)
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
                                    if ("Item".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        int Id = int.Parse(xml.GetNamedItem("Id").Value);
                                        ItemsModel Item = new ItemsModel(Id)
                                        {
                                            ObjectId = ComDiv.ValidateStockId(Id),
                                            Name = xml.GetNamedItem("Name").Value,
                                            Count = 1,
                                            Equip = ItemEquipType.Permanent
                                        };
                                        Basics.Add(Item);
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
        private static void ParseCafePCItem(string Path)
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
                                    if ("Item".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        int Id = int.Parse(xml.GetNamedItem("Id").Value);
                                        ItemsModel Item = new ItemsModel(Id)
                                        {
                                            ObjectId = ComDiv.ValidateStockId(Id),
                                            Name = xml.GetNamedItem("Name").Value,
                                            Count = 1,
                                            Equip = ItemEquipType.CafePC
                                        };
                                        if (ComDiv.GetIdStatics(Item.Id, 1) == 6 || (ComDiv.GetIdStatics(Item.Id, 4) >= 7 && ComDiv.GetIdStatics(Item.Id, 4) <= 14))
                                        {
                                            CLogger.Print($"You can't use Character as PC Cafe Item!", LoggerType.Warning);
                                        }
                                        else
                                        {
                                            CafePCs.Add(Item);
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
        private static void ParseAwardItem(string Path)
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
                                    if ("Item".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        ItemsModel Item = new ItemsModel(int.Parse(xml.GetNamedItem("Id").Value))
                                        {
                                            Name = xml.GetNamedItem("Name").Value,
                                            Count = uint.Parse(xml.GetNamedItem("Count").Value),
                                            Equip = ItemEquipType.Durable
                                        };
                                        if (ComDiv.GetIdStatics(Item.Id, 1) == 6)
                                        {
                                            CLogger.Print($"You can't use Character as a Gift!", LoggerType.Warning);
                                        }
                                        else
                                        {
                                            Awards.Add(Item);
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
    }
}
