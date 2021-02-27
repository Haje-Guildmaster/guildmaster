using GuildMaster.Data;
using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class ItemListView : MonoBehaviour
    {
        [SerializeField] public ItemIcon ItemIconPrefab;
        [SerializeField] public DraggingItemIcon draggingItemIcon;
        public enum View_Category
        {
            Inventory,
            Bag,
        }
        public enum Window_Category
        {
            InventoryWindow,
            ExplorationItemSelectingWindow,
        }
        [SerializeField] public View_Category view_Category;
        [SerializeField] public Window_Category window_Category;

        public event Action<Item> PointerEntered;
        public event Action PointerExited;
        public event Action<PointerEventData, int> BeginDrag;
        public event Action EndDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData, int> Drop;
        public event Action<Item, int> Click;

        private void InvokePointerEntered(Item item)
        {
            PointerEntered?.Invoke(item);
        }
        private void InvokePointerExited()
        {
            PointerExited?.Invoke();
        }
        private void InvokeBeginDrag(PointerEventData eventData, int index)
        {
            BeginDrag?.Invoke(eventData, index);
        }
        private void InvokeEndDrag()
        {
            EndDrag?.Invoke();
        }
        private void InvokeDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData);
        }
        private void InvokeDrop(PointerEventData eventData, int index)
        {
            Drop?.Invoke(eventData, index);
        }
        private void InvokeClick(Item item, int number)
        {
            Click?.Invoke(item, number);
        }

        private void Awake()
        {
            _ItemIconList = GetComponentsInChildren<ItemIcon>().ToList();
        }
        private void InitializeIcons()
        {
            foreach (var icn in GetComponentsInChildren<ItemIcon>()) Destroy(icn.gameObject);
            _ItemIconList.Clear();
            IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
            for (int i = 0; i < _inventory.Size; i++)
            {
                var itemicon = Instantiate(ItemIconPrefab, transform);

                itemicon.UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i);

                _ItemIconList.Add(itemicon);

                itemicon.PointerEntered -= InvokePointerEntered;
                itemicon.PointerEntered += InvokePointerEntered;
                itemicon.PointerExited -= InvokePointerExited;
                itemicon.PointerExited += InvokePointerExited;
                itemicon.BeginDrag -= InvokeBeginDrag;
                itemicon.BeginDrag += InvokeBeginDrag;
                itemicon.EndDrag -= InvokeEndDrag;
                itemicon.EndDrag += InvokeEndDrag;
                itemicon.Drag -= InvokeDrag;
                itemicon.Drag += InvokeDrag;
                itemicon.Drop -= InvokeDrop;
                itemicon.Drop += InvokeDrop;
                itemicon.Click -= InvokeClick;
                itemicon.Click += InvokeClick;
            }
        }
        private void UpdateIcons()
        {
            IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
            for (int i = 0; i < _inventory.Size; i++)
            {
                _ItemIconList[i].UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i);

                _ItemIconList[i].PointerEntered -= InvokePointerEntered;
                _ItemIconList[i].PointerExited -= InvokePointerExited;
                _ItemIconList[i].BeginDrag -= InvokeBeginDrag;
                _ItemIconList[i].EndDrag -= InvokeEndDrag;
                _ItemIconList[i].Drag -= InvokeDrag;
                _ItemIconList[i].Drop -= InvokeDrop;
                _ItemIconList[i].Click -= InvokeClick;

                _ItemIconList[i].PointerEntered += InvokePointerEntered;
                _ItemIconList[i].PointerExited += InvokePointerExited;
                _ItemIconList[i].BeginDrag += InvokeBeginDrag;
                _ItemIconList[i].EndDrag += InvokeEndDrag;
                _ItemIconList[i].Drag += InvokeDrag;
                _ItemIconList[i].Drop += InvokeDrop;
                _ItemIconList[i].Click += InvokeClick;
            }
        }
        public void ChangeItemStackIndex(int _index1, int _index2)
        {
            _inventory.ChangeItemIndex(_index1, _index2);
        }
        public ItemStack getItemStack(int _index)
        {
            return _inventory.TryGetItemStack(_index);
        }
        public void OnOffItemIcon(bool onoff, int _index)
        {
            _ItemIconList[_index].UpdateAppearance(_inventory.TryGetItemStack(_index).Item, _inventory.TryGetItemStack(_index).ItemNum, _index);
            _ItemIconList[_index].ItemIconOnOff(onoff);
        }
        public void Refresh()
        {
            UpdateIcons();
        }
        public void SetInventory(Inventory _inventory)
        {
            this._inventory = _inventory;
            InitializeIcons();
        }
        private int _currentCategory = 0;
        private Inventory _inventory;
        private List<ItemIcon> _ItemIconList;
    }
}

