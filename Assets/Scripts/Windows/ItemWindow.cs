using GuildMaster.Data;
using GuildMaster.Windows;
using GuildMaster.Windows.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemWindow : MonoBehaviour
{
    [SerializeField] private Type _type;
    [SerializeField] private ItemIcon ItemIconPrefab;
    [SerializeField] private bool ClickEventOn = true;

    public static ItemIcon DraggingItem;

    public enum Type
    {
        Inven, Bag,
    }
    public enum ItemCategory
    {
        Equipable,
        Consumable,
        Etc,
        Important
    }
    public static int CategoryToNum(ItemCategory category)
    {
        if (category == ItemCategory.Equipable) return 0;
        else if (category == ItemCategory.Consumable) return 1;
        else if (category == ItemCategory.Etc) return 2;
        else return 3;
    }
    private void Awake()
    {
        _ItemIconList = GetComponentsInChildren<ItemIcon>().ToList<ItemIcon>();
    }
    private void Start()
    {
        
    }
    public void RefreshCategory(ItemWindow.ItemCategory itemCategory)
    {
        //if (_type != Type.Inven) return;
        _currentCategory = ItemWindow.CategoryToNum(itemCategory);
        Refresh();
        return;
    }
    private void ItemClick(int index)
    {
        if(_type == Type.Inven)
        {
            var (_item, _itemnum) = _inventory.InventoryAList[_currentCategory][index].getItemStack();
            Player.Instance.Inventory.TryDeleteItem(_item, _itemnum);
            UiWindowsManager.Instance.ExplorationItemSelectingWindow.ExploreInventory.TryAddItem(_item, _itemnum);
        }
        if (_type == Type.Bag)
        {
            var (_item, _itemnum) = UiWindowsManager.Instance.ExplorationItemSelectingWindow.ExploreInventory.InventoryAList[_currentCategory][index].getItemStack();
            Player.Instance.Inventory.TryAddItem(_item, _itemnum);
            UiWindowsManager.Instance.ExplorationItemSelectingWindow.ExploreInventory.TryDeleteItem(_item, _itemnum);
        }
        UiWindowsManager.Instance.ExplorationItemSelectingWindow.bagWindow.Refresh();
        return;
    }
    private void OnIconClick(int index)
    {
        String Where;
        if (_type == Type.Inven) Where = "가방으로";
        else Where = "인벤토리로";
        UiWindowsManager.Instance.ShowMessageBox("확인", Where + " 옮기시겠습니까?",
                new (string buttonText, Action onClicked)[]
                    {("확인", () => ItemClick(index)), ("취소", () => { }) });
        return;
    }
    private void InitiateIcons()
    {
        foreach (var icn in GetComponentsInChildren<ItemIcon>()) Destroy(icn.gameObject);
        _ItemIconList.Clear();
        for (int i = 0; i < _inventory.WindowSize; i++)
        {
            _ItemIconList.Add(Instantiate(ItemIconPrefab, transform));
            _ItemIconList[i].UpdateAppearance(null, 0, i, _inventory, this);
        }
    }
    private void RefreshIconEvents()
    {
        if (!ClickEventOn) return;
        foreach (var icon in _ItemIconList)
        {
            icon.Clicked -= OnIconClick;
            if (icon.item == null) continue;
            icon.Clicked += OnIconClick;
        }
    }
    public void Refresh() 
    {
        InitiateIcons();
        List<ItemStack>[] itemList = _inventory.InventoryAList;
        for (int i = 0; i < _inventory.WindowSize; i++)
        {
            var (_item, _itemnum) = itemList[_currentCategory][i].getItemStack();
            _ItemIconList[i].UpdateAppearance(_item, _itemnum, i, _inventory, this);
        }
        RefreshIconEvents();
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
