using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Tools
{
    [RequireComponent(typeof(Button))]
    public class AsyncButton: MonoBehaviour
    {
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        public async Task WaitForClick()
        {
            await _clickTaskCompletionSource.Task;
        }
        private void OnClick()
        {
            var temp = _clickTaskCompletionSource;
            _clickTaskCompletionSource = new TaskCompletionSource<bool>();
            temp.SetResult(true);
        }

        private TaskCompletionSource<bool> _clickTaskCompletionSource = new TaskCompletionSource<bool>();
        private Button _button;
    }
}