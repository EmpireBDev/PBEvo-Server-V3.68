using Plugin.Core.Enums;
using Plugin.Core.Utility;

namespace Plugin.Core.Models
{
    public class ItemsModel
    {
        public int Id;
        public string Name;
        public ItemCategory Category;
        public ItemEquipType Equip;
        public long ObjectId;
        public uint Count;
        public ItemsModel()
        {
        }
        public ItemsModel(int Id)
        {
            SetItemId(Id);
        }
        public ItemsModel(int Id, string Name, ItemEquipType Equip, uint Count)
        {
            SetItemId(Id);
            this.Name = Name;
            this.Equip = Equip;
            this.Count = Count;
        }
        public ItemsModel(ItemsModel Item)
        {
            Id = Item.Id;
            Name = Item.Name;
            Count = Item.Count;
            Equip = Item.Equip;
            Category = Item.Category;
            ObjectId = Item.ObjectId;
        }
        public void SetItemId(int Id)
        {
            this.Id = Id;
            Category = ComDiv.GetItemCategory(Id);
        }
    }
}