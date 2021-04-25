using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class InventoryViewItemDragManagerComponent : MonoBehaviour
    {
        [SerializeField] private AutoRefreshedInventoryView _inventoryView;
        [SerializeField] private InventoryViewItemDragManager _dragManager;
        
        [Serializable]
        private class InventoryViewItemDragManager: ItemDragManager
        {
            public void Initialize(InventoryViewItemDragManagerComponent parent)
            {
                _parent = parent;
            }
            protected override void ProcessDragResult(IDragFrom dragStart, IDropIn dragEnd)
            {
                TransferItem(dragStart.Giver, dragEnd.Receiver);
            }
            protected override List<IDragFrom> GetAllDragFroms()
            {
                return _parent._dragFromSlotAdapter.DragFromSlots.ToList();
            }
            protected override List<IDropIn> GetAllDropIns()
            {
                return _parent._dropInSlotAdapter.DropInSlots.ToList();
            }
            protected override bool IsDragAble(IDragFrom dragStart, IDropIn dragEnd)
            {
                return true;
            }

            private InventoryViewItemDragManagerComponent _parent;
        }
        
        private void Awake()
        {
            _dragManager.Initialize(this);
            _dragFromSlotAdapter = new InventoryDragFromSlotAdapter(_inventoryView);
            _dropInSlotAdapter = new InventoryDropInSlotAdapter(_inventoryView);
        }

        private void OnEnable()
        {
            _dragManager.Enable();
        }

        private void OnDisable()
        {
            _dragManager.Disable();
        }

        private InventoryDragFromSlotAdapter _dragFromSlotAdapter;
        private InventoryDropInSlotAdapter _dropInSlotAdapter;
    }
}