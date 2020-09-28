﻿using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Windows.Inventory;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class ExplorationItemSelectingWindow : DraggableWindow, IToggleableWindow
    {
        // Start is called before the first frame update
        public void Open()
        {
            base.OpenWindow();
        }

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
                    if (b) ChangeCategory((ItemCategory)cat);
                });
            }

            ChangeCategory(ItemCategory.Equipable);
        }

        private void OnEnable()
        {
            Player.Instance.Inventory.Changed += Refresh;
        }

        private void OnDisable()
        {
            Player.Instance.Inventory.Changed -= Refresh;
        }


        private void OnItemIconClick(Item item)
        {
            if (item == null) return;
            if (_IsItemInCategory(item, ItemCategory.Equipable))
            {
                UiWindowsManager.Instance.ShowMessageBox("확인", "증여하시겠습니까?",
                    new (string buttonText, Action onClicked)[]
                        {("확인", () => Debug.Log("확인")), ("취소", () => Debug.Log("취소"))});
            }
            else if (_IsItemInCategory(item, ItemCategory.Consumable))
            {
                UiWindowsManager.Instance.ShowMessageBox("확인", "짐칸으로 옮기시겠습니까?",
                    new (string buttonText, Action onClicked)[]
                        {("확인", () => MoveItem(item)), ("취소", () => { })});
            }
        }

        private void Refresh()
        {
            var itemList = Player.Instance.Inventory.GetItemList()
                .Where(tup => _IsItemInCategory(tup.item, _currentCategory));

            foreach (var ii in _itemIcons)
            {
                ii.Clear();
            }
            foreach (var ((item, number), i) in itemList.Select((tup, i) => (tup, i)))
            {
                _itemIcons[i].UpdateAppearance(item, number);
            }

            foreach (var ii in _exploreItemIcons)
            {
                ii.Clear();
            }
            foreach (var ((item, number), i) in itemList.Select((tup, i) => (tup, i)))
            {
                _exploreItemIcons[i].UpdateAppearance(item, number);
            }

        }

        private void MoveItem(Item item)
        {
            Player.Instance.Inventory.TryDeleteItem(item, -1);
             _ExploreItems.Add(item, 3);
            Debug.Log("확인");
            return;
        }

        private bool _changeCategoryBlock = false; //ChangeCategory안에서 ChangeCategory가 다시 실행되는 것 방지.

        // (isOn을 수정하며 이벤트 리스너에 의해 ChangeCategory가 다시 불림)
        public void ChangeCategory(ItemCategory category)
        {
            if (_changeCategoryBlock) return;
            _changeCategoryBlock = true;
            _currentCategory = category;
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.isOn = (ItemCategory)ict.category == category;
            }
            Refresh();
            _changeCategoryBlock = false;
        }

        public enum ItemCategory
        {
            Equipable,
            Consumable,
            Etc,
            Important
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
        private Dictionary<Item, int> _ExploreItems = new Dictionary<Item, int>();
        private List<ItemIcon> _itemIcons , _exploreItemIcons;
    }
}
