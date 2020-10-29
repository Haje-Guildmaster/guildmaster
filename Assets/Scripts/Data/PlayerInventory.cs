using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;

namespace GuildMaster.Data
{
    public class PlayerInventory
    {
        /// <summary>
        /// public readonly로 바꿔야함
        /// </summary>
        public Inventory[] PlayerInventoryArray => _playerInventoryArray;

        public enum ItemCategory: int
        {
            Equipable = 0,
            Consumable = 1,
            Etc = 2,
            Important = 3,
        }

        public int getItemToCategoryNum(Item item) //아이템의 카테고리를 숫자로서 반환해 줌.
        {
            var itemData = ItemDatabase.Get(item.Code);
            if (itemData.IsEquipable) return 0;
            else if (itemData.IsConsumable) return 1;
            else if (!itemData.IsImportant && !itemData.IsConsumable && !itemData.IsEquipable) return 2;
            else if (itemData.IsImportant) return 3;
            else return 0;
        }

        public PlayerInventory(int RowSize, int Size, bool IsStacked)
        {
            this.RowSize = RowSize;
            this.Size = Size;
            this.IsStacked = IsStacked;
            _playerInventoryArray = new Inventory[RowSize];
            for (int i = 0; i< RowSize; i++) _playerInventoryArray[i] = new Inventory(Size, IsStacked);
        }
        public readonly int RowSize;
        public readonly int Size;
        public readonly bool IsStacked;
        public event Action Changed;
        //플레이어 인벤토리에 종속적이므로 static 선언을 함
        public bool TryAddItem(Item item, int number)
        {
            _playerInventoryArray[getItemToCategoryNum(item)].TryAddItem(item, number);
            Changed?.Invoke();
            return true;
        }

        public bool TryDeleteItem(Item item, int number)
        {
            _playerInventoryArray[getItemToCategoryNum(item)].TryDeleteItem(item, number);
            Changed?.Invoke();
            return true;
        }

        private Inventory[] _playerInventoryArray;
    }
}


