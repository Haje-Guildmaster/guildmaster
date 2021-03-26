using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [SerializeField] private ItemCategorySelector _categorySelector;
        public PlayerInventory.ItemCategory CurrentCategory { get; private set; }
        [field: SerializeField] public AutoRefreshedInventoryView InventoryView { get; private set; }

        private void Awake()
        {
            if (_categorySelector != null)
                _categorySelector.CategoryChanged += SetCategory;
        }
        
        public void SetCategory(PlayerInventory.ItemCategory category)
        {
            if (_categorySelector != null)
                _categorySelector.SetCategoryWithoutNotify(category);
            InventoryView.SetInventory(_playerInventory.GetInventory(category));
            CurrentCategory = category;
        }

        public void SetPlayerInventory(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
            SetCategory(PlayerInventory.ItemCategory.Equipable);
        }

        private PlayerInventory _playerInventory;
    }
}