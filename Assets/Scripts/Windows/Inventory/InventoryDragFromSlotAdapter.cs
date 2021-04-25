using System;
using System.Collections.Generic;
using System.Reflection;
using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class InventoryDragFromSlotAdapter
    {
        public IReadOnlyList<ItemDragManager.IDragFrom> DragFromSlots => _dragFromSlots;

        public InventoryDragFromSlotAdapter(AutoRefreshedInventoryView inventoryView)
        {
            _inventoryView = inventoryView;
            var size = _inventoryView.ItemIconCount;
            _dragFromSlots = new DragFromSlot[size];
            for (int i = 0; i < size; i++)
            {
                _dragFromSlots[i] = new DragFromSlot(_inventoryView, i);
            }

            inventoryView.DraggedItemIcon +=
                (index, pointerEvent) => _dragFromSlots[index].InvokeDragged(pointerEvent);
            inventoryView.BeganDragItemIcon +=
                (index, pointerEvent) => _dragFromSlots[index].InvokeBeganDrag(pointerEvent);
            inventoryView.EndedDragItemIcon +=
                (index, pointerEvent) => _dragFromSlots[index].InvokeEndedDrag(pointerEvent);
            // Todo: Dispose될 때 event 구독 취소하기
        }


        private class DragFromSlot : ItemDragManager.IDragFrom, IItemGiver
        {
            public event Action<PointerEventData> Dragged;
            public event Action<PointerEventData> BeganDrag;
            public event Action<PointerEventData> EndedDrag;
            public IItemGiver Giver => this;
            

            public DragFromSlot(AutoRefreshedInventoryView inventoryView, int itemIndex)
            {
                _inventoryView = inventoryView;
                _itemIndex = itemIndex;
            }

            public void IndicateBeingDragged(bool doIndicate)
            {
                _inventoryView.SetSlotBackgroundColor(_itemIndex, doIndicate ? Color.red: Color.white);
            }

            public void InvokeDragged(PointerEventData ped) => Dragged?.Invoke(ped);

            public void InvokeBeganDrag(PointerEventData ped) => BeganDrag?.Invoke(ped);

            public void InvokeEndedDrag(PointerEventData ped) => EndedDrag?.Invoke(ped);
            
            public ItemStack GetAvailable() => _inventoryView.Inventory.GetItemStack(_itemIndex);

            public void TakeItem(int number, Item item) => _inventoryView.Inventory.RemoveItemFromIndex(_itemIndex, number, item);

            private readonly int _itemIndex;
            private readonly AutoRefreshedInventoryView _inventoryView;
            
        }

        private readonly DragFromSlot[] _dragFromSlots;
        private readonly AutoRefreshedInventoryView _inventoryView;
    }
}