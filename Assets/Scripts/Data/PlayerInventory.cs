using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;

namespace GuildMaster.Data
{
    public class PlayerInventory
    {
        public enum ItemCategory
        {
            Equipable,
            Consumable,
            Etc,
            Important,

            COUNT, // 개수.
        }

        /// <summary>
        /// 아이템의 카테고리를 분류해 반환함.
        /// </summary>
        /// <param name="item"> 아이템 </param>
        /// <returns> 지정한 아이템의 카테고리 </returns>
        public static ItemCategory GetItemCategory(Item item)
        {
            var itemData = ItemDatabase.Get(item.Code);
            if (itemData.IsEquipable) return ItemCategory.Equipable;
            if (itemData.IsConsumable) return ItemCategory.Consumable;
            if (itemData.IsImportant) return ItemCategory.Important;
            return ItemCategory.Etc;
        }

        public PlayerInventory(int size, bool isStacked)
        {
            _inventoryArray = new Inventory[(int) ItemCategory.COUNT];
            for (int i = 0; i < (int) ItemCategory.COUNT; i++)
            {
                var newInventory = new Inventory(size, isStacked);
                _inventoryArray[i] = newInventory;
                newInventory.Changed += () => Changed?.Invoke();
            }
        }

        public Inventory GetInventory(ItemCategory category) => _inventoryArray[(int) category];


        public event Action Changed;

        /// <summary>
        /// 지정한 아이템을 지정한 숫자만큼 인벤토리에 넣습니다. <br/>
        /// 넣는 데에 성공한 개수를 반환합니다.
        /// </summary>
        /// <param name="item"> 아이템 </param>
        /// <param name="number"> 넣는 개수 </param>
        /// <returns></returns>
        public int TryAddItem(Item item, int number)
        {
            return GetInventory(GetItemCategory(item)).TryAddItem(item, number);
        }

        /// <summary>
        /// 지정한 아이템을 지정한 숫자만큼 제거합니다. <br/>
        /// 실제로 제거된 아이템 개수를 반환합니다.
        /// </summary>
        /// <param name="item"> 제거할 아이템 </param>
        /// <param name="number"> 제거하는 개수 </param>
        /// <returns> 실제로 제거된 아이템 개수 </returns>
        public int TryDeleteItem(Item item, int number)
        {
            return GetInventory(GetItemCategory(item)).TryRemoveItem(item, number);
        }

        private readonly Inventory[] _inventoryArray;
    }
}