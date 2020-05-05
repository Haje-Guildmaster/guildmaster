using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Database;
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

        
        private readonly Dictionary<Item, int> _inventoryMap = new Dictionary<Item, int>();
    }
}