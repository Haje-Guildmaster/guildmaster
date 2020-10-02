using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuildMaster.Data
{
    public class ItemStack
    {
        public Item Item => item;
        public int ItemNum => itemnum;
        public ItemStack(Item _item, int _itemnum)
        {
            item = _item;
            itemnum = _itemnum;
        }
        public void setItemStack(Item _item, int _itemnum)
        {
            item = _item;
            itemnum = _itemnum;
        }
        public (Item, int) getItemStack()
        {
            return (item, itemnum);
        }
        private Item item;
        private int itemnum;
    }
    public class Inventory
    {
        public Inventory(int _WindowSize, int _TotalSize, bool _IsStacked)
        {
            IsStacked = _IsStacked;
            windowsize = _WindowSize;
            totalsize = _TotalSize;
            row = totalsize / windowsize;
            inventoryAList = new List<ItemStack>[row];
            for (int i = 0; i < row; i++)
            {
                inventoryAList[i] = new List<ItemStack>();
                inventoryAList[i].Clear();
                for (int j = 0; j < windowsize; j++) inventoryAList[i].Add(new ItemStack(null, 0));
            }
        }

        public readonly bool IsStacked;

        public event Action Changed;
        public List<ItemStack>[] InventoryAList => inventoryAList;
        public int WindowSize => windowsize;
        public int TotalSize => totalsize;

        private int getCategory(Item item)
        {
            var itemData = ItemDatabase.Get(item.Code);
            if (itemData.IsEquipable) return 0;
            else if (itemData.IsConsumable) return 1;
            else if (!itemData.IsImportant && !itemData.IsConsumable && !itemData.IsEquipable) return 2;
            else if (itemData.IsImportant) return 3;
            else return -1;
        }
        public void ChangeItemIndex(int category, Item item, int index1, int index2)
        {
            ItemStack itemStack;
            itemStack = InventoryAList[category][index1];
            InventoryAList[category][index1] = InventoryAList[category][index2];
            InventoryAList[category][index2] = itemStack;
            return;
        }
        public bool TryAddItem(Item item, int number)
        {
            if (item == (Item)null || number == 0) return false;
            var itemData = ItemDatabase.Get(item.Code);
            var categoryIndex = getCategory(item);
            if (IsStacked)
            {
                for (int i = 0; i < windowsize; i++)
                {
                    var (_item, _number) = inventoryAList[categoryIndex][i].getItemStack();
                    if (item.Equals(_item))
                    {
                        var availableSpace = itemData.MaxStack - _number;
                        if (number <= availableSpace)
                        {
                            _number += number;
                            number = 0;
                        }
                        else
                        {
                            _number += availableSpace;
                            number -= availableSpace;
                        }
                    }
                    inventoryAList[categoryIndex][i].setItemStack(_item, _number);
                    if (number == 0) break;
                }
                int emptyIndex;
                while (number > 0)
                {
                    var newStackSize = Math.Min(number, itemData.MaxStack);
                    emptyIndex = InventoryAList[categoryIndex].FindIndex(x => x.Item == (Item)null);
                    number -= newStackSize;
                    inventoryAList[categoryIndex][emptyIndex].setItemStack(item, newStackSize);
                }
            }
            else if (!IsStacked)
            {
                if (inventoryAList[categoryIndex].Exists(x => item.Equals(x.Item)))
                {
                    var index = inventoryAList[categoryIndex].FindIndex(x => item.Equals(x.Item));
                    var (_item, _number) = inventoryAList[categoryIndex][index].getItemStack();
                    inventoryAList[categoryIndex][index].setItemStack(_item, _number + number);
                    number = 0;
                }
                else
                {
                    if (!inventoryAList[categoryIndex].Exists(x => x.Item == (Item)null))
                    {
                        return false;
                    }
                    else
                    {
                        inventoryAList[categoryIndex].Find(x => x.Item == null).setItemStack(item, number);
                        number = 0;
                    }
                }
            }
            Changed?.Invoke();
            return true;
        }

        public bool TryDeleteItem(Item item, int number)
        {
            if (item == (Item)null || number == 0) return false;
            var categoryIndex = getCategory(item);
            if (!inventoryAList[categoryIndex].Exists(x => x.Equals(item))) return false;
            var itemData = ItemDatabase.Get(item.Code);
            int totalNum = 0;
            int index;
            foreach (var items in inventoryAList[categoryIndex].FindAll(x => x.Item.Equals(item)))
            {
                var (_item, _number) = items.getItemStack();
                totalNum += _number;
            }
            if (totalNum < number) return false;
            if (IsStacked)
            {
                while (number > 0)
                {
                    index = inventoryAList[categoryIndex].FindLastIndex(x => x.Item.Equals(item));
                    var (_item, _number) = inventoryAList[categoryIndex][index].getItemStack();
                    if (number >= itemData.MaxStack)
                    {
                        _number = 0;
                        number -= itemData.MaxStack;
                        _item = null;
                    }
                    else
                    {
                        _number -= number;
                        number = 0;
                    }
                    if (number == 0) break;
                }
            }
            if (!IsStacked)
            {
                index = inventoryAList[categoryIndex].FindIndex(x => x.Item.Equals(item));
                var (_item, _number) = inventoryAList[categoryIndex][index].getItemStack();
                inventoryAList[categoryIndex][index].setItemStack(item, _number - number);
            }
            Changed?.Invoke();
            return true;
        }
        private readonly int windowsize, totalsize, row;
        private readonly List<ItemStack>[] inventoryAList;
    }
}