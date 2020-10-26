using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Windows;
using GuildMaster.Windows.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerItemListView : MonoBehaviour
{

    public void Refresh()
    {
        _itemListView.Refresh();
    }

    public void ChangeCategory(PlayerInventory.ItemCategory itemCategory)
    {
        _itemListView.SetInventory(_playerInventory.PlayerInventoryArray[(int)itemCategory]);
        Refresh();
    }

    public void SetPlayerInventory(PlayerInventory _playerInventory)
    {
        this._playerInventory = _playerInventory;
        ChangeCategory(PlayerInventory.ItemCategory.Equipable);
        Refresh();
    }
    private PlayerInventory _playerInventory;
    private ItemListView _itemListView;
}
