using System;
using System.Collections.Generic;
using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class InventoryDropInSlotAdapter
    {
        public IReadOnlyList<ItemDragManager.IDropIn> DropInSlots => _dropInSlots;

        public InventoryDropInSlotAdapter(AutoRefreshedInventoryView inventoryView)
        {
            _inventoryView = inventoryView;
            var size = _inventoryView.ItemIconCount;
            _dropInSlots = new DropInSlot[size];
            for (int i = 0; i < size; i++)
            {
                _dropInSlots[i] = new DropInSlot(_inventoryView, i);
            }

            inventoryView.PointerEnteredItemIcon +=
                (index, pointerEvent) => _dropInSlots[index].InvokePointerEntered(pointerEvent);
            inventoryView.BeganDragItemIcon +=
                (index, pointerEvent) => _dropInSlots[index].InvokePointerExited(pointerEvent);
            // Todo: Dispose될 때 event 구독 취소하기
        }


        private class DropInSlot : ItemDragManager.IDropIn, IItemReceiver
        {
            public event Action<PointerEventData> PointerEntered;
            public event Action<PointerEventData> PointerExited;

            public IItemReceiver Receiver => this;
            
            public DropInSlot(AutoRefreshedInventoryView inventoryView, int itemIndex)
            {
                _inventoryView = inventoryView;
                _itemIndex = itemIndex;
            }

            public void Highlight(bool doHighlight)
            {
                // throw new NotImplementedException();
            }

            public int GiveItem(Item item, int number)
            {
                return _inventoryView.Inventory.TryAddItemToIndex(_itemIndex, item, number);
            }

            public void InvokePointerEntered(PointerEventData ped) => PointerEntered?.Invoke(ped);
            public void InvokePointerExited(PointerEventData ped) => PointerExited?.Invoke(ped);

            private readonly AutoRefreshedInventoryView _inventoryView;
            private readonly int _itemIndex;
        }

        private readonly DropInSlot[] _dropInSlots;
        private readonly AutoRefreshedInventoryView _inventoryView;
    }
}