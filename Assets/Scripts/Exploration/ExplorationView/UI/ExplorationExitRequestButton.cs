using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 클릭했을 시 ExplorationManager에 탐색 종료를 요청합니다.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ExplorationExitRequestButton: MonoBehaviour
    {
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ExplorationManager.Instance.RequestExit();
        }

        private Button _button;
    }
}