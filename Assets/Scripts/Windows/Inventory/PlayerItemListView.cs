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

    public void ChangeCategory(int _index)
    {
        _itemListView.SetInventory(_playerInventory.PlayerInventoryArray[_index]);
        Refresh();
    }

    public void SetPlayerInventory(PlayerInventory _playerInventory)
    {
        this._playerInventory = _playerInventory;
        int _index = (int)PlayerInventory.ItemCategory.Equipable;
        ChangeCategory(_index);
        Refresh();
    }
    private PlayerInventory _playerInventory;
    private ItemListView _itemListView;
}
