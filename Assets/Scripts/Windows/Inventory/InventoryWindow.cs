using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class InventoryWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private PlayerItemListView playerItemListView;

        public void Open()
        {
            base.OpenWindow();
            Refresh();
        }

        private void Awake()
        {
            playerItemListView.SetPlayerInventory(Player.Instance.PlayerInventory);
        }

        private void Start()
        {
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

        private void OnEnable()
        {
            Player.Instance.PlayerInventory.Changed += Refresh;
        }

        private void OnDisable()
        {
            Player.Instance.PlayerInventory.Changed -= Refresh;
        }

        private void Refresh()
        {
            playerItemListView.Refresh();
        }


        private bool _changeCategoryBlock = false; //ChangeCategory안에서 ChangeCategory가 다시 실행되는 것 방지.
        // (isOn을 수정하며 이벤트 리스너에 의해 ChangeCategory가 다시 불림)
        public void ChangeCategory(PlayerInventory.ItemCategory category)
        {
            if (_changeCategoryBlock) return;
            _changeCategoryBlock = true;
            _currentCategory = category;
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.isOn = ict.category == category;
            }
            playerItemListView.ChangeCategory((int)category);
            
            _changeCategoryBlock = false;
        }

        [SerializeField] private PlayerInventory.ItemCategory _currentCategory;
    }
}