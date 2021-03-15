using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace GuildMaster.Data
{
    [Serializable]
    public class ItemCount
    {
        public Item Item;
        public int Number;
        public ItemCount()
        {
            Item = null;
            Number = 0;
        }
        public ItemCount(Item item, int number)
        {
            Item = item;
            Number = number;
        }
    }
    [Serializable]
    public class ItemStack
    {
        public Item Item;
        public int ItemNum;
        public int BuyCost;
        public int SellCost;
        public int Quantity;
        public bool isInfinite;
        /// <summary>
        /// 빈칸을 표시하기 위한 생성자.
        /// </summary>
        public ItemStack()
        {
            Item = null;
            ItemNum = 0;
            Quantity = 0;
        }
        /// <summary>
        /// 개수 제한이 있는 아이템을 생성하기 위한 생성자.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNum"></param>
        public ItemStack(Item item, int itemNum)
        {
            Item = item;
            ItemNum = itemNum;
            BuyCost = item.StaticData.BuyPrice;
            SellCost = item.StaticData.SellPrice;
            Quantity = 0;
            if (itemNum > 0)
                isInfinite = false;
            else
                throw new ArgumentException("아이템의 개수가 0 또는 음수입니다");
        }
        /// <summary>
        /// 개수 제한이 없는 아이템을 생성하기 위한 생성자.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="infinite"></param>
        public ItemStack(Item item, bool infinite)
        {
            if (infinite == false) throw new ArgumentException("ItemStack(int, bool) 생성자는 개수가 제한이 없는 아이템 스택을 생성하기 위해 존재하는 생성자입니다.");
            Item = item;
            ItemNum = 1; //TryAddItem 리팩토링 최소화를 위해 1로 설정
            BuyCost = item.StaticData.BuyPrice;
            SellCost = item.StaticData.SellPrice;
            Quantity = 0;
            isInfinite = infinite;
        }
        public void setInfiniteItemStack(Item item)
        {
            if (item == null)
                throw new ArgumentException("아이템 값이 null입니다.");
            Item = item;
            ItemNum = 1;
            BuyCost = item.StaticData.BuyPrice;
            SellCost = item.StaticData.SellPrice;
            Quantity = 0;
            isInfinite = true;
        }
        public void setItemStack(Item item, int itemNum)
        {
            if (item == (Item)null && itemNum != 0)
                throw new ArgumentException("아이템 값이 null인데 아이템 개수가 0이 아닙니다.");
            Item = item;
            ItemNum = itemNum;
            if (item != null)
            {
                BuyCost = item.StaticData.BuyPrice;
                SellCost = item.StaticData.SellPrice;
            }
            isInfinite = false;
            if (ItemNum < 0)
                throw new ArgumentException("아이템의 개수가 음수입니다");
        }
        public void setItemQuantity(int quantity)
        {
            if (Item == (Item)null && quantity != 0)
                throw new ArgumentException("아이템 값이 null인 ItemStack의 quantity를 바꾸려고 시도했습니다.");
            if (quantity > ItemNum || quantity < 0)
                return;
            Quantity = quantity;
        }
    }
    public class Inventory
    {
        public Inventory(int Size, bool IsStacked)
        {
            this.IsStacked = IsStacked;
            this.Size = Size;
            _inventoryList = new List<ItemStack>();
            _inventoryList.Clear();
            for (int i = 0; i < Size; i++) _inventoryList.Add(new ItemStack());
        }
        public readonly bool IsStacked;
        public readonly int Size;
        public IReadOnlyList<ItemStack> InventoryList => _inventoryList;
        public int Num
        {
            get
            {
                int num = 0;
                foreach (ItemStack itemStack in _inventoryList)
                {
                    if (itemStack.Item != null) num += 1;
                }
                return num;
            }
        }
        public event Action Changed;

        public void ChangeItemIndex(int index1, int index2)
        {
            Item item = _inventoryList[index1].Item;
            int number = _inventoryList[index1].ItemNum;
            _inventoryList[index1].setItemStack(_inventoryList[index2].Item, _inventoryList[index2].ItemNum);
            _inventoryList[index2].setItemStack(item, number);
            return;
        }
        public ItemStack TryGetItemStack(int _index)
        {
            if (_index >= 0 && _index < Size) return _inventoryList[_index];
            else return (ItemStack)null;
        }
        /// <summary>
        /// 상점에서 플레이어가 판 물건을 npc 인벤토리에 넣기 위한 메서드. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool TryAddInfiniteItem(Item item)
        {
            if (item == null) return false;
            int index = _inventoryList.FindIndex(x => x.Item == null);
            _inventoryList[index] = new ItemStack(item, true);
            return true;
        }
        public bool TryAddItem(Item item, int number)
        {
            if (item == (Item)null || number == 0) return false;
            var itemData = ItemDatabase.Get(item.Code);
            int index, emptyIndex;
            if (IsStacked)
            {
                if (_inventoryList.Exists(x => item.Equals(x.Item) && !x.isInfinite))
                {
                    index = _inventoryList.FindLastIndex(x => item.Equals(x.Item) && !x.isInfinite);
                    ItemStack itemStack = _inventoryList.FindLast(x => item.Equals(x.Item) && !x.isInfinite);
                    Item _item = itemStack.Item;
                    int _number = itemStack.ItemNum;
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
                    _inventoryList[index].setItemStack(_item, _number);
                }
                while (number > 0)
                {
                    int newStackSize = Math.Min(number, itemData.MaxStack);
                    emptyIndex = _inventoryList.FindIndex(x => x.Item == (Item)null);
                    if (emptyIndex < 0 || emptyIndex >= Size) 
                        throw new Exception("인벤토리의 사이즈가 충분하지 않습니다.");
                    number -= newStackSize;
                    _inventoryList[emptyIndex].setItemStack(item, newStackSize);
                }
            }
            else if (!IsStacked)
            {
                if (_inventoryList.Exists(x => item.Equals(x.Item) && !x.isInfinite))
                {
                    index = _inventoryList.FindIndex(x => item.Equals(x.Item) && !x.isInfinite);
                    Item _item = _inventoryList[index].Item;
                    int _number = _inventoryList[index].ItemNum;
                    _inventoryList[index].setItemStack(_item, _number + number);
                    number = 0;
                }
                else
                {
                    if (!_inventoryList.Exists(x => x.Item == (Item)null))
                    {
                        return false;
                    }
                    else
                    {
                        _inventoryList.Find(x => x.Item == null).setItemStack(item, number);
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
            if (!_inventoryList.Exists(x => item.Equals(x.Item))) return false;
            if (_inventoryList.Exists(x => x.isInfinite)) return true;
            var itemData = ItemDatabase.Get(item.Code);
            int totalNum = 0;
            int index;
            foreach (var items in _inventoryList.FindAll(x => item.Equals(x.Item)))
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
                    index = _inventoryList.FindLastIndex(x => item.Equals(x.Item));
                    Item _item = _inventoryList[index].Item;
                    int _number = _inventoryList[index].ItemNum;
                    if (number >= _number)
                    {
                        number -= _number;
                        _number = 0;
                        _inventoryList[index].setItemStack(null, 0);
                    }
                    else
                    {
                        _number -= number;
                        number = 0;
                        _inventoryList[index].setItemStack(_item, _number);
                    }
                }
            }
            if (!IsStacked)
            {
                index = _inventoryList.FindIndex(x => item.Equals(x.Item));
                Item _item = _inventoryList[index].Item;
                int _number = _inventoryList[index].ItemNum;
                _inventoryList[index].setItemStack(item, _number - number);
                if (_number == number) _inventoryList[index].setItemStack(null, 0);
            }
            Changed?.Invoke();
            return true;
        }
        private readonly List<ItemStack> _inventoryList;
    }
}
