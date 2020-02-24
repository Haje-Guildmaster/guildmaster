using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class InventoryWindow: DraggableWindow
    {
        private void Awake()
        {
            _itemIcons = GetComponentsInChildren<ItemIcon>().ToList();
        }
        private void Start()
        {
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                var cat = ict.category;
                ict.Toggle.onValueChanged.AddListener(b =>
                {
                    if (b) ChangeCategory(cat);
                });
            }
            ChangeCategory(ItemCategory.Equipable);
        }
        
        private void OnEnable()
        {
            PlayerData.Instance.InventoryChanged += Refresh;
        }

        private void OnDisable()
        {
            PlayerData.Instance.InventoryChanged -= Refresh;
        }

        protected override void OnOpen()
        {
            Refresh();
        }

        private void Refresh()
        {
            var itemList = PlayerData.Instance.GetInventory()
                .Where(tup => _IsItemInCategory(tup.item, _currentCategory));

            foreach (var ii in _itemIcons)
            {
                ii.Clear();
            }
            foreach (var ((item, number), i) in itemList.Select((tup,i)=>(tup,i)))
            {
                _itemIcons[i].UpdateAppearance(item, number);
            }
        }

        public void ChangeCategory(ItemCategory category)
        {
            _currentCategory = category;
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.isOn = ict.category == category;
            }
            Refresh();
        }

        public enum ItemCategory
        {
            Equipable, Consumable, Etc, Important
        }
        
        
        private static bool _IsItemInCategory(Item item, ItemCategory category)
        {
            var itemData = ItemDatabase.Instance.GetItemStaticData(item.Code);
            switch (category)
            {
                case ItemCategory.Equipable:
                    return item.EquipAble;
                case ItemCategory.Consumable:
                    return itemData.IsConsumable;
                case ItemCategory.Important:
                    return itemData.IsImportant;
                case ItemCategory.Etc:
                    return !item.EquipAble && !itemData.IsConsumable && !itemData.IsImportant;
                default:
                    return false;
            }
        }

        private ItemCategory _currentCategory; 
        private List<ItemIcon> _itemIcons;
    }
}