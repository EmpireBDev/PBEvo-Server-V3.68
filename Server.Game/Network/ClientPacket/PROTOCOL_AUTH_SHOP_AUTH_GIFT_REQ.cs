using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ : GameClientPacket
    {
        private long msgId;
        public PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            msgId = ReadUD();
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
                if (Player.Inventory.Items.Count >= 500)
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(2147487785));
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(0x80000000));
                }
                else
                {
                    MessageModel msg = DaoManagerSQL.GetMessage(msgId, Player.PlayerId);
                    if (msg != null && msg.Type == NoteMessageType.Gift)
                    {
                        GoodsItem good = ShopManager.GetGood((int)msg.SenderId);
                        if (good != null)
                        {
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(1, good.Item, Player));
                            DaoManagerSQL.DeleteMessage(msgId, Player.PlayerId);
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_AUTH_GIFT_ACK(0x80000000));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}