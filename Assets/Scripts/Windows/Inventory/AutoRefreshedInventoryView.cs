using GuildMaster.Data;
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
            for (int i = 0; i < _ItemIconList.Length; i++)
            {
                var itemIcon = _ItemIconList[i];
                var indexCapture = i;

                void CallWithIndex(PointerEventData ped, ItemIconPointerEventHandler handler)
                {
                    Assert.IsTrue(Inventory != null);
                    Assert.IsTrue(indexCapture < Inventory.Size);
                    Assert.IsTrue(itemIcon.ItemStackView.ItemStack == Inventory.ItemStackList[indexCapture]); // consistency check
                    handler?.Invoke(indexCapture, ped);
                }

                var pointerEvents = itemIcon.PointerEnterExitEvents;
                if (pointerEvents != null)
                {
                    pointerEvents.PointerEnter += ped => CallWithIndex(ped, PointerEnteredItemIcon);
                    pointerEvents.PointerExit += ped => CallWithIndex(ped, PointerExitedItemIcon);
                }

                var dragEvents = itemIcon.DragForwarder;
                if (dragEvents != null)
                {
                    dragEvents.Drag += ped => CallWithIndex(ped, DraggedItemIcon);
                    dragEvents.BeginDrag += ped => CallWithIndex(ped, BeganDragItemIcon);
                    dragEvents.EndDrag += ped => CallWithIndex(ped, EndedDragItemIcon);
                }

                var btn = itemIcon.Button;
                if (btn != null)
                {
                    btn.onClick.AddListener(() => ClickedItemIcon?.Invoke(indexCapture));
                }
            }
        }

        public Inventory Inventory { get; private set; }
        public int ItemIconCount => _ItemIconList.Length;

        public void SetInventory(Inventory inventory)
        {
            var prevInventory = Inventory;
            Inventory = inventory;

            if (prevInventory != null)
                prevInventory.Changed -= Refresh;
            if (Inventory != null)
                Inventory.Changed += Refresh;


            foreach (var icon in _ItemIconList.Skip(Inventory?.Size ?? 0))
            {
                icon.Interactable = false;
            }

            Refresh();
        }

        public void SetSlotBackgroundColor(int index, Color color)
        {
            _ItemIconList[index].BackGroundImage.color = color;
            // Todo: priority queue&취소기능.
        }

        private void Refresh()
        {
            foreach (var itemIcon in _ItemIconList)
                itemIcon.ItemStackView.ItemStack = new ItemStack(null, 0);
            foreach (var (itemIcon, itemStack) in Enumerable.Zip(_ItemIconList, Inventory.ItemStackList,
                (a, b) => (a, b)))
            {
                itemIcon.ItemStackView.ItemStack = itemStack;
            }
        }


        private ItemIcon[] _ItemIconList =>
            _itemIconListCache ?? (_itemIconListCache = GetComponentsInChildren<ItemIcon>());

        private ItemIcon[] _itemIconListCache;
    }
}