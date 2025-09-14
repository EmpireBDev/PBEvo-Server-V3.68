using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Globalization;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLEBOX_AUTH_REQ : GameClientPacket
    {
        private long ObjectId;
        private int TagPrice;
        public PROTOCOL_BATTLEBOX_AUTH_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ObjectId = ReadUD();
            TagPrice = ReadD();
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
                ItemsModel Item = Player.Inventory.GetItem(ObjectId);
                if (Item != null)
                {
                    BattleBoxModel BattleBox = ShopManager.GetBattleBox(Item.Id);
                    if (BattleBox != null)
                    {
                        if (BattleBox.Tags > Player.Tags)
                        {
                            Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Not Enoug Tags"));
                        }
                        else if (!DaoManagerSQL.UpdateAccountTags(Player.PlayerId, Player.Tags - BattleBox.Tags))
                        {
                            Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Unknown Network Error - 102"));
                        }
                        else
                        {
                            BattleBoxItem Box = BattleBox.Items[new Random().Next(BattleBox.Items.Count)];
                            Player.Tags -= BattleBox.Tags;
                            ComDiv.UpdateDB("accounts", "tags", Player.Tags, "player_id", Player.PlayerId);
                            ItemsModel Reward = Player.Inventory.GetItem(Box.Id);
                            if (Reward == null)
                            {
                                Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(Box.Id, Box.Name, ItemEquipType.Durable, Box.Count)));
                            }
                            else
                            {
                                if (Reward.Equip == ItemEquipType.Durable)
                                {
                                    Reward.Count += Box.Count;
                                }
                                else if (Reward.Equip == ItemEquipType.Temporary)
                                {
                                    DateTime data = DateTime.ParseExact(Reward.Count.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
                                    Reward.Count = uint.Parse(data.AddSeconds(Box.Count).ToString("yyMMddHHmm"));
                                }
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(1, Reward));
                            }
                            Client.SendPacket(new PROTOCOL_BATTLEBOX_AUTH_ACK(Player, Box.Id));
                            Item.Count -= 1;
                            if (Item.Count <= 0)
                            {
                                if (DaoManagerSQL.DeletePlayerInventoryItem(Item.ObjectId, Player.PlayerId))
                                {
                                    Player.Inventory.RemoveItem(Item);
                                }
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK(1, ObjectId));
                            }
                            else
                            {
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(1, Item));
                            }
                            if (GameXender.Client.Config.GiftSystem)
                            {
                                SendGiftMessage(Translation.GetLabel("GiftBattleBox"), new ItemsModel(Box.Id, Box.Name, ItemEquipType.Durable, Box.Count), Player, false);
                            }
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Unknown Network Error - 101"));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("Unknown Network Error - 100"));
                }
            }
            catch
            {

            }
        }
        public void SendGiftMessage(string From, ItemsModel Model, Account Player, bool ToComposeName)
        {
            string text = Translation.GetLabel("GiftText", Model.Name);
            if (ToComposeName)
            {
                if (Model.Equip == ItemEquipType.Durable)
                {
                    if (Model.Count < 3600)
                    {
                        Translation.GetLabel("GiftText", Model.Name + " (" + Model.Count + " qty)");
                    }
                    else if (Model.Count >= 3600 && Model.Count < 86400)
                    {
                        Translation.GetLabel("GiftText", Model.Name + " (" + Model.Count + " hours)");
                    }
                    else if (Model.Count >= 86400)
                    {
                        Translation.GetLabel("GiftText", Model.Name + " (" + (Model.Count / 86400) + " days)");
                    }
                }
            }
            MessageModel Message = CreateMessage("", 1, Player.PlayerId, text);
            if (Message != null)
            {
                Player.Connection.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(Message));
            }
        }
        public MessageModel CreateMessage(string SenderName, long OwnerId, long SenderId, string Text)
        {
            MessageModel msg = new MessageModel(15)
            {
                SenderName = SenderName,
                SenderId = SenderId,
                Text = Text,
                State = NoteMessageState.Unreaded
            };
            if (!DaoManagerSQL.CreateMessage(OwnerId, msg))
            {
                return null;
            }
            return msg;
        }
    }
}
