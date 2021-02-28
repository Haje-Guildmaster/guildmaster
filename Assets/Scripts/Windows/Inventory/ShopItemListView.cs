using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemListView : MonoBehaviour
{
    [SerializeField] public ShopItemIcon ShopItemIconPrefab;
    public enum View_Category
    {
        Player_Inventory,
        Shop,
    }
    public enum Window_Category
    {
        ShopWindow,
    }
    public enum BuyOrSell
    {
        Buy,
        Sell,
    }
    [SerializeField] public View_Category view_Category;
    [SerializeField] public Window_Category window_Category;
    [SerializeField] public BuyOrSell buyOrSell;

    public event Action<Item> PointerEntered;
    public event Action PointerExited;
    public event Action<ItemStack, int> SClick;
    public IReadOnlyList<ShopItemIcon> ShopItemIconList => _ShopItemIconList.AsReadOnly();

    private void InvokePointerEntered(Item item)
    {
        PointerEntered?.Invoke(item);
    }
    private void InvokePointerExited()
    {
        PointerExited?.Invoke();
    }
    private void InvokeClick(ItemStack itemStack, int index)
    {
        SClick?.Invoke(itemStack, index);
    }
    private void Awake()
    {
        if(view_Category == View_Category.Player_Inventory)
            _ShopItemIconList = GetComponentsInChildren<ShopItemIcon>().ToList();
    }
    public void OnOffItemIcon(bool onoff, int _index, bool isbuy)
    {
        ItemStack itemstack = _inventory.TryGetItemStack(_index);
        _ShopItemIconList[_index].UpdateAppearance(itemstack, _index, isbuy);
        _ShopItemIconList[_index].ItemIconOnOff(onoff);
    }
    public void ResetOnOffItemIcon()
    {
        foreach (ShopItemIcon shopItemIcon in _ShopItemIconList)
        {
            shopItemIcon.ItemIconOnOff(false);
        }
    }
    private void InitializeIcons()
    {
        foreach (var icn in GetComponentsInChildren<ItemIcon>()) Destroy(icn.gameObject);
        _ShopItemIconList.Clear();
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++)
        {
            var itemicon = Instantiate(ShopItemIconPrefab, transform);

            if (buyOrSell == BuyOrSell.Buy)
                itemicon.UpdateAppearance(itemList[i], i, true);
            if (buyOrSell == BuyOrSell.Sell)
                itemicon.UpdateAppearance(itemList[i], i, false);

            _ShopItemIconList.Add(itemicon);

            itemicon.PointerEntered -= InvokePointerEntered;
            itemicon.PointerEntered += InvokePointerEntered;
            itemicon.PointerExited -= InvokePointerExited;
            itemicon.PointerExited += InvokePointerExited;
            itemicon.SClick -= InvokeClick;
            itemicon.SClick += InvokeClick;
        }
    }
    private void UpdateIcons()
    {
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++)
        {
            if (buyOrSell == BuyOrSell.Buy)
                _ShopItemIconList[i].UpdateAppearance(itemList[i], i, true);
            if (buyOrSell == BuyOrSell.Sell)
                _ShopItemIconList[i].UpdateAppearance(itemList[i], i, false);

            _ShopItemIconList[i].PointerEntered -= InvokePointerEntered;
            _ShopItemIconList[i].PointerEntered += InvokePointerEntered;
            _ShopItemIconList[i].PointerExited -= InvokePointerExited;
            _ShopItemIconList[i].PointerExited += InvokePointerExited;
            _ShopItemIconList[i].SClick -= InvokeClick;
            _ShopItemIconList[i].SClick += InvokeClick;
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
    public void Refresh()
    {
        UpdateIcons();
    }
    public void SetInventory(Inventory _inventory)
    {
        this._inventory = _inventory;
        InitializeIcons();
    }
    private Inventory _inventory;
    private List<ShopItemIcon> _ShopItemIconList = new List<ShopItemIcon>();
}
