using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Data.Sync.Client
{
    public class InventorySync
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            long ObjectId = C.ReadQ();
            int ItemId = C.ReadD();
            ItemEquipType Equip = (ItemEquipType)C.ReadC();
            ItemCategory Category = (ItemCategory)C.ReadC();
            uint Count = C.ReadUD();
            byte NameLength = C.ReadC();
            string Name = C.ReadS(NameLength);
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player == null)
            {
                return;
            }
            ItemsModel Item = Player.Inventory.GetItem(ObjectId);
            if (Item == null)
            {
                ItemsModel CI = new ItemsModel() 
                { 
                    ObjectId = ObjectId, 
                    Id = ItemId, 
                    Equip = Equip, 
                    Count = Count, 
                    Category = Category, 
                    Name = Name 
                };
                Player.Inventory.AddItem(CI);
            }
            else
            {
                Item.Count = Count;
            }
        }
    }
}