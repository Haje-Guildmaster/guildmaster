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
    public event Action<Item, int> Click;

    private void InvokePointerEntered(Item item)
    {
        PointerEntered?.Invoke(item);
    }
    private void InvokePointerExited()
    {
        PointerExited?.Invoke();
    }
    private void InvokeClick(Item item, int number)
    {
        Click?.Invoke(item, number);
    }
    private void Awake()
    {
        if(view_Category == View_Category.Player_Inventory)
            _ShopItemIconList = GetComponentsInChildren<ShopItemIcon>().ToList();
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
                itemicon.UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i, itemList[i].BuyCost, 0, itemList[i].isInfinite);
            if (buyOrSell == BuyOrSell.Sell)
                itemicon.UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i, itemList[i].SellCost, 0, itemList[i].isInfinite);

            _ShopItemIconList.Add(itemicon);

            itemicon.PointerEntered -= InvokePointerEntered;
            itemicon.PointerExited -= InvokePointerExited;
            itemicon.Click -= InvokeClick;

            itemicon.PointerEntered += InvokePointerEntered;
            itemicon.PointerExited += InvokePointerExited;
            itemicon.Click += InvokeClick;
        }
    }
    private void UpdateIcons()
    {
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++)
        {
            if (buyOrSell == BuyOrSell.Buy)
                _ShopItemIconList[i].UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i, itemList[i].BuyCost, 0, itemList[i].isInfinite);
            if (buyOrSell == BuyOrSell.Sell)
                _ShopItemIconList[i].UpdateAppearance(itemList[i].Item, itemList[i].ItemNum, i, itemList[i].SellCost, 0, itemList[i].isInfinite);

            _ShopItemIconList[i].PointerEntered -= InvokePointerEntered;
            _ShopItemIconList[i].PointerExited -= InvokePointerExited;
            _ShopItemIconList[i].Click -= InvokeClick;

            _ShopItemIconList[i].PointerEntered += InvokePointerEntered;
            _ShopItemIconList[i].PointerExited += InvokePointerExited;
            _ShopItemIconList[i].Click += InvokeClick;
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
        ItemStack itemstack = _inventory.TryGetItemStack(_index);
        if (onoff)
        {
            _ShopItemIconList[_index].UpdateAppearance(itemstack.Item, itemstack.ItemNum, _index);
            _ShopItemIconList[_index].ItemIconOnOff(onoff);
        }
        else
        {
            //원래는 null, 0으로 안보이게 하는 함수였지만 일단 보류.
            _ShopItemIconList[_index].UpdateAppearance(itemstack.Item, itemstack.ItemNum, _index);
            _ShopItemIconList[_index].ItemIconOnOff(onoff);
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
    private Inventory _inventory;
    private List<ShopItemIcon> _ShopItemIconList = new List<ShopItemIcon>();
}
