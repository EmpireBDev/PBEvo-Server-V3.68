using Plugin.Core.Enums;
using System.Collections.Generic;
using Plugin.Core.Models;
using System.IO;
using System.Xml;
using Plugin.Core.Utility;

namespace Plugin.Core.XML
{
    public static class CouponEffectXML
    {
        private static List<CouponFlag> Effects = new List<CouponFlag>();
        public static void Load()
        {
            string Path = "Data/CouponFlags.xml";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Effects.Count} Coupon Effects", LoggerType.Info);
        }
        public static CouponFlag GetCouponEffect(int id)
        {
            lock (Effects)
            {
                for (int i = 0; i < Effects.Count; i++)
                {
                    CouponFlag flag = Effects[i];
                    if (flag.ItemId == id)
                    {
                        return flag;
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
                                    if ("Coupon".Equals(Node2.Name))
                                    {
                                        XmlNamedNodeMap xml = Node2.Attributes;
                                        CouponFlag Coupon = new CouponFlag()
                                        {
                                            ItemId = int.Parse(xml.GetNamedItem("ItemId").Value),
                                            EffectFlag = ComDiv.ConvertToEnum<CouponEffects>(xml.GetNamedItem("EffectFlag").Value)
                                        };
                                        Effects.Add(Coupon);
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