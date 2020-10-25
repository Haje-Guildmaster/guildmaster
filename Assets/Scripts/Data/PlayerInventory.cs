using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;

namespace GuildMaster.Data
{
    public class PlayerInventory
    {
        public PlayerInventory(int _RowSize, int _Size, bool _IsStacked)
        {
            RowSize = _RowSize;
            Size = _Size;
            IsStacked = _IsStacked;
            _playerInventoryList = new Inventory[_RowSize];
            for (int i = 0; i< RowSize; i++)
            {
                for (int j = 0; j < _RowSize; j++) _playerInventoryList[i] = new Inventory(_Size, _IsStacked);
            }
        }
        public readonly int RowSize;
        public readonly int Size;
        public readonly bool IsStacked;
        public event Action Changed;
        //플레이어 인벤토리에 종속적이므로 static 선언을 함
        public static int getItemToCategoryNum(Item item) //아이템의 카테고리를 숫자로서 반환해 줌.
        {
            var itemData = ItemDatabase.Get(item.Code);
            if (itemData.IsEquipable) return 0;
            else if (itemData.IsConsumable) return 1;
            else if (!itemData.IsImportant && !itemData.IsConsumable && !itemData.IsEquipable) return 2;
            else if (itemData.IsImportant) return 3;
            else return 0;
        }
        public bool TryAddItem(Item item, int number)
        {
            _playerInventoryList[getItemToCategoryNum(item)].TryAddItem(item, number);
            return true;
        }

        public bool TryDeleteItem(Item item, int number)
        {
            _playerInventoryList[getItemToCategoryNum(item)].TryDeleteItem(item, number);
            return true;
        }

        private readonly Inventory[] _playerInventoryList;
    }
}


