using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class InventoryViewItemDragManager : ItemDragManager
    {
        [SerializeField] private AutoRefreshedInventoryView _inventoryView;

        protected override void ProcessDragResult(IDragFrom dragStart, IDropIn dragEnd)
        {
            TransferItem(dragStart.Giver, dragEnd.Receiver);
        }

        protected override void Awake()
        {
            _dragFromSlotAdapter = new InventoryDragFromSlotAdapter(_inventoryView);
            _dropInSlotAdapter = new InventoryDropInSlotAdapter(_inventoryView);
            base.Awake();
        }

        protected override List<IDragFrom> GetAllDragFroms()
        {
            return _dragFromSlotAdapter.DragFromSlots.ToList();
        }

        protected override List<IDropIn> GetAllDropIns()
        {
            return _dropInSlotAdapter.DropInSlots.ToList();
        }


        protected override bool IsDragAble(IDragFrom dragStart, IDropIn dragEnd)
        {
            return true;
        }

        private InventoryDragFromSlotAdapter _dragFromSlotAdapter;
        private InventoryDropInSlotAdapter _dropInSlotAdapter;
    }
}