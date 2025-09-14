using System.Collections.Generic;
using Plugin.Core.Models;
using System.IO;
using System.Xml;
using Plugin.Core.Enums;
using Plugin.Core.Utility;

namespace Plugin.Core.XML
{
    public class SChannelXML
    {
        public static List<SChannelModel> Servers = new List<SChannelModel>();
        public static void Load()
        {
            string Path = "Data/Server/SChannels.xml";
            if (File.Exists(Path))
            {
                ParseLoad(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Servers.Count} Server Channel", LoggerType.Info);
        }
        public static void UpdateServer(int ServerId)
        {
            string Path = "Data/Server/SChannels.xml";
            if (File.Exists(Path))
            {
                ParseReload(Path, ServerId);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Servers.Count} Server Channel (U)", LoggerType.Info);
        }
        public static SChannelModel GetServer(int id)
        {
            lock (Servers)
            {
                foreach (SChannelModel server in Servers)
                {
                    if (server.Id == id)
                    {
                        return server;
                    }
                }
                return null;
            }
        }
        private static void ParseLoad(string Path)
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
                                    if ("Server".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        SChannelModel SChannel = new SChannelModel(xml.GetNamedItem("Host").Value, ushort.Parse(xml.GetNamedItem("Port").Value))
                                        {
                                            Id = int.Parse(xml.GetNamedItem("Id").Value),
                                            State = bool.Parse(xml.GetNamedItem("State").Value),
                                            Type = ComDiv.ParseEnum<SChannelType>(xml.GetNamedItem("Type").Value),
                                            IsMobile = bool.Parse(xml.GetNamedItem("Mobile").Value),
                                            MaxPlayers = int.Parse(xml.GetNamedItem("MaxPlayers").Value),
                                            ChannelPlayers = int.Parse(xml.GetNamedItem("MaxPlayers").Value)
                                        };
                                        Servers.Add(SChannel);
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
        private static void ParseReload(string Path, int ServerId)
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
                                    if ("Server".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        SChannelModel Server = GetServer(ServerId);
                                        if (Server != null)
                                        {
                                            Server.State = bool.Parse(xml.GetNamedItem("State").Value);
                                            Server.Host = xml.GetNamedItem("Host").Value;
                                            Server.Port = ushort.Parse(xml.GetNamedItem("Port").Value);
                                            Server.Type = ComDiv.ParseEnum<SChannelType>(xml.GetNamedItem("Type").Value);
                                            Server.IsMobile = bool.Parse(xml.GetNamedItem("Mobile").Value);
                                            Server.MaxPlayers = int.Parse(xml.GetNamedItem("MaxPlayers").Value);
                                            Server.ChannelPlayers = int.Parse(xml.GetNamedItem("ChannelPlayers").Value);
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