using GuildMaster.Data;
using GuildMaster.Windows;
using System;

public class PlayerShopItemListView : ShopItemListView
{
    public void ChangeCategory(int _index)
    {
        Console.WriteLine(_index);
        SetInventory(_playerInventory.PlayerInventoryList[_index]);
    }

    public void SetPlayerInventory(PlayerInventory _playerInventory)
    {
        this._playerInventory = _playerInventory;
        ChangeCategory((int)PlayerInventory.ItemCategory.Equipable);
    }
    private PlayerInventory _playerInventory;
}
