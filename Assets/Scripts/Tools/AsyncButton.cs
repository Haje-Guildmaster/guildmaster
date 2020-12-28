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
            await _clickWaiter.Wait();
        }
        private void OnClick()
        {
            _clickWaiter.Signal();
        }

        private SignalWaiter _clickWaiter = new SignalWaiter();
        private Button _button;
    }
}