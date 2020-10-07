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
        public Item Item;
        public int ItemNum;
        public ItemStack()
        {
            Item = null;
            ItemNum = 0;
        }
        public ItemStack(Item item, int itemNum)
        {
            Item = item;
            ItemNum = itemNum;
        }
        public void setItemStack(Item item, int itemNum)
        {
            if (item == (Item)null && itemNum != 0) throw new ArgumentException("아이템 값이 null입니다");
            Item = item;
            ItemNum = itemNum;
        }
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
                for (int j = 0; j < windowsize; j++) inventoryAList[i].Add(new ItemStack());
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
            Item item = InventoryAList[category][index1].Item;
            int number = InventoryAList[category][index1].ItemNum;
            InventoryAList[category][index1].setItemStack(InventoryAList[category][index2].Item, InventoryAList[category][index2].ItemNum);
            InventoryAList[category][index2].setItemStack(item, number);
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
                    Item _item = inventoryAList[categoryIndex].FindLast(x => item.Equals(x.Item)).Item;
                    int _number = inventoryAList[categoryIndex].FindLast(x => item.Equals(x.Item)).ItemNum;
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
                    Item _item = inventoryAList[categoryIndex][index].Item;
                    int _number = inventoryAList[categoryIndex][index].ItemNum;
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
                Item _item = items.Item;
                int _number = items.ItemNum;
                totalNum += _number;
            }
            if (totalNum < number) return false;
            if (IsStacked)
            {
                while (number > 0)
                {
                    index = inventoryAList[categoryIndex].FindLastIndex(x => item.Equals(x.Item));
                    Item _item = inventoryAList[categoryIndex][index].Item;
                    int _number = inventoryAList[categoryIndex][index].ItemNum;
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
                Item _item = inventoryAList[categoryIndex][index].Item;
                int _number = inventoryAList[categoryIndex][index].ItemNum;
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
