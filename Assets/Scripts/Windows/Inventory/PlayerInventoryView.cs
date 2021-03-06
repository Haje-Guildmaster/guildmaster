﻿using GuildMaster.Data;
using System;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class PlayerInventoryView: MonoBehaviour
    {
        [field: SerializeField] public AutoRefreshedInventoryView InventoryView { get; private set; }
        public PlayerInventory.ItemCategory CurrentCategory { get; private set; }
        
        
        public void ChangeCategory(PlayerInventory.ItemCategory category)
        {
            InventoryView.SetInventory(_playerInventory.GetInventory(category));
            CurrentCategory = category;
        }

        public void SetPlayerInventory(PlayerInventory playerInventory)
        {
            this._playerInventory = playerInventory;
            ChangeCategory(PlayerInventory.ItemCategory.Equipable);
        }
        
        private PlayerInventory _playerInventory;
    }

}
