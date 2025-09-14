using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Server.Match.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Match.Data.XML
{
    public class MapStructureXML
    {
        public static List<MapModel> Maps = new List<MapModel>();
        public static void Load()
        {
            string path = "Data/Match/MapStructure.xml";
            if (File.Exists(path))
            {
                Parse(path);
            }
            else
            {
                CLogger.Print("File not found: " + path, LoggerType.Warning);
            }
        }
        public static void Reset()
        {
            Maps.Clear();
        }
        public static MapModel GetMapId(int MapId)
        {
            lock (Maps)
            {
                foreach (MapModel Map in Maps)
                {
                    if (Map.Id == MapId)
                    {
                        return Map;
                    }
                }
                return null;
            }
        }
        public static void SetObjectives(ObjectModel obj, RoomModel room)
        {
            if (obj.UltraSync == 0)
            {
                return;
            }
            if (obj.UltraSync == 1 || obj.UltraSync == 3)
            {
                room.Bar1 = obj.Life;
                room.Default1 = room.Bar1;
            }
            else if (obj.UltraSync == 2 || obj.UltraSync == 4)
            {
                room.Bar2 = obj.Life;
                room.Default2 = room.Bar2;
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
                                    if ("Map".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        MapModel map = new MapModel
                                        {
                                            Id = int.Parse(xml.GetNamedItem("Id").Value)
                                        };
                                        BombsXML(xmlNode2, map);
                                        ObjectsXML(xmlNode2, map);
                                        Maps.Add(map);
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
        private static void BombsXML(XmlNode xmlNode, MapModel map)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("BombPositions".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Bomb".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            BombPosition Bomb = new BombPosition()
                            {
                                X = float.Parse(xml4.GetNamedItem("X").Value),
                                Y = float.Parse(xml4.GetNamedItem("Y").Value),
                                Z = float.Parse(xml4.GetNamedItem("Z").Value)
                            };
                            Bomb.Position = new Half3(Bomb.X, Bomb.Y, Bomb.Z);
                            if (Bomb.X == 0 && Bomb.Y == 0 && Bomb.Z == 0)
                            {
                                Bomb.EveryWhere = true;
                            }
                            map.Bombs.Add(Bomb);
                        }
                    }
                }
            }
        }
        private static void ObjectsXML(XmlNode xmlNode, MapModel map)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("Objects".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Obj".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            ObjectModel obj = new ObjectModel(bool.Parse(xml4.GetNamedItem("NeedSync").Value))
                            {
                                Id = int.Parse(xml4.GetNamedItem("Id").Value),
                                Life = int.Parse(xml4.GetNamedItem("Life").Value),
                                Animation = int.Parse(xml4.GetNamedItem("Animation").Value)
                            };
                            if (obj.Life > -1)
                            {
                                obj.Destroyable = true;
                            }
                            if (obj.Animation > 255)
                            {
                                if (obj.Animation == 256)
                                {
                                    obj.UltraSync = 1;
                                }
                                else if (obj.Animation == 257)
                                {
                                    obj.UltraSync = 2;
                                }
                                else if (obj.Animation == 258)
                                {
                                    obj.UltraSync = 3;
                                }
                                else if (obj.Animation == 259)
                                {
                                    obj.UltraSync = 4;
                                }
                                obj.Animation = 255;
                            }
                            AnimsXML(xmlNode4, obj);
                            DeffectsXML(xmlNode4, obj);
                            map.Objects.Add(obj);
                        }
                    }
                }
            }
        }
        private static void AnimsXML(XmlNode xmlNode, ObjectModel obj)
        {
            for (XmlNode xmlNode5 = xmlNode.FirstChild; xmlNode5 != null; xmlNode5 = xmlNode5.NextSibling)
            {
                if ("Anims".Equals(xmlNode5.Name))
                {
                    for (XmlNode xmlNode6 = xmlNode5.FirstChild; xmlNode6 != null; xmlNode6 = xmlNode6.NextSibling)
                    {
                        if ("Sync".Equals(xmlNode6.Name))
                        {
                            XmlNamedNodeMap xml6 = xmlNode6.Attributes;
                            AnimModel anim = new AnimModel
                            {
                                Id = int.Parse(xml6.GetNamedItem("Id").Value),
                                Duration = float.Parse(xml6.GetNamedItem("Date").Value),
                                NextAnim = int.Parse(xml6.GetNamedItem("Next").Value),
                                OtherObj = int.Parse(xml6.GetNamedItem("OtherOBJ").Value),
                                OtherAnim = int.Parse(xml6.GetNamedItem("OtherANIM").Value)
                            };
                            if (anim.Id == 0)
                            {
                                obj.NoInstaSync = true;
                            }
                            if (anim.Id != 255)
                            {
                                obj.UpdateId = 3;
                            }
                            obj.Animations.Add(anim);
                        }
                    }
                }
            }
        }
        private static void DeffectsXML(XmlNode xmlNode, ObjectModel obj)
        {
            for (XmlNode xmlNode5 = xmlNode.FirstChild; xmlNode5 != null; xmlNode5 = xmlNode5.NextSibling)
            {
                if ("DestroyEffects".Equals(xmlNode5.Name))
                {
                    for (XmlNode xmlNode6 = xmlNode5.FirstChild; xmlNode6 != null; xmlNode6 = xmlNode6.NextSibling)
                    {
                        if ("Effect".Equals(xmlNode6.Name))
                        {
                            XmlNamedNodeMap xml6 = xmlNode6.Attributes;
                            DeffectModel anim = new DeffectModel
                            {
                                Id = int.Parse(xml6.GetNamedItem("Id").Value),
                                Life = int.Parse(xml6.GetNamedItem("Percent").Value)
                            };
                            obj.Effects.Add(anim);
                        }
                    }
                }
            }
        }
    }


}