using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Plugin.Core.JSON
{
    public class ServerConfigJSON
    {
        public static List<ServerConfig> Configs = new List<ServerConfig>();
        public static void Load()
        {
            string Path = "Data/ServerConfig.json";
            if (File.Exists(Path))
            {
                Parse(Path);
            }
            else
            {
                CLogger.Print($"File not found: {Path}", LoggerType.Warning);
            }
            CLogger.Print($"Plugin Loaded: {Configs.Count} Server Configs", LoggerType.Info);
        }
        public static ServerConfig GetConfig(int ConfigId)
        {
            lock (Configs)
            {
                foreach (ServerConfig Config in Configs)
                {
                    if (Config.ConfigId == ConfigId)
                    {
                        return Config;
                    }
                }
                return null;
            }
        }
        private static void Parse(string Path)
        {
            using (FileStream STR = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                if (STR.Length == 0)
                {
                    CLogger.Print($"File is empty: {Path}", LoggerType.Warning);
                }
                else
                {
                    try
                    {
                        using (StreamReader Stream = new StreamReader(STR, Encoding.UTF8))
                        {
                            JsonDocument DataDeserialize = JsonDocument.Parse(Stream.ReadToEnd());
                            foreach (JsonElement Element in DataDeserialize.RootElement.GetProperty("Server").EnumerateArray())
                            {
                                int ConfigId = int.Parse(Element.GetProperty("ConfigId").GetString());
                                if (ConfigId == 0)
                                {
                                    CLogger.Print($"Invalid Config Id: {ConfigId}", LoggerType.Warning);
                                    return;
                                }
                                ServerConfig Config = new ServerConfig()
                                {
                                    ConfigId = ConfigId,
                                    OnlyGM = bool.Parse(Element.GetProperty("ChannelOnlyGM").GetString()),
                                    Missions = bool.Parse(Element.GetProperty("EnableMissions").GetString()),
                                    AccessUFL = bool.Parse(Element.GetProperty("AccessUFL").GetString()),
                                    UserFileList = Element.GetProperty("UserFileList").GetString(),
                                    ClientVersion = Element.GetProperty("ClientVersion").GetString(),
                                    GiftSystem = bool.Parse(Element.GetProperty("EnableGiftSystem").GetString()),
                                    EnableClan = bool.Parse(Element.GetProperty("EnableClan").GetString()),
                                    EnableTicket = bool.Parse(Element.GetProperty("EnableTicket").GetString()),
                                    EnableTags = bool.Parse(Element.GetProperty("EnableTags").GetString()),
                                    EnableBlood = bool.Parse(Element.GetProperty("EnableBlood").GetString()),
                                    ExitURL = Element.GetProperty("ExitURL").GetString(),
                                    ShopURL = Element.GetProperty("ShopURL").GetString(),
                                    OfficialSteam = Element.GetProperty("OfficialSteam").GetString(),
                                    OfficialBannerEnabled = bool.Parse(Element.GetProperty("OfficialBannerEnabled").GetString()),
                                    OfficialBanner = Element.GetProperty("OfficialBanner").GetString(),
                                    OfficialAddress = Element.GetProperty("OfficialAddress").GetString(),
                                    ChatAnnounceColor = int.Parse(Element.GetProperty("ChatAnnoucementColor").GetString()),
                                    ChannelAnnounceColor = int.Parse(Element.GetProperty("ChannelAnnoucementColor").GetString()),
                                    ChatAnnounce = Element.GetProperty("ChatAnnountcement").GetString(),
                                    ChannelAnnouce = Element.GetProperty("ChannelAnnouncement").GetString(),
                                    Showroom = ComDiv.ParseEnum<ShowroomView>(Element.GetProperty("Showroom").GetString())
                                };
                                Configs.Add(Config);
                            }
                            Stream.Dispose();
                            Stream.Close();
                        }
                    }
                    catch (Exception Ex)
                    {
                        CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                    }
                }
                STR.Dispose();
                STR.Close();
            }
        }
    }
}
