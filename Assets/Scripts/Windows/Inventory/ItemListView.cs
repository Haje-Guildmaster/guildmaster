using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Windows;
using GuildMaster.Windows.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemListView : MonoBehaviour
{
    [SerializeField] public ItemIcon ItemIconPrefab;
    [SerializeField] private bool ClickEventOn = true;

    public event Action<PointerEventData, Item> PointerEntered;
    public event Action<PointerEventData> PointerExited;
    public event Action<PointerEventData, int> BeginDrag;
    public event Action<PointerEventData> EndDrag;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData, int> Drop;
    private void Awake()
    {
        _ItemIconList = GetComponentsInChildren<ItemIcon>().ToList<ItemIcon>();
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

            itemicon.PointerEntered -= (eventData, item) => this.PointerEntered?.Invoke(eventData, item);
            itemicon.PointerExited -= (eventData) => this.PointerExited?.Invoke(eventData);
            itemicon.BeginDrag -= (eventData, index) => this.BeginDrag?.Invoke(eventData, index);
            itemicon.EndDrag -= (eventData) => this.EndDrag?.Invoke(eventData);
            itemicon.Drag -= (eventData) => this.Drag?.Invoke(eventData);
            itemicon.Drop -= (eventData, index) => this.Drop?.Invoke(eventData, index);

            itemicon.PointerEntered += (eventData, item) => this.PointerEntered?.Invoke(eventData, item);
            itemicon.PointerExited += (eventData) => this.PointerExited?.Invoke(eventData);
            itemicon.BeginDrag += (eventData, index) => this.BeginDrag?.Invoke(eventData, index);
            itemicon.EndDrag += (eventData) => this.EndDrag?.Invoke(eventData);
            itemicon.Drag += (eventData) => this.Drag?.Invoke(eventData);
            itemicon.Drop += (eventData, index) => this.Drop?.Invoke(eventData, index);
        }
    }
    private void UpdateIcons()
    {
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++)
        {

            _ItemIconList[i].UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i);

            _ItemIconList[i].PointerEntered -= (eventData, item) => this.PointerEntered?.Invoke(eventData, item);
            _ItemIconList[i].PointerExited -= (eventData) => this.PointerExited?.Invoke(eventData);
            _ItemIconList[i].BeginDrag -= (eventData, index) => this.BeginDrag?.Invoke(eventData, index);
            _ItemIconList[i].EndDrag -= (eventData) => this.EndDrag?.Invoke(eventData);
            _ItemIconList[i].Drag -= (eventData) => this.Drag?.Invoke(eventData);
            _ItemIconList[i].Drop -= (eventData, index) => this.Drop?.Invoke(eventData, index);

            _ItemIconList[i].PointerEntered += (eventData, item) => this.PointerEntered?.Invoke(eventData, item);
            _ItemIconList[i].PointerExited += (eventData) => this.PointerExited?.Invoke(eventData);
            _ItemIconList[i].BeginDrag += (eventData, index) => this.BeginDrag?.Invoke(eventData, index);
            _ItemIconList[i].EndDrag += (eventData) => this.EndDrag?.Invoke(eventData);
            _ItemIconList[i].Drag += (eventData) => this.Drag?.Invoke(eventData);
            _ItemIconList[i].Drop += (eventData, index) => this.Drop?.Invoke(eventData, index);
        }
    }
    public void ChangeItemStackIndex(int _index1, int _index2)
    {
        _inventory.ChangeItemIndex(_index1, _index2);
    }
    public ItemStack getItemStack(int _index)
    {
        return _inventory.TryGetItem(_index);
    }
    public void OnOffItemIcon(Item _item, int _number, int _index)
    {
        _ItemIconList[_index].UpdateAppearance(_item, _number, _index);
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
    private List<ItemIcon> _ItemIconList = new List<ItemIcon>();
}
