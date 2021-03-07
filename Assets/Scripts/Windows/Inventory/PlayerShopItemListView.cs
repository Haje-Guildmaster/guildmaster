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
    public void ResetQuantity()
    {
        foreach (var pinven in _playerInventory.PlayerInventoryList)
        {
            foreach (ItemStack itemStack in pinven.InventoryList)
            {
                if (itemStack.Item == null) continue;
                itemStack.Quantity = 0;
            }
        }
    }
    public void SetPlayerInventory(PlayerInventory _playerInventory)
    {
        this._playerInventory = _playerInventory;
        ChangeCategory((int)PlayerInventory.ItemCategory.Equipable);
    }
    private PlayerInventory _playerInventory;
}
