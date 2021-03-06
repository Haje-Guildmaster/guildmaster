using GuildMaster.Data;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class AutoRefreshedInventoryView : MonoBehaviour
    {
        [SerializeField] public ItemIcon ItemIconPrefab;

        public delegate void ItemIconPointerEventHandler(int index, PointerEventData pointerEventData);

        public event ItemIconPointerEventHandler PointerEnteredItemIcon;
        public event ItemIconPointerEventHandler PointerExitedItemIcon;
        public event ItemIconPointerEventHandler BeganDragItemIcon;
        public event ItemIconPointerEventHandler EndedDragItemIcon;
        public event ItemIconPointerEventHandler DraggedItemIcon;

        public delegate void ItemIconClickHandler(int index);

        public event ItemIconClickHandler ClickedItemIcon;

        private void Awake()
        {
            _itemIconList = GetComponentsInChildren<ItemIcon>();
            for (int i = 0; i < _itemIconList.Length; i++)
            {
                var itemIcon = _itemIconList[i];
                var indexCapture = i;

                // (indexCapture를 추가해 주어진 핸들러를 실행하는 함수)를 반환하는 함수.
                Action<PointerEventData> Adapter(ItemIconPointerEventHandler handler)
                {
                    return (PointerEventData ped) =>
                    {
                        Assert.IsTrue(indexCapture < _inventory.Size);
                        Assert.IsTrue(itemIcon.ItemStackView.ItemStack == _inventory.ItemStackList[indexCapture]);
                        handler?.Invoke(indexCapture, ped);
                    };
                }

                var pointerEvents = itemIcon.PointerEnterExitEvents;
                if (pointerEvents != null)
                {
                    pointerEvents.PointerEnter += Adapter(PointerEnteredItemIcon);
                    pointerEvents.PointerExit += Adapter(PointerExitedItemIcon);
                }

                var dragEvents = itemIcon.DragForwarder;
                if (dragEvents != null)
                {
                    dragEvents.Drag += Adapter(DraggedItemIcon);
                    dragEvents.BeginDrag += Adapter(BeganDragItemIcon);
                    dragEvents.EndDrag += Adapter(EndedDragItemIcon);
                }
            }
        }

        public Inventory Inventory
        {
            get => _inventory;
            set => SetInventory(value);
        }
        
        private void Refresh()
        {
            foreach (var itemIcon in _itemIconList)
                itemIcon.ItemStackView.ItemStack = new ItemStack(null, 0);
            foreach (var (itemIcon, itemStack) in Enumerable.Zip(_itemIconList, _inventory.ItemStackList,
                (a, b) => (a, b)))
            {
                itemIcon.ItemStackView.ItemStack = itemStack;
            }
        }

        private void SetInventory(Inventory inventory)
        {
            var prevInventory = _inventory;
            _inventory = inventory;

            if (prevInventory != null)
                prevInventory.Changed -= Refresh;
            if (_inventory != null)
                _inventory.Changed += Refresh;

            
            foreach (var icon in _itemIconList.Skip(_inventory?.Size ?? 0))
            {
                icon.Interactable = false;
            }

            Refresh();
        }

        private Inventory _inventory;
        private ItemIcon[] _itemIconList;
    }
}