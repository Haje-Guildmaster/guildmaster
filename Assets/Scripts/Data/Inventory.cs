using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Databases;
using GuildMaster.Items;
using UnityEditorInternal;

namespace GuildMaster.Data
{
    public class Inventory
    {
        public event Action Changed;
        public IEnumerable<(Item item, int number)> GetItemList() =>_inventoryMap.Select(a => (a.Key, a.Value));
        
        
        public bool TryAddItem(Item item, int number)
        {
            _inventoryMap.TryGetValue(item, out var prevItemNum);
            var itemData = ItemDatabase.Get(item.Code);
            var updatedNumber = prevItemNum + number;
            if (updatedNumber <= itemData.MaxStack)
                _inventoryMap[item] = updatedNumber;
            else
            {
                //미정.
                _inventoryMap[item] = itemData.MaxStack;
            }
            
            if (prevItemNum != _inventoryMap[item])
                Changed?.Invoke();

            return true;
        }

        public bool TryDeleteItem(Item item, int number)
        {
            _inventoryMap.TryGetValue(item, out var prevItemNum);
            if (number < 0 || prevItemNum == number)
            {
                _inventoryMap.Remove(item);
                Changed?.Invoke();
            }
            else if(number > 0 && prevItemNum < number)
            {
                _inventoryMap[item] = prevItemNum - number;
                Changed?.Invoke();
            }
            return true;
        }

        // Todo: Clone 또는 CopyFrom 구현.

        private readonly Dictionary<Item, int> _inventoryMap = new Dictionary<Item, int>();
    }
}