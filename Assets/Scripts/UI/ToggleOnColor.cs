using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleOnColor: MonoBehaviour
    {
        private ColorBlock _originalColor;
        public ColorBlock onColor;
        
        private void Awake()
        {
            Toggle = GetComponent<Toggle>();
            _originalColor = Toggle.colors;
            Toggle.onValueChanged.AddListener(UpdateToggleColor);
        }

        private void UpdateToggleColor(bool isOn)
        {
            Toggle.colors = isOn ? onColor : _originalColor;
        }
        public Toggle Toggle { get; private set; }
    }
}