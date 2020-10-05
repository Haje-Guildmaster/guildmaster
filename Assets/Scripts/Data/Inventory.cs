using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

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

        public static int getItemToCategoryNum(Item item)
        {
            var itemData = ItemDatabase.Get(item.Code);
            if (itemData.IsEquipable) return 0;
            else if (itemData.IsConsumable) return 1;
            else if (!itemData.IsImportant && !itemData.IsConsumable && !itemData.IsEquipable) return 2;
            else if (itemData.IsImportant) return 3;
            else return 0;
        }
        public void ChangeItemIndex(int category, int index1, int index2)
        {
            var(_item, _itemnum) = InventoryAList[category][index1].getItemStack();
            var(_item2, _itemnum2) = InventoryAList[category][index2].getItemStack();
            InventoryAList[category][index1].setItemStack(_item2, _itemnum2);
            InventoryAList[category][index2].setItemStack(_item, _itemnum);
            return;
        }
        public bool TryAddItem(Item item, int number)
        {
            if (item == (Item)null || number == 0) return false;
            var itemData = ItemDatabase.Get(item.Code);
            int index, emptyIndex, categoryIndex = getItemToCategoryNum(item);
            if (IsStacked)
            {
                categoryIndex = 0;
                if(inventoryAList[categoryIndex].Exists(x => item.Equals(x.Item)))
                {
                    index = inventoryAList[categoryIndex].FindLastIndex(x => item.Equals(x.Item));
                    var (_item, _number) = inventoryAList[categoryIndex].FindLast(x => item.Equals(x.Item)).getItemStack();
                    int availableSpace = itemData.MaxStack - _number;
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
                    inventoryAList[categoryIndex][index].setItemStack(_item, _number);
                }
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
                    index = inventoryAList[categoryIndex].FindIndex(x => item.Equals(x.Item));
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
            var categoryIndex = getItemToCategoryNum(item);
            if (IsStacked) categoryIndex = 0;
            if (!inventoryAList[categoryIndex].Exists(x => item.Equals(x.Item))) return false;
            var itemData = ItemDatabase.Get(item.Code);
            int totalNum = 0;
            int index;
            foreach (var items in inventoryAList[categoryIndex].FindAll(x => item.Equals(x.Item)))
            {
                var (_item, _number) = items.getItemStack();
                totalNum += _number;
            }
            if (totalNum < number) return false;
            if (IsStacked)
            {
                while (number > 0)
                {
                    index = inventoryAList[categoryIndex].FindLastIndex(x => item.Equals(x.Item));
                    var (_item, _number) = inventoryAList[categoryIndex][index].getItemStack();
                    if (number >= _number)
                    {
                        number -= _number;
                        _number = 0;
                        inventoryAList[categoryIndex][index].setItemStack(null, 0);
                    }
                    else
                    {
                        _number -= number;
                        number = 0;
                        inventoryAList[categoryIndex][index].setItemStack(_item, _number);
                    }
                }
            }
            if (!IsStacked)
            {
                index = inventoryAList[categoryIndex].FindIndex(x => item.Equals(x.Item));
                var (_item, _number) = inventoryAList[categoryIndex][index].getItemStack();
                inventoryAList[categoryIndex][index].setItemStack(item, _number - number);
                if (_number == number) inventoryAList[categoryIndex][index].setItemStack(null, 0);
            }
            Changed?.Invoke();
            return true;
        }
        private readonly int windowsize, totalsize, row;
        private readonly List<ItemStack>[] inventoryAList;
    }
}
