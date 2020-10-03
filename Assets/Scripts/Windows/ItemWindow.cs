using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Windows;
using GuildMaster.Windows.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Transactions;
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
        ItemIconList = GetComponentsInChildren<ItemIcon>().ToList<ItemIcon>();
    }
    private void Start()
    {
        
    }
    public void RefreshCategory(ItemWindow.ItemCategory itemCategory)
    {
        //if (_type != Type.Inven) return;
        currentCategory = ItemWindow.CategoryToNum(itemCategory);
        Refresh();
        return;
    }
    private void ItemClick(int index)
    {
        if(_type == Type.Inven)
        {
            var (_item, _itemnum) = inventory.InventoryAList[currentCategory][index].getItemStack();
            if(Player.Instance.Inventory.TryDeleteItem(_item, _itemnum)) Debug.Log("인벤토리 아이템 삭제 성공");
            if(UiWindowsManager.Instance.ExplorationItemSelectingWindow.ExploreInventory.TryAddItem(_item, _itemnum)) Debug.Log("가방 아이템 더하기 성공");
        }
        if (_type == Type.Bag)
        {
            var (_item, _itemnum) = UiWindowsManager.Instance.ExplorationItemSelectingWindow.ExploreInventory.InventoryAList[currentCategory][index].getItemStack();
            if (Player.Instance.Inventory.TryAddItem(_item, _itemnum)) Debug.Log("인벤토리 아이템 더하기 성공");
            if (UiWindowsManager.Instance.ExplorationItemSelectingWindow.ExploreInventory.TryDeleteItem(_item, _itemnum)) Debug.Log("가방 아이템 삭제 성공");
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
        ItemIconList.Clear();
        for (int i = 0; i < inventory.WindowSize; i++)
        {
            ItemIconList.Add(Instantiate(ItemIconPrefab, transform));
            ItemIconList[i].UpdateAppearance(null, 0, i, inventory);
        }
    }
    private void RefreshIconEvents()
    {
        if (!ClickEventOn) return;
        foreach (var icon in ItemIconList)
        {
            icon.Clicked -= OnIconClick;
            if (icon.item == null) continue;
            icon.Clicked += OnIconClick;
        }
    }
    public void Refresh() 
    {
        InitiateIcons();
        List<ItemStack>[] itemList = inventory.InventoryAList;
        for (int i = 0; i < inventory.WindowSize; i++)
        {
            var (_item, _itemnum) = itemList[currentCategory][i].getItemStack();
            ItemIconList[i].UpdateAppearance(_item, _itemnum, i, inventory);
        }
        RefreshIconEvents();
    }
    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
        Refresh();
    }
    private int currentCategory = 0;
    private Inventory inventory;
    private List<ItemIcon> ItemIconList = new List<ItemIcon>();
}
