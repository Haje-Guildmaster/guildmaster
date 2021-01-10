using GuildMaster.Data;
using GuildMaster.Items;
using System;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class ExplorationItemSelectingWindow : DraggableWindow
    {
        // Start is called before the first frame update
        [SerializeField] public ItemListView bagWindow;
        [SerializeField] private PlayerItemListView playerItemListView;
        [SerializeField] private int _panelRequestId;

        public class Response
        {
            public enum ActionEnum
            {
                Cancel,
                GoBack,
                GoNext,
            }

            public ActionEnum NextAction;
        }

        public async Task<Response> GetResponse(Inventory targetInventory,
            CancellationToken cancellationToken = default)
        {
            return await _getResponseSingularRun.Run(async linkedCancellationToken =>
            {
                try
                {
                    // 타겟 인벤토리 변경.
                    _exploreInventory = targetInventory;
                    bagWindow.SetInventory(_exploreInventory);
                    
                    // 윈도우 초기 화면
                    base.OpenWindow();
                    Refresh();

                    // 입력 기다리기
                    _responseTaskCompletionSource = new TaskCompletionSource<Response>();
                    return await _responseTaskCompletionSource.CancellableTask(linkedCancellationToken);
                }
                finally
                {
                    Close();
                }
            }, cancellationToken);
        }

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

        void P_BeginDrag(PointerEventData eventData, int index)
        {
            _draggingItemIndex = index;
            _draggingItemStack = playerItemListView.getItemStack(index);
            if (_draggingItemStack.Item == null) return;
            _currentViewCategory = ItemListView.View_Category.Inventory;
            _currentWindowCategory = ItemListView.Window_Category.ExplorationItemSelectingWindow;

            //아이템이 따라가게 하는건 일단 봉인. 다 만들고 UI 개선시킬때 다시 하겠음.
            //_ItemIcon = Instantiate(playerItemListView.draggingItemIcon, transform);
            //_ItemIcon.UpdateAppearance(_draggingItemStack.Item, _draggingItemStack.ItemNum, index);
            //_draggingItemIcon = Instantiate(_ItemIcon.transform, GameObject.FindGameObjectWithTag("Canvas").transform);

            //Destroy(_ItemIcon.gameObject);
            playerItemListView.OnOffItemIcon(false, _draggingItemIndex);
        }

        void B_BeginDrag(PointerEventData eventData, int index)
        {
            _draggingItemIndex = index;
            _draggingItemStack = bagWindow.getItemStack(index);
            if (_draggingItemStack.Item == null) return;
            _currentViewCategory = ItemListView.View_Category.Bag;
            _currentWindowCategory = ItemListView.Window_Category.ExplorationItemSelectingWindow;

            //아이템이 따라가게 하는건 일단 봉인. 다 만들고 UI 개선시킬때 다시 하겠음.
            //_ItemIcon = Instantiate(playerItemListView.draggingItemIcon, transform);
            //_ItemIcon.UpdateAppearance(_draggingItemStack.Item, _draggingItemStack.ItemNum, index);
            //_draggingItemIcon = Instantiate(_ItemIcon.transform, GameObject.FindGameObjectWithTag("Canvas").transform);

            //Destroy(_ItemIcon.gameObject);
            bagWindow.OnOffItemIcon(false, _draggingItemIndex);
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
            bagWindow.OnOffItemIcon(true, _draggingItemIndex);
        }

        void P_Drop(PointerEventData eventData, int index)
        {
            if (_draggingItemStack == null) return;
            if (_draggingItemStack.Item == null || _draggingItemStack.ItemNum == 0) return;
            if (_currentWindowCategory != ItemListView.Window_Category.ExplorationItemSelectingWindow) return;
            if (_currentViewCategory == ItemListView.View_Category.Inventory)
            {
                if (_draggingItemIndex != index)
                {
                    playerItemListView.ChangeItemStackIndex(index, _draggingItemIndex);
                }
            }
            else if (_currentViewCategory == ItemListView.View_Category.Bag)
            {
                P_Clicked(playerItemListView.getItemStack(index).Item, playerItemListView.getItemStack(index).ItemNum);
            }

            Refresh();
            return;
        }

        void B_Drop(PointerEventData eventData, int index)
        {
            if (_draggingItemStack == null) return;
            if (_draggingItemStack.Item == null || _draggingItemStack.ItemNum == 0) return;
            if (_currentWindowCategory != ItemListView.Window_Category.ExplorationItemSelectingWindow) return;
            if (_currentViewCategory == ItemListView.View_Category.Bag)
            {
                if (_draggingItemIndex != index)
                {
                    bagWindow.ChangeItemStackIndex(index, _draggingItemIndex);
                }
            }
            else if (_currentViewCategory == ItemListView.View_Category.Inventory)
            {
                B_Clicked(_exploreInventory.TryGetItemStack(index).Item,
                    _exploreInventory.TryGetItemStack(index).ItemNum);
            }

            Refresh();
            return;
        }

        void P_Clicked(Item item, int number)
        {
            if (item == null) return;
            UiWindowsManager.Instance.ShowMessageBox("확인", "가방으로 이동하겠습니까?",
                new (string buttonText, Action onClicked)[]
                {
                    ("확인", () =>
                    {
                        Player.Instance.PlayerInventory.TryDeleteItem(item, number);
                        _exploreInventory.TryAddItem(item, number);
                        Refresh();
                    }),
                    ("취소", () => Debug.Log("취소"))
                });
        }

        void B_Clicked(Item item, int number)
        {
            if (item == null) return;
            UiWindowsManager.Instance.ShowMessageBox("확인", "인벤토리로 이동하겠습니까?",
                new (string buttonText, Action onClicked)[]
                {
                    ("확인", () =>
                    {
                        _exploreInventory.TryDeleteItem(item, number);
                        Player.Instance.PlayerInventory.TryAddItem(item, number);
                        Refresh();
                    }),
                    ("취소", () => Debug.Log("취소"))
                });
        }

        public void GoNext()
        {
            _responseTaskCompletionSource.TrySetResult(
                new Response
                {
                    NextAction = Response.ActionEnum.GoNext,
                });
        }

        public void Back()
        {
            _responseTaskCompletionSource.TrySetResult(
                new Response
                {
                    NextAction = Response.ActionEnum.GoBack,
                });
        }

        protected override void OnClose()
        {
            _responseTaskCompletionSource.TrySetResult(
                new Response
                {
                    NextAction = Response.ActionEnum.Cancel,
                });
        }

        private void Awake()
        {
            playerItemListView.SetPlayerInventory(Player.Instance.PlayerInventory);
            bagWindow.SetInventory(_exploreInventory);
            playerItemListView.PointerEntered -= PointerEntered;
            playerItemListView.PointerExited -= PointerExited;
            playerItemListView.BeginDrag -= P_BeginDrag;
            playerItemListView.Drag -= Drag;
            playerItemListView.EndDrag -= EndDrag;
            playerItemListView.Drop -= P_Drop;
            playerItemListView.Click -= P_Clicked;

            playerItemListView.PointerEntered += PointerEntered;
            playerItemListView.PointerExited += PointerExited;
            playerItemListView.BeginDrag += P_BeginDrag;
            playerItemListView.Drag += Drag;
            playerItemListView.EndDrag += EndDrag;
            playerItemListView.Drop += P_Drop;
            playerItemListView.Click += P_Clicked;

            bagWindow.PointerEntered -= PointerEntered;
            bagWindow.PointerExited -= PointerExited;
            bagWindow.BeginDrag -= B_BeginDrag;
            bagWindow.Drag -= Drag;
            bagWindow.EndDrag -= EndDrag;
            bagWindow.Drop -= B_Drop;
            bagWindow.Click -= B_Clicked;

            bagWindow.PointerEntered += PointerEntered;
            bagWindow.PointerExited += PointerExited;
            bagWindow.BeginDrag += B_BeginDrag;
            bagWindow.Drag += Drag;
            bagWindow.EndDrag += EndDrag;
            bagWindow.Drop += B_Drop;
            bagWindow.Click += B_Clicked;
            Refresh();
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
            bagWindow.Refresh();
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

            playerItemListView.ChangeCategory((int) category);

            _changeCategoryBlock = false;
        }

        private readonly SingularRun _getResponseSingularRun = new SingularRun();
        private Inventory _exploreInventory = new Inventory(12, true);
        private PlayerInventory.ItemCategory _currentCategory;
        private int _draggingItemIndex;
        private ItemStack _draggingItemStack;
        private ItemListView.Window_Category _currentWindowCategory;
        private ItemListView.View_Category _currentViewCategory;

        private TaskCompletionSource<Response> _responseTaskCompletionSource = new TaskCompletionSource<Response>();
    }
}