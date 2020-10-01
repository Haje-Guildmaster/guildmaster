using GuildMaster.Data;
using GuildMaster.Items;
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
    public static List<ItemIcon> ItemIconList = new List<ItemIcon>();

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
    private int CategoryNum(ItemCategory category)
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
    public void RefreshCategory(ItemCategory _currentCategory)
    {
        //if (_type != Type.Inven) return;
        currentCategory = CategoryNum(_currentCategory);
        Refresh();
        return;
    }
    private void ItemClick(int index)
    {
        ItemIcon icon = ItemIconList[index];
        ItemIconList[index + 1].UpdateAppearance(icon.item, icon.number, index + 1);
        ItemIconList[index].UpdateAppearance(null, 0, index);
    }
    private void OnIconClick(int index)
    {
        Item item = ItemIconList[index].item;
        if (item == null)
        {

        }
        else if(item != null && _type == Type.Inven)
        {
            UiWindowsManager.Instance.ShowMessageBox("확인", "가방으로 옮기시겠습니까?",
                new (string buttonText, Action onClicked)[]
                    {("확인", () => ItemClick(index)), ("취소", () => { }) });
        }
    }
    private void InitiateIcons()
    {
        foreach (var icn in GetComponentsInChildren<ItemIcon>()) Destroy(icn.gameObject);
        ItemIconList.Clear();
        for (int i = 0; i < inventory.WindowSize; i++)
        {
            ItemIconList.Add(Instantiate(ItemIconPrefab, transform));
            ItemIconList[i].UpdateAppearance(null, 0, i);
        }
    }
    private void RefreshIconEvents(Type type)
    {
        foreach (var icon in ItemIconList)
        {
            icon.Clicked -= OnIconClick;
            icon.Clicked += OnIconClick;
        }
    }
    public void Refresh() 
    {
        InitiateIcons();
        List<ItemStack>[] itemList = inventory.InventoryAList;
        if (_type.Equals(Type.Inven))
        {
            for (int i = 0; i < inventory.WindowSize; i++)
            {
                var (_item, _itemnum) = itemList[currentCategory][i].getItemStack();
                ItemIconList[i].UpdateAppearance(_item, _itemnum, i);
            }
        }
        else if (_type.Equals(Type.Bag))
        {
            for (int i = 0; i < inventory.WindowSize; i++)
            {
                var (_item, _itemnum) = itemList[0][i].getItemStack();
                ItemIconList[i].UpdateAppearance(_item, _itemnum, i);
            }
        }
        RefreshIconEvents(_type);
    }
    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
        Refresh();
    }
    private int currentCategory = 0;
    private Inventory inventory;
}
