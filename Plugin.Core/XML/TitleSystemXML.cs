using Plugin.Core.Enums;
using Plugin.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class TitleSystemXML
    {
        private static List<TitleModel> Titles = new List<TitleModel>();
        public static void Load()
        {
            string path = "Data/Titles/System.xml";
            if (File.Exists(path))
            {
                Parse(path);
            }
            else
            {
                CLogger.Print($"File not found: {path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Titles.Count} Title System", LoggerType.Info);
        }
        public static TitleModel GetTitle(int titleId, bool ReturnNull = true)
        {
            if (titleId == 0)
            {
                if (!ReturnNull)
                {
                    return new TitleModel();
                }
                else
                {
                    return null;
                }
            }
            foreach (TitleModel title in Titles)
            {
                if (title.Id == titleId)
                {
                    return title;
                }
            }
            return null;
        }
        public static void Get2Titles(int titleId1, int titleId2, out TitleModel title1, out TitleModel title2, bool ReturnNull = true)
        {
            if (!ReturnNull)
            {
                title1 = new TitleModel();
                title2 = new TitleModel();
            }
            else
            {
                title1 = null;
                title2 = null;
            }
            if (titleId1 == 0 && titleId2 == 0)
            {
                return;
            }
            foreach (TitleModel title in Titles)
            {
                if (title.Id == titleId1)
                {
                    title1 = title;
                }
                else if (title.Id == titleId2)
                {
                    title2 = title;
                }
            }
        }
        public static void Get3Titles(int titleId1, int titleId2, int titleId3, out TitleModel title1, out TitleModel title2, out TitleModel title3, bool ReturnNull)
        {
            if (!ReturnNull)
            {
                title1 = new TitleModel();
                title2 = new TitleModel();
                title3 = new TitleModel();
            }
            else
            {
                title1 = null;
                title2 = null;
                title3 = null;
            }
            if (titleId1 == 0 && titleId2 == 0 && titleId3 == 0)
            {
                return;
            }
            foreach (TitleModel title in Titles)
            {
                if (title.Id == titleId1)
                {
                    title1 = title;
                }
                else if (title.Id == titleId2)
                {
                    title2 = title;
                }
                else if (title.Id == titleId3)
                {
                    title3 = title;
                }
            }
        }
        private static void Parse(string Path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(Path, FileMode.Open))
            {
                if (fileStream.Length == 0)
                {
                    CLogger.Print($"File is empty: {Path}", LoggerType.Warning);
                }
                else
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
                                    if ("Title".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        TitleModel Title = new TitleModel(int.Parse(xml.GetNamedItem("Id").Value))
                                        {
                                            ClassId = int.Parse(xml.GetNamedItem("List").Value),
                                            Ribbon = int.Parse(xml.GetNamedItem("Ribbon").Value),
                                            Ensign = int.Parse(xml.GetNamedItem("Ensign").Value),
                                            Medal = int.Parse(xml.GetNamedItem("Medal").Value),
                                            MasterMedal = int.Parse(xml.GetNamedItem("MasterMedal").Value),
                                            Rank = int.Parse(xml.GetNamedItem("Rank").Value),
                                            Slot = int.Parse(xml.GetNamedItem("Slot").Value),
                                            Req1 = int.Parse(xml.GetNamedItem("ReqT1").Value),
                                            Req2 = int.Parse(xml.GetNamedItem("ReqT2").Value)
                                        };
                                        Titles.Add(Title);
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
    }
}
