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
            for (int i = 0; i < ItemIconList.Length; i++)
            {
                var itemIcon = ItemIconList[i];
                var indexCapture = i;

                // (indexCapture를 추가해 주어진 핸들러를 실행하는 함수)를 반환하는 함수.
                // handler를 직접 안 받고 getter를 받는 이유는, 안 그러면 딱 그 순간 구독한 핸들러 delegate만 캡쳐해서입니다.
                // ref로 못 받는 건 람다가 ref 변수를 캡쳐하지 못하기 때문.
                Action<PointerEventData> Adapter(Func<ItemIconPointerEventHandler> handlerGetter)
                {
                    return ped =>
                    {
                        Assert.IsTrue(Inventory != null);
                        Assert.IsTrue(indexCapture < Inventory.Size);
                        Assert.IsTrue(itemIcon.ItemStackView.ItemStack == Inventory.ItemStackList[indexCapture]);
                        handlerGetter()?.Invoke(indexCapture, ped);
                    };
                }

                var pointerEvents = itemIcon.PointerEnterExitEvents;
                if (pointerEvents != null)
                {
                    pointerEvents.PointerEnter += Adapter(() => PointerEnteredItemIcon);
                    pointerEvents.PointerExit += Adapter(() => PointerExitedItemIcon);
                }

                var dragEvents = itemIcon.DragForwarder;
                if (dragEvents != null)
                {
                    dragEvents.Drag += Adapter(() => DraggedItemIcon);
                    dragEvents.BeginDrag += Adapter(() => BeganDragItemIcon);
                    dragEvents.EndDrag += Adapter(() => EndedDragItemIcon);
                }
            }
        }

        public Inventory Inventory { get; private set; }

        public void SetInventory(Inventory inventory)
        {
            var prevInventory = Inventory;
            Inventory = inventory;

            if (prevInventory != null)
                prevInventory.Changed -= Refresh;
            if (Inventory != null)
                Inventory.Changed += Refresh;

            
            foreach (var icon in ItemIconList.Skip(Inventory?.Size ?? 0))
            {
                icon.Interactable = false;
            }

            Refresh();
        }
        
        private void Refresh()
        {
            foreach (var itemIcon in ItemIconList)
                itemIcon.ItemStackView.ItemStack = new ItemStack(null, 0);
            foreach (var (itemIcon, itemStack) in Enumerable.Zip(ItemIconList, Inventory.ItemStackList,
                (a, b) => (a, b)))
            {
                itemIcon.ItemStackView.ItemStack = itemStack;
            }
        }


        private ItemIcon[] ItemIconList => _itemIconListCache ?? (_itemIconListCache = GetComponentsInChildren<ItemIcon>());

        private ItemIcon[] _itemIconListCache;

    }
}