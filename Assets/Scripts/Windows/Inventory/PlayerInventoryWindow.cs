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
        }

        public PlayerInventory.ItemCategory CurrentCategory => _playerInventoryView.CurrentCategory;
    }
}