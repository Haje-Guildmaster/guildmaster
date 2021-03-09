using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class PlayerInventoryWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private PlayerInventoryView _playerInventoryView;
        
        public void Open()
        {
            base.OpenWindow();
        }
        
        private void Start()
        {
            _playerInventoryView.SetPlayerInventory(Player.Instance.PlayerInventory);
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                var cat = ict.category;
                ict.Toggle.onValueChanged.AddListener(b =>
                {
                    if (b) ChangeCategory(cat);
                });
            }
            ChangeCategory(PlayerInventory.ItemCategory.Equipable);
        }

        public PlayerInventory.ItemCategory CurrentCategory => _playerInventoryView.CurrentCategory;

        public void ChangeCategory(PlayerInventory.ItemCategory category)
        {
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.SetIsOnWithoutNotify(ict.category == category);
            }
            _playerInventoryView.ChangeCategory(category);
        }

        private int _panelRequestId;

        private int _draggingItemIndex;
        private ItemStack _draggingItemStack;
    }
}