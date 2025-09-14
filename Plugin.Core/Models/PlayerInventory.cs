using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class PlayerInventory
    {
        public List<ItemsModel> Items = new List<ItemsModel>();
        public PlayerInventory()
        {
        }
        public ItemsModel GetItem(int Id)
        {
            lock (Items)
            {
                foreach (ItemsModel Item in Items)
                {
                    if (Item.Id == Id)
                    {
                        return Item;
                    }
                }
            }
            return null;
        }
        public ItemsModel GetItem(long ObjectId)
        {
            lock (Items)
            {
                foreach (ItemsModel Item in Items)
                {
                    if (Item.ObjectId == ObjectId)
                    {
                        return Item;
                    }
                }
            }
            return null;
        }
        public List<ItemsModel> GetItemsByType(ItemCategory Type)
        {
            List<ItemsModel> List = new List<ItemsModel>();
            lock (Items)
            {
                foreach (ItemsModel Item in Items)
                {
                    if (Item.Category == Type || Item.Id > 1600000 && Item.Id < 1700000 && Type == ItemCategory.NewItem)
                    {
                        List.Add(Item);
                    }
                }
            }
            return List;
        }
        public bool RemoveItem(ItemsModel Item)
        {
            lock (Items)
            {
                return Items.Remove(Item);
            }
        }
        public void AddItem(ItemsModel Item)
        {
            lock (Items)
            {
                Items.Add(Item);
            }
        }
        public void LoadBasicItems()
        {
            lock (Items)
            {
                foreach (ItemsModel Item in TemplatePackXML.Basics)
                {
                    if (ComDiv.GetIdStatics(Item.Id, 1) != 6)
                    {
                        Items.Add(Item);
                    }
                }
            }
        }
        public void LoadCafeItems()
        {
            lock (Items)
            {
                Items.AddRange(TemplatePackXML.CafePCs);
            }
        }
        public void LoadGeneralBeret()
        {
            lock (Items)
            {
                Items.Add(new ItemsModel(2700008, "Beret S. General", ItemEquipType.Permanent, 1));
            }
        }
        public void LoadHatForGM()
        {
            lock (Items)
            {
                Items.Add(new ItemsModel(700160, "MOD Hat", ItemEquipType.Permanent, 1));
            }
        }
    }
}
