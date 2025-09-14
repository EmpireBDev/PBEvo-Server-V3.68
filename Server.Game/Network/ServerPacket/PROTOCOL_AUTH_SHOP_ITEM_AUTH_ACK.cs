using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly ItemsModel Item;
        public PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(uint Error, ItemsModel Item = null, Account Player = null)
        {
            this.Error = Error;
            this.Item = Item;
            if (Item != null)
            {
                int StaticIdx = ComDiv.GetIdStatics(Item.Id, 1);
                if (StaticIdx == 17 || StaticIdx == 18 || StaticIdx == 20)
                {
                    if (Item.Count > 1 && Item.Equip == ItemEquipType.Durable)
                    {
                        ComDiv.UpdateDB("player_items", "count", (long)--Item.Count, "object_id", Item.ObjectId, "owner_id", Player.PlayerId);
                    }
                    else
                    {
                        DaoManagerSQL.DeletePlayerInventoryItem(Item.ObjectId, Player.PlayerId);
                        Player.Inventory.RemoveItem(Item);
                        Item.Id = 0;
                        Item.Count = 0;
                    }
                }
                else
                {
                    Item.Equip = ItemEquipType.Temporary;
                }
            }
            else
            {
                this.Error = 0x80000000;
            }
        }
        public override void Write()
        {
            WriteH(1048);
            WriteD(Error);
            if (Error == 1)
            {
                WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
                WriteD((uint)Item.ObjectId);
                if (Item.Category == ItemCategory.Coupon && Item.Equip == ItemEquipType.Temporary)
                {
                    WriteD(0);
                    WriteC(1);
                    WriteD(0);
                }
                else
                {
                    WriteD(Item.Id);
                    WriteC((byte)Item.Equip);
                    WriteD(Item.Count);
                }
            }
            //0x80001086 STR_TBL_GUI_BASE_NO_EQUIP_PRE_DESIGNATION
        }
    }
}