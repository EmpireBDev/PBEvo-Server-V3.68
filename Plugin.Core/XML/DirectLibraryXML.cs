using Plugin.Core.Enums;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class DirectLibraryXML
    {
        public static List<string> HashFiles = new List<string>();
        public static void Load()
        {
            string path = "Data/DirectLibrary.xml";
            if (File.Exists(path))
            {
                Load(path);
            }
            else
            {
                CLogger.Print("File not found: " + path, LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {HashFiles.Count} Lib Hases", LoggerType.Info);
        }
        public static bool IsValid(string md5)
        {
            if (string.IsNullOrEmpty(md5))
            {
                return true;
            }
            for (int i = 0; i < HashFiles.Count; i++)
            {
                if (HashFiles[i] == md5)
                {
                    return true;
                }
            }
            return false;
        }
        private static void Load(string Path)
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
                                    if ("D3D9".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        HashFiles.Add(xml.GetNamedItem("MD5").Value);
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