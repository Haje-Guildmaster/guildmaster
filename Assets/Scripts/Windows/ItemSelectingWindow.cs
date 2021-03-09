using GuildMaster.Data;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Windows
{
    public class ItemSelectingWindow : DraggableWindow
    {
        [SerializeField] public AutoRefreshedInventoryView _selectedInventoryView;
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
            switch (await UiWindowsManager.Instance.AsyncShowMessageBox("확인", "가방으로 이동하겠습니까?",
                new[] {"확인", "취소"}))
            {
                case 0:
                    // Todo: 아이템 보내기&기능 분리?
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
            switch (await UiWindowsManager.Instance.AsyncShowMessageBox("확인", "가방으로 이동하겠습니까?",
                new[] {"확인", "취소"}))
            {
                case 0:
                    // Todo: 아이템 보내기&기능 분리?
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