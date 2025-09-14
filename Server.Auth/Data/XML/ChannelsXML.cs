using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Auth.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Auth.Data.XML
{
    public static class ChannelsXML
    {
        public static List<ChannelModel> Channels = new List<ChannelModel>();
        public static void Load()
        {
            string Path = "Data/Server/Channels.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
        }
        public static ChannelModel GetChannel(int ServerId, int Id)
        {
            lock (Channels)
            {
                foreach (ChannelModel Channel in Channels)
                {
                    if (Channel.ServerId == ServerId && Channel.Id == Id)
                    {
                        return Channel;
                    }
                }
                return null;
            }
        }
        public static List<ChannelModel> GetChannels(int ServerId)
        {
            List<ChannelModel> Channels = new List<ChannelModel>(11);
            for (int i = 0; i < ChannelsXML.Channels.Count; i++)
            {
                ChannelModel channel = ChannelsXML.Channels[i];
                if (channel.ServerId == ServerId)
                {
                    Channels.Add(channel);
                }
            }
            return Channels;
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
                        for (XmlNode Node1 = Document.FirstChild; Node1 != null; Node1 = Node1.NextSibling)
                        {
                            if ("List".Equals(Node1.Name))
                            {
                                for (XmlNode Node2 = Node1.FirstChild; Node2 != null; Node2 = Node2.NextSibling)
                                {
                                    if ("Channel".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        int ServerId = int.Parse(xml.GetNamedItem("ServerId").Value);
                                        ChannelList(Node2, ServerId);
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
        private static void ChannelList(XmlNode xmlNode, int ServerId)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("Count".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Setting".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            ChannelModel Channel = new ChannelModel(ServerId)
                            {
                                Id = int.Parse(xml4.GetNamedItem("Id").Value),
                                Type = ComDiv.ParseEnum<ChannelType>(xml4.GetNamedItem("Type").Value),
                                MaxRooms = int.Parse(xml4.GetNamedItem("MaxRooms").Value),
                                ExpBonus = int.Parse(xml4.GetNamedItem("ExpBonus").Value),
                                GoldBonus = int.Parse(xml4.GetNamedItem("GoldBonus").Value),
                                CashBonus = int.Parse(xml4.GetNamedItem("CashBonus").Value)
                            };
                            if (Channel.Type == ChannelType.CH_PW)
                            {
                                Channel.Password = xml4.GetNamedItem("Password").Value;
                            }
                            Channels.Add(Channel);
                        }
                    }
                }
            }
        }
    }
}