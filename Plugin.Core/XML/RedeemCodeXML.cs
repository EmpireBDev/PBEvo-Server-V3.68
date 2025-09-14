using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Plugin.Core.XML
{
    public class RedeemCodeXML
    {
        public static List<TicketModel> Tickets = new List<TicketModel>();
        public static void Load()
        {
            string Path = "Data/RedeemCodes.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Tickets.Count} Redeem Codes", LoggerType.Info);
        }
        public static TicketModel GetTicket(string RedeemCode)
        {
            lock (Tickets)
            {
                foreach (TicketModel Ticket in Tickets)
                {
                    if (Ticket.Token == RedeemCode)
                    {
                        return Ticket;
                    }
                }
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
                        for (XmlNode Node1 = Document.FirstChild; Node1 != null; Node1 = Node1.NextSibling)
                        {
                            if ("List".Equals(Node1.Name))
                            {
                                for (XmlNode Node2 = Node1.FirstChild; Node2 != null; Node2 = Node2.NextSibling)
                                {
                                    if ("Ticket".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        TicketModel Ticket = new TicketModel(ComDiv.ConvertToEnum<TicketType>(xml.GetNamedItem("Type").Value), xml.GetNamedItem("Token").Value, uint.Parse(xml.GetNamedItem("Count").Value), uint.Parse(xml.GetNamedItem("PlayerRation").Value));
                                        if (Ticket.Type.HasFlag(TicketType.ITEM))
                                        {
                                            ItemRewardXML(Node2, Ticket);
                                        }
                                        if (Ticket.Type.HasFlag(TicketType.VALUE))
                                        {
                                            Ticket.GoldReward = int.Parse(xml.GetNamedItem("GoldReward").Value);
                                            Ticket.CashReward = int.Parse(xml.GetNamedItem("CashReward").Value);
                                            Ticket.TagsReward = int.Parse(xml.GetNamedItem("TagsReward").Value);
                                        }
                                        Tickets.Add(Ticket);
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
        private static void ItemRewardXML(XmlNode xmlNode, TicketModel Ticket)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("Rewards".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Goods".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            Ticket.Rewards.Add(int.Parse(xml4.GetNamedItem("Id").Value));
                        }
                    }
                }
            }
        }
    }
}
