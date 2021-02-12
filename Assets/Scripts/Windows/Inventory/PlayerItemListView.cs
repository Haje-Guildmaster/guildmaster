using GuildMaster.Data;
using System;

namespace GuildMaster.Windows
{
    public class PlayerItemListView : ItemListView
    {

        public void ChangeCategory(PlayerInventory.ItemCategory category)
        {
            SetInventory(_playerInventory.GetInventory(category));
        }

        public void SetPlayerInventory(PlayerInventory _playerInventory)
        {
            this._playerInventory = _playerInventory;
            ChangeCategory(PlayerInventory.ItemCategory.Equipable);
        }
        private PlayerInventory _playerInventory;
    }

}
