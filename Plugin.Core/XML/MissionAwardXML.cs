using Plugin.Core.Enums;
using Plugin.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class MissionAwardXML
    {
        private static List<MissionAwards> Awards = new List<MissionAwards>();
        public static void Load()
        {
            string Path = "Data/Cards/MissionAwards.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Awards.Count} Mission Awards", LoggerType.Info);
        }
        public static MissionAwards GetAward(int MissionId)
        {
            lock (Awards)
            {
                foreach (MissionAwards Mission in Awards)
                {
                    if (Mission.Id == MissionId)
                    {
                        return Mission;
                    }
                }
                return null;
            }
        }
        private static void Parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length == 0)
                {
                    CLogger.Print($"File is empty: {path}", LoggerType.Warning);
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
                                    if ("Mission".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        int id = int.Parse(xml.GetNamedItem("Id").Value);
                                        int blueOrder = int.Parse(xml.GetNamedItem("MasterMedal").Value);
                                        int exp = int.Parse(xml.GetNamedItem("Exp").Value);
                                        int gp = int.Parse(xml.GetNamedItem("Point").Value);
                                        Awards.Add(new MissionAwards(id, blueOrder, exp, gp));
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
