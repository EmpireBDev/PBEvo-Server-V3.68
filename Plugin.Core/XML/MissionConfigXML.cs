using Plugin.Core.Enums;
using Plugin.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class MissionConfigXML
    {
        public static uint MissionPage1, MissionPage2;
        private static List<MissionStore> Missions = new List<MissionStore>();
        public static void Load()
        {
            string Path = "Data/MissionConfig.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Missions.Count} Mission Stores", LoggerType.Info);
        }
        public static int GetMissionPrice(int MissionId)
        {
            lock (Missions)
            {
                foreach (MissionStore Mission in Missions)
                {
                    if (Mission.Id == MissionId)
                    {
                        return Mission.Price;
                    }
                }
                return -1;
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
                                        bool EnableMission = bool.Parse(xml.GetNamedItem("Enable").Value);
                                        MissionStore Mission = new MissionStore()
                                        {
                                            Id = int.Parse(xml.GetNamedItem("Id").Value),
                                            Price = int.Parse(xml.GetNamedItem("Price").Value)
                                        };
                                        uint MissionFlag = (uint)(1 << Mission.Id);
                                        int MissionListId = (int)(Math.Ceiling(Mission.Id / 32.0));
                                        if (EnableMission)
                                        {
                                            if (MissionListId == 1)
                                            {
                                                MissionPage1 += MissionFlag;
                                            }
                                            else if (MissionListId == 2)
                                            {
                                                MissionPage2 += MissionFlag;
                                            }
                                        }
                                        Missions.Add(Mission);
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
