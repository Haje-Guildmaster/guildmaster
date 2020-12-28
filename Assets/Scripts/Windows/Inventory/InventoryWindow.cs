using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Windows.Inven;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class InventoryWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private PlayerItemListView playerItemListView;
        [SerializeField] private int _panelRequestId;

        void PointerEntered(Item item)
        {
            if (item != null)
                _panelRequestId = UiWindowsManager.Instance.itemInfoPanel.Open(item.Code);
        }

        void PointerExited()
        {
            if (_panelRequestId == 0) return;
            UiWindowsManager.Instance.itemInfoPanel.Close(_panelRequestId);
            _panelRequestId = 0;
        }

        void BeginDrag(PointerEventData eventData, int index)
        {
            _draggingItemIndex = index;
            _draggingItemStack = playerItemListView.getItemStack(index);
            if (_draggingItemStack.Item == null) return;
            _currentWindowCategory = ItemListView.Window_Category.InventoryWindow;

            //아이템이 따라가게 하는건 일단 봉인. 다 만들고 UI 개선시킬때 다시 하겠음.
            //_ItemIcon = Instantiate(playerItemListView.draggingItemIcon, transform);
            //_ItemIcon.UpdateAppearance(_draggingItemStack.Item, _draggingItemStack.ItemNum, index);
            //_draggingItemIcon = Instantiate(_ItemIcon.transform, GameObject.FindGameObjectWithTag("Canvas").transform);

            //Destroy(_ItemIcon.gameObject);
            playerItemListView.OnOffItemIcon(false, _draggingItemIndex);
        }

        void Drag(PointerEventData eventData)
        {
            //if (_draggingItemStack.Item == null) return;
            //_draggingItemIcon.position = eventData.position;
        }

        void EndDrag()
        {
            //Destroy(_draggingItemIcon.gameObject);
            playerItemListView.OnOffItemIcon(true, _draggingItemIndex);
        }

        void Drop(PointerEventData eventData, int index)
        {
            if (_draggingItemStack == null) return;
            if (_draggingItemStack.Item == null) return;
            if (_currentWindowCategory != ItemListView.Window_Category.InventoryWindow) return;
            playerItemListView.ChangeItemStackIndex(index, _draggingItemIndex);
            Refresh();
        }

        public void Open()
        {
            base.OpenWindow();
            Refresh();
        }

        private void Awake()
        {
            playerItemListView.SetPlayerInventory(Player.Instance.PlayerInventory);
            initialized = true;
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
            if (initialized == false) return;
            playerItemListView.Refresh();

            playerItemListView.PointerEntered -= PointerEntered;
            playerItemListView.PointerExited -= PointerExited;
            playerItemListView.BeginDrag -= BeginDrag;
            playerItemListView.Drag -= Drag;
            playerItemListView.EndDrag -= EndDrag;
            playerItemListView.Drop -= Drop;

            playerItemListView.PointerEntered += PointerEntered;
            playerItemListView.PointerExited += PointerExited;
            playerItemListView.BeginDrag += BeginDrag;
            playerItemListView.Drag += Drag;
            playerItemListView.EndDrag += EndDrag;
            playerItemListView.Drop += Drop;
        }

        private bool _changeCategoryBlock = false; //ChangeCategory안에서 ChangeCategory가 다시 실행되는 것 방지.
        // (isOn을 수정하며 이벤트 리스너에 의해 ChangeCategory가 다시 불림)
        private void ChangeCategory(PlayerInventory.ItemCategory category)
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


        private int _draggingItemIndex;
        private ItemStack _draggingItemStack;
        private PlayerInventory.ItemCategory _currentCategory;
        private ItemListView.Window_Category _currentWindowCategory;
        private bool initialized = false;
    }
}