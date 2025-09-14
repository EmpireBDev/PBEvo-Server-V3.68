using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_RANDOMBOX_LIST_REQ : GameClientPacket
    {
        private string MD5Hash;
        public PROTOCOL_BASE_RANDOMBOX_LIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MD5Hash = ReadS(32);
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
                if (Bitwise.ReadFile($"{Environment.CurrentDirectory}/Data/Raws/RandomBox.dat") == MD5Hash)
                {
                    Client.SendPacket(new PROTOCOL_BASE_RANDOMBOX_LIST_ACK(false));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_BASE_RANDOMBOX_LIST_ACK(true));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_RANDOMBOX_LIST_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}