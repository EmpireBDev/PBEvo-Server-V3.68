using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Plugin.Core.Models;
using Plugin.Core.Enums;

namespace Plugin.Core.XML
{
    public class RandomBoxXML
    {
        public static SortedList<int, RandomBoxModel> RBoxes = new SortedList<int, RandomBoxModel>();
        public static void Load()
        {
            DirectoryInfo folder = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Data\RBoxes");
            if (!folder.Exists)
            {
                return;
            }
            foreach (FileInfo file in folder.GetFiles())
            {
                try
                {
                    LoadBox(int.Parse(file.Name.Substring(0, file.Name.Length - 4)));
                }
                catch (Exception Ex)
                {
                    CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                }
            }
            CLogger.Print($"Plugin Loaded: {RBoxes.Count} Random Boxes", LoggerType.Info);
        }
        private static void LoadBox(int id)
        {
            string path = "Data/RBoxes/" + id + ".xml";
            if (File.Exists(path))
            {
                Parse(path, id);
            }
            else
            {
                CLogger.Print("File not found: " + path, LoggerType.Warning);
            }
        }
        private static void Parse(string path, int cupomId)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream Stream = new FileStream(path, FileMode.Open))
            {
                if (Stream.Length == 0)
                {
                    CLogger.Print("File is empty: " + path, LoggerType.Warning);
                }
                else
                {
                    try
                    {
                        xmlDocument.Load(Stream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("List".Equals(xmlNode1.Name))
                            {
                                XmlNamedNodeMap xml2 = xmlNode1.Attributes;
                                RandomBoxModel RBox = new RandomBoxModel()
                                {
                                    ItemsCount = int.Parse(xml2.GetNamedItem("Count").Value)
                                };
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("Item".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        int ItemId = int.Parse(xml.GetNamedItem("Id").Value);
                                        uint ItemCount = uint.Parse(xml.GetNamedItem("Count").Value);
                                        ItemsModel Item = new ItemsModel(ItemId) { Name = xml.GetNamedItem("Name").Value, Equip = ItemEquipType.Durable, Count = ItemCount };
                                        if (Item != null)
                                        {
                                            RandomBoxItem RBI = new RandomBoxItem()
                                            {
                                                Index = int.Parse(xml.GetNamedItem("Index").Value),
                                                Percent = int.Parse(xml.GetNamedItem("Percent").Value),
                                                Special = bool.Parse(xml.GetNamedItem("Special").Value),
                                                Count = ItemCount,
                                                Item = Item
                                            };
                                            RBox.Items.Add(RBI);
                                        }
                                    }
                                }
                                RBox.SetTopPercent();
                                RBoxes.Add(cupomId, RBox);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CLogger.Print("[Box: " + cupomId + "] " + ex.Message, LoggerType.Error, ex);
                    }
                }
                Stream.Dispose();
                Stream.Close();
            }
        }
        public static bool ContainsBox(int id)
        {
            return RBoxes.ContainsKey(id);
        }
        public static RandomBoxModel GetBox(int id)
        {
            try
            {
                return RBoxes[id];
            }
            catch
            {
                return null;
            }
        }
    }
}
