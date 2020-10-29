using GuildMaster.Data;
using GuildMaster.Windows.Inventory;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class InventoryWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private PlayerItemListView playerItemListView;

        public void Open()
        {
            base.OpenWindow();
            playerItemListView.Refresh();
        }

        private void Awake()
        {
            playerItemListView.SetPlayerInventory(Player.Instance.PlayerInventory);
            playerItemListView.PointerEntered += (eventData, item) =>
            {
                if (item != null)
                    _panelRequestId = UiWindowsManager.Instance.itemInfoPanel.Open(item.Code);
            };
            playerItemListView.PointerExited += (eventData) => 
            {
                if (_panelRequestId == 0) return;
                UiWindowsManager.Instance.itemInfoPanel.Close(_panelRequestId);
                _panelRequestId = 0;
            };
            playerItemListView.BeginDrag += (eventData, index) => {
                _draggingItemIndex = index;
                _draggingItemStack = playerItemListView.getItemStack(index);

                _ItemIcon = Instantiate(playerItemListView.ItemIconPrefab, transform);
                _ItemIcon.UpdateAppearance(_draggingItemStack.Item, _draggingItemStack.ItemNum, index);
                _draggingItemIcon = Instantiate(_ItemIcon.transform, GameObject.FindGameObjectWithTag("Canvas").transform);
                
                Destroy(_ItemIcon.gameObject);
            };
            playerItemListView.Drag += (eventData) => 
            {
                _draggingItemIcon.position = eventData.position;
            };
            playerItemListView.EndDrag += (eventData) => {
                Destroy(_draggingItemIcon.gameObject);
            };
            playerItemListView.Drop += (eventData, index) => {
                playerItemListView.ChangeItemStackIndex(index, _draggingItemIndex);
                playerItemListView.Refresh();
            };
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
            Player.Instance.PlayerInventory.Changed += playerItemListView.Refresh;
        }

        private void OnDisable()
        {
            Player.Instance.PlayerInventory.Changed -= playerItemListView.Refresh;
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

        private int _panelRequestId;
        private int _draggingItemIndex;
        private ItemStack _draggingItemStack;
        private ItemIcon _ItemIcon;
        private Transform _draggingItemIcon;
        private PlayerInventory.ItemCategory _currentCategory;
    }
}