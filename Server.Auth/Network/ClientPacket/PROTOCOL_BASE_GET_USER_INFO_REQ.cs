using Plugin.Core;
using Server.Auth.Network.ServerPacket;
using System;
using Server.Auth.Data.Models;
using Server.Auth.Data.Utils;
using Plugin.Core.Models;
using System.Collections.Generic;
using Plugin.Core.Enums;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_USER_INFO_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_GET_USER_INFO_REQ(AuthClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
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
                List<ItemsModel> Items = Player.Inventory.Items;
                if (Items.Count == 0)
                {
                    AllUtils.LoadPlayerInventory(Player);
                    AllUtils.LoadPlayerMissions(Player);
                    AllUtils.ValidatePlayerInventoryStatus(Player);
                    AllUtils.DiscountPlayerItems(Player);
                    Client.SendPacket(new PROTOCOL_BASE_GET_USER_INFO_ACK(Player));
                    Client.SendPacket(new PROTOCOL_BASE_GET_CHARA_INFO_ACK(Player));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_GET_USER_INFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}