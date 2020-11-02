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
    [SerializeField] public DraggingItemIcon draggingItemIcon;
    public enum View_Category{
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

            itemicon.PointerEntered -= (item) => PointerEntered?.Invoke(item);
            itemicon.PointerExited -= () => PointerExited?.Invoke();
            itemicon.BeginDrag -= (eventData, index) => BeginDrag?.Invoke(eventData, index);
            itemicon.EndDrag -= () => EndDrag?.Invoke();
            itemicon.Drag -= (eventData) => Drag?.Invoke(eventData);
            itemicon.Drop -= (eventData, index) => Drop?.Invoke(eventData, index);

            itemicon.PointerEntered += (item) => PointerEntered?.Invoke(item);
            itemicon.PointerExited += () => PointerExited?.Invoke();
            itemicon.BeginDrag += (eventData, index) => BeginDrag?.Invoke(eventData, index);
            itemicon.EndDrag += () => EndDrag?.Invoke();
            itemicon.Drag += (eventData) => Drag?.Invoke(eventData);
            itemicon.Drop += (eventData, index) => Drop?.Invoke(eventData, index);
        }
    }
    private void UpdateIcons()
    {
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++)
        {

            _ItemIconList[i].UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i);

            _ItemIconList[i].PointerEntered -= (item) => PointerEntered?.Invoke(item);
            _ItemIconList[i].PointerExited -= () => PointerExited?.Invoke();
            _ItemIconList[i].BeginDrag -= (eventData, index) => BeginDrag?.Invoke(eventData, index);
            _ItemIconList[i].EndDrag -= () => EndDrag?.Invoke();
            _ItemIconList[i].Drag -= (eventData) => Drag?.Invoke(eventData);
            _ItemIconList[i].Drop -= (eventData, index) => Drop?.Invoke(eventData, index);

            _ItemIconList[i].PointerEntered += (item) => PointerEntered?.Invoke(item);
            _ItemIconList[i].PointerExited += () => PointerExited?.Invoke();
            _ItemIconList[i].BeginDrag += (eventData, index) => BeginDrag?.Invoke(eventData, index);
            _ItemIconList[i].EndDrag += () => EndDrag?.Invoke();
            _ItemIconList[i].Drag += (eventData) => Drag?.Invoke(eventData);
            _ItemIconList[i].Drop += (eventData, index) => Drop?.Invoke(eventData, index);
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
        if (onoff) {
            _ItemIconList[_index].UpdateAppearance(_inventory.TryGetItemStack(_index).Item, _inventory.TryGetItemStack(_index).ItemNum, _index);
            _ItemIconList[_index].ItemIconOnOff(onoff);
        }
        else
        {
            //원래는 null, 0으로 안보이게 하는 함수였지만 일단 보류.
            _ItemIconList[_index].UpdateAppearance(_inventory.TryGetItemStack(_index).Item, _inventory.TryGetItemStack(_index).ItemNum, _index);
            _ItemIconList[_index].ItemIconOnOff(onoff);
        } 
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
