using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Auth.Data.Models;
using Server.Auth.Data.Utils;
using Server.Auth.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_INVEN_INFO_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_GET_INVEN_INFO_REQ(AuthClient client, byte[] data)
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
                List<ItemsModel> Items = AllUtils.LimitationIndex(Player, Player.Inventory.Items);
                if (Items.Count > 0)
                {
                    Client.SendPacket(new PROTOCOL_BASE_GET_INVEN_INFO_ACK(0, Items));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_GET_INVEN_INFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
