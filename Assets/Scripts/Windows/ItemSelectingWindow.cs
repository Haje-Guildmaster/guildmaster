using GuildMaster.Data;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Items;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Windows
{
    public class ItemSelectingWindow : DraggableWindow
    {
        [SerializeField] private AutoRefreshedInventoryView _selectedInventoryView;
        [SerializeField] private PlayerInventoryView _playerInventoryView;

        private void Awake()
        {
            _playerInventoryView.InventoryView.ClickedItemIcon += MoveItemPlayerInventoryToSelected;
            _selectedInventoryView.ClickedItemIcon += MoveItemSelectedToPlayerInventory;
        }

        private void Start()
        {
            _selectedInventoryView.SetInventory(_selectedItemInventory);
            _playerInventoryView.SetPlayerInventory(Player.Instance.PlayerInventory);
        }

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
                    _selectedItemInventory = targetInventory;
                    _selectedInventoryView.SetInventory(_selectedItemInventory);

                    // 윈도우 초기 화면
                    base.OpenWindow();

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


        private async void MoveItemPlayerInventoryToSelected(int playerInventoryItemIndex)
        {
            var fromInv = _playerInventoryView.CurrentInventory;
            var capture = fromInv.GetItemStack(playerInventoryItemIndex);
            if (capture.IsEmpty()) return;
            
            switch (await UiWindowsManager.Instance.AsyncShowMessageBox("확인", "가방으로 이동하겠습니까?",
                new[] {"확인", "취소"}))
            {
                case 0:
                    var movingStack = fromInv.GetItemStack(playerInventoryItemIndex);
                    // 질문을 할 때와 지정된 위치의 스택에 변화가 생겼으면 이동 취소.
                    if (!capture.Equals(movingStack)) return;
                    var addedNum = _selectedItemInventory.TryAddItem(movingStack);
                    fromInv.RemoveItemFromIndex(playerInventoryItemIndex, addedNum, movingStack.Item);
                    if (addedNum != movingStack.ItemNum)
                        Debug.LogWarning("Todo: 아이템 옮기기 실패 알리기");
                    break;
                case 1:
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        private async void MoveItemSelectedToPlayerInventory(int selectedInventoryItemIndex)
        {
            var fromInv = _selectedItemInventory;
            var capture = fromInv.GetItemStack(selectedInventoryItemIndex);
            if (capture.IsEmpty()) return;
            switch (await UiWindowsManager.Instance.AsyncShowMessageBox("확인", "가방으로 이동하겠습니까?",
                new[] {"확인", "취소"}))
            {
                case 0:
                    var movingStack = fromInv.GetItemStack(selectedInventoryItemIndex);
                    // 질문을 할 때와 지정된 위치의 스택에 변화가 생겼으면 이동 취소.
                    if (!capture.Equals(movingStack)) return;
                    var addedNum = _playerInventoryView.CurrentPlayerInventory.TryAddItem(movingStack);
                    fromInv.RemoveItemFromIndex(selectedInventoryItemIndex, addedNum, movingStack.Item);
                    if (addedNum != movingStack.ItemNum)
                        Debug.LogWarning("Todo: 아이템 옮기기 실패 알리기");
                    break;
                case 1:
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }


        private readonly SingularRun _getResponseSingularRun = new SingularRun();
        private Inventory _selectedItemInventory = new Inventory(12, true);
        private TaskCompletionSource<Response> _responseTaskCompletionSource = new TaskCompletionSource<Response>();
    }
}