using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Match.Data.XML
{
    public class CharaStructureXML
    {
        public static List<CharaModel> Charas = new List<CharaModel>();
        public static void Load()
        {
            string path = "Data/Match/CharaHealth.xml";
            if (File.Exists(path))
            {
                Parse(path);
            }
            else
            {
                CLogger.Print("File not found: " + path, LoggerType.Warning);
            }
        }
        public static int GetCharaHP(int CharaId)
        {
            foreach (CharaModel Chara in Charas)
            {
                if (Chara.Id == CharaId)
                {
                    return Chara.HP;
                }
            }
            return 100;
        }
        private static void Parse(string Path)
        {
            XmlDocument xmlDocument = new XmlDocument();
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
                        xmlDocument.Load(Stream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("List".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("Chara".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        CharaModel Chara = new CharaModel()
                                        {
                                            Id = int.Parse(xml.GetNamedItem("Id").Value),
                                            HP = int.Parse(xml.GetNamedItem("HP").Value)
                                        };
                                        Charas.Add(Chara);
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
