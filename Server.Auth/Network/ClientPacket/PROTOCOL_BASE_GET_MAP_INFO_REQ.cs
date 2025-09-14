using Server.Auth.Network.ServerPacket;
using Plugin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Core.XML;
using Plugin.Core.Models;
using Plugin.Core.Enums;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_MAP_INFO_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_GET_MAP_INFO_REQ(AuthClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_BASE_MAP_RULELIST_ACK());
                IEnumerable<IEnumerable<MapMatch>> Parts = SystemMapXML.Matches.Split(100);
                int Total = 0;
                foreach (IEnumerable<MapMatch> Part in Parts)
                {
                    Total += 100;
                    List<MapMatch> List = Part.ToList();
                    if (List.Count > 0)
                    {
                        Client.SendPacket(new PROTOCOL_BASE_MAP_MATCHINGLIST_ACK(List, Total));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_GET_MAP_INFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
