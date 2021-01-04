using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 이벤트에서 선택지 하나를 표시하는 버튼. ScrollPicker의 자식으로 들어감.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(CanvasGroup))]
    public class ChoiceSelectorElement : MonoBehaviour, IScrollPickerElement
    {
        public Text DescriptionLabel;

        private void Awake()
        {
            Button = GetComponent<Button>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public Transform Transform => transform;

        public float Alpha
        {
            get => _canvasGroup.alpha;
            set => _canvasGroup.alpha = value;
        }

        public Button Button { get; private set; }
        private CanvasGroup _canvasGroup;
    }
}