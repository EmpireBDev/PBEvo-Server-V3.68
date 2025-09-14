using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_SHOP_REPAIR_REQ : GameClientPacket
    {
        private long ObjectId;
        private int State;
        public PROTOCOL_SHOP_REPAIR_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            State = ReadC();
            ObjectId = ReadD();
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
                List<ItemsModel> Items = AllUtils.RepairableItems(Player, ObjectId, State, out int Gold, out int Cash, out uint Error);
                if (Items.Count > 0)
                {
                    Player.Gold -= (Gold + ComDiv.Percentage(Gold, 10));
                    Player.Cash -= (Cash + ComDiv.Percentage(Cash, 10));
                    if (ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, new string[] { "gold", "cash" }, Player.Gold, Player.Cash))
                    {
                        foreach (ItemsModel Item in Items)
                        {
                            Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(3, Player, Item));
                        }
                    }
                    Client.SendPacket(new PROTOCOL_SHOP_REPAIR_ACK(Error, Items, Player));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_SHOP_REPAIR_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
