using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Windows;
using GuildMaster.Windows.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemListView : MonoBehaviour
{
    [SerializeField] private Type _type;
    [SerializeField] private ItemIcon ItemIconPrefab;
    [SerializeField] private bool ClickEventOn = true;
    public enum Type
    {
        Inven, Bag,
    }
    private void Awake()
    {
        _ItemIconList = GetComponentsInChildren<ItemIcon>().ToList<ItemIcon>();
    }
    private void Start()
    {
        
    }
    private void InitializeIcons()
    {
        foreach (var icn in GetComponentsInChildren<ItemIcon>()) Destroy(icn.gameObject);
        _ItemIconList.Clear();
        for (int i = 0; i < _inventory.Size; i++)
        {
            _ItemIconList.Add(Instantiate(ItemIconPrefab, transform));
            _ItemIconList[i].UpdateAppearance(null, 0);
        }
    }
    public void Refresh() 
    {
        InitializeIcons();
        IReadOnlyList<ItemStack> itemList = _inventory.InventoryList;
        for (int i = 0; i < _inventory.Size; i++)
        {
            Item _item = itemList[i].Item;
            int _number = itemList[i].ItemNum;
            _ItemIconList[i].UpdateAppearance(_item, _number);
        }
    }
    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        Refresh();
    }
    private int _currentCategory = 0;
    private Inventory _inventory;
    private List<ItemIcon> _ItemIconList = new List<ItemIcon>();
}
