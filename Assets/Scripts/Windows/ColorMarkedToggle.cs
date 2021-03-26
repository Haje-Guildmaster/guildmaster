using System;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    [RequireComponent(typeof(Toggle))]
    public class ColorMarkedToggle: MonoBehaviour
    {
        private ColorBlock _originalColor;
        public ColorBlock onColor;

        public event Action<bool> ValueChanged;
        
        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _originalColor = _toggle.colors;
            _toggle.onValueChanged.AddListener(UpdateToggleColor);
            _toggle.onValueChanged.AddListener(b => ValueChanged?.Invoke(b));
        }

        public bool IsOn
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }

        public void SetIsOnWithoutNotify(bool isOn)
        {
            UpdateToggleColor(isOn);
            _toggle.SetIsOnWithoutNotify(isOn);
        }
        
        private void UpdateToggleColor(bool isOn)
        {
            _toggle.colors = isOn ? onColor : _originalColor;
        }

        private Toggle _toggle;
    }
}