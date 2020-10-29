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
    [SerializeField] private ItemIcon ItemIconPrefab;
    [SerializeField] private bool ClickEventOn = true;

    public event Action<PointerEventData, Item> PointerEntered;
    public event Action<PointerEventData> PointerExited;
    public event Action<PointerEventData, int> BeginDrag;
    public event Action<PointerEventData> EndDrag;
    public event Action<PointerEventData, Item, int> Drag;
    public event Action<PointerEventData, int> Drop;
    private void Awake()
    {
        _ItemIconList = GetComponentsInChildren<ItemIcon>().ToList<ItemIcon>();
    }
    private void InitializeIcons()
    {
        foreach (var icn in GetComponentsInChildren<ItemIcon>()) Destroy(icn.gameObject);
        _ItemIconList.Clear();
        for (int i = 0; i < _inventory.Size; i++)
        {
            var itemicon = Instantiate(ItemIconPrefab, transform);
            
            itemicon.UpdateAppearance(null, 0, i);
            itemicon.PointerEntered -= (eventData, item) => this.PointerEntered?.Invoke(eventData, item);
            itemicon.PointerExited -= (eventData) => this.PointerExited?.Invoke(eventData);
            itemicon.BeginDrag -= (eventData, index) => this.BeginDrag?.Invoke(eventData, index);
            itemicon.EndDrag -= (eventData) => this.EndDrag?.Invoke(eventData);
            itemicon.Drag -= (eventData, item, number) => this.Drag?.Invoke(eventData, item, number);
            itemicon.Drop -= (eventData, index) => this.Drop?.Invoke(eventData, index);

            itemicon.PointerEntered += (eventData, item) => this.PointerEntered?.Invoke(eventData, item);
            itemicon.PointerExited += (eventData) => this.PointerExited?.Invoke(eventData);
            itemicon.BeginDrag += (eventData, index) => this.BeginDrag?.Invoke(eventData, index);
            itemicon.EndDrag += (eventData) => this.EndDrag?.Invoke(eventData);
            itemicon.Drag += (eventData, item, number) => this.Drag?.Invoke(eventData, item, number);
            itemicon.Drop += (eventData, index) => this.Drop?.Invoke(eventData, index);

            _ItemIconList.Add(itemicon);
        }
    }
    public void Refresh() 
    {
        InitializeIcons();
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++) _ItemIconList[i].UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i);
    }
    public void SetInventory(Inventory _inventory)
    {
        this._inventory = _inventory;
        Refresh();
    }
    private int _currentCategory = 0;
    private Inventory _inventory;
    private List<ItemIcon> _ItemIconList = new List<ItemIcon>();
}
