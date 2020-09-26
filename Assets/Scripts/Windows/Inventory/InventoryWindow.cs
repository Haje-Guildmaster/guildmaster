using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows.Inventory
{
    public class InventoryWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private ItemWindow itemWindow;

        public void Open()
        {
            base.OpenWindow();
        }

        private void Awake()
        {
            itemWindow.SetInventory(Player.Instance.Inventory);
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
            ChangeCategory(ItemWindow.ItemCategory.Equipable);
        }

        private void OnEnable()
        {
            Player.Instance.Inventory.Changed += Refresh;
        }

        private void OnDisable()
        {
            Player.Instance.Inventory.Changed -= Refresh;
        }

        private void Refresh()
        {
            itemWindow.Refresh();
        }


        private bool _changeCategoryBlock = false; //ChangeCategory안에서 ChangeCategory가 다시 실행되는 것 방지.
        // (isOn을 수정하며 이벤트 리스너에 의해 ChangeCategory가 다시 불림)
        public void ChangeCategory(ItemWindow.ItemCategory category)
        {
            if (_changeCategoryBlock) return;
            _changeCategoryBlock = true;
            _currentCategory = category;
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.isOn = ict.category == category;
            }
            itemWindow.RefreshCategory(category);
            Refresh();
            _changeCategoryBlock = false;
        }

        private ItemWindow.ItemCategory _currentCategory;
    }
}