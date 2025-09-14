using Plugin.Core.Enums;
using Plugin.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class SynchronizeXML
    {
        public static List<Synchronize> Servers = new List<Synchronize>();
        public static void Load()
        {
            string Path = "Data/Synchronize.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
        }
        public static Synchronize GetServer(int Port)
        {
            if (Servers.Count == 0)
            {
                return null;
            }
            try
            {
                lock (Servers)
                {
                    foreach (Synchronize Sync in Servers)
                    {
                        if (Sync.RemotePort == Port)
                        {
                            return Sync;
                        }
                    }
                    return null;
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
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
                        for (XmlNode xmlNode1 = Document.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("List".Equals(xmlNode1.Name))
                            {
                                XmlNamedNodeMap xml2 = xmlNode1.Attributes;
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("Sync".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        Synchronize Sync = new Synchronize(xml.GetNamedItem("Host").Value, int.Parse(xml.GetNamedItem("Port").Value))
                                        {
                                            RemotePort = int.Parse(xml.GetNamedItem("RemotePort").Value)
                                        };
                                        Servers.Add(Sync);
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
