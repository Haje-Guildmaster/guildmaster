using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Database;
using GuildMaster.Items;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class InventoryWindow: DraggableWindow, IToggleableWindow
    {
        private void Awake()
        {
            UpdateChildrenItemIcons();
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
            PlayerData.Instance.Inventory.Changed += Refresh;
        }

        private void OnDisable()
        {
            PlayerData.Instance.Inventory.Changed -= Refresh;
        }


        private void OnItemIconClick(Item item)
        {
            if (item == null) return;
            if (_IsItemInCategory(item, ItemCategory.Equipable))
            {
                UiWindowsManager.Instance.ShowMessageBox("확인", "증여하시겠습니까?", 
                    new (string buttonText, Action onClicked)[]
                        {("확인", ()=>Debug.Log("확인")), ("취소", () => Debug.Log("취소"))});
            }
            else if (_IsItemInCategory(item, ItemCategory.Consumable))
            {
                UiWindowsManager.Instance.ShowMessageBox("확인", "짐칸으로 옮기시겠습니까?", 
                    new (string buttonText, Action onClicked)[]
                        {("확인", ()=>Debug.Log("확인")), ("취소", () => Debug.Log("취소"))});
            }
        }
        
        public void Open()
        {
            base.OpenWindow();
            Refresh();
        }

        private void Refresh()
        {
            var itemList = PlayerData.Instance.Inventory.GetItemList()
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


        private bool _changeCategoryBlock = false;    //ChangeCategory안에서 ChangeCategory가 다시 실행되는 것 방지.
                                                      // (isOn을 수정하며 이벤트 리스너에 의해 ChangeCategory가 다시 불림)
        public void ChangeCategory(ItemCategory category)
        {
            if (_changeCategoryBlock) return;
            _changeCategoryBlock = true;
            _currentCategory = category;
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.isOn = ict.category == category;
            }
            Refresh();
            _changeCategoryBlock = false;
        }

        public enum ItemCategory
        {
            Equipable, Consumable, Etc, Important
        }


        private void UpdateChildrenItemIcons()
        {
            _itemIcons = GetComponentsInChildren<ItemIcon>().ToList();
            foreach (var icon in _itemIcons)
            {
                icon.Clicked += OnItemIconClick;
            }
        }
        private static bool _IsItemInCategory(Item item, ItemCategory category)
        {
            if (item == null) return false;
            var itemData = ItemDatabase.Get(item.Code);
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