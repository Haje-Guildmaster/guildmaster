using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows.Inventory
{
    public class ExplorationItemSelectingWindow : DraggableWindow, IToggleableWindow
    {
        // Start is called before the first frame update
        [SerializeField] private ItemWindow inventoryWindow;
        [SerializeField] private ItemWindow bagWindow;
        public void Open()
        {
            base.OpenWindow();
        }
        public void Back()
        {
            base.Close();
            UiWindowsManager.Instance.ExplorationCharacterSelectingWindow.Open();
        }
        private void Awake()
        {
            inventoryWindow.SetInventory(Player.Instance.Inventory);
            bagWindow.SetInventory(_exploreInventory);
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                var cat = ict.category;
                ict.Toggle.onValueChanged.AddListener(b =>
                {
                    if (b) ChangeCategory(cat);
                });
            }
            ChangeCategory(ItemWindow.ItemCategory.Equipable);
            Refresh();
        }

        private void Start()
        {
            
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
            inventoryWindow.Refresh();
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
            inventoryWindow.RefreshCategory(category);
            Refresh();
            _changeCategoryBlock = false;
        }

        private ItemWindow.ItemCategory _currentCategory;
        private Data.Inventory _exploreInventory = new Data.Inventory(12, 12, true);
    }
}

