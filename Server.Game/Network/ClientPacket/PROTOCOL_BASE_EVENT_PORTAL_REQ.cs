using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_EVENT_PORTAL_REQ : GameClientPacket
    {
        private string MD5HASH;
        public PROTOCOL_BASE_EVENT_PORTAL_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MD5HASH = ReadS(32);
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                if (!Player.LoadedShop)
                {
                    Player.LoadedShop = true;
                }
                if (Bitwise.ReadFile($"{Environment.CurrentDirectory}/Data/Raws/EventPortal.dat") == MD5HASH)
                {
                    Client.SendPacket(new PROTOCOL_BASE_EVENT_PORTAL_ACK(false));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_BASE_EVENT_PORTAL_ACK(true));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_EVENT_PORTAL_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
