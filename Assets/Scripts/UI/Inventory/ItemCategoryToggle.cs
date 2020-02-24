using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    [RequireComponent(typeof(Toggle))]
    public class ItemCategoryToggle: MonoBehaviour
    {
        public InventoryWindow.ItemCategory category;
        private ColorBlock _originalColor;
        public ColorBlock selectedColor;
        
        private void Awake()
        {
            Toggle = GetComponent<Toggle>();
            _originalColor = Toggle.colors;
            Toggle.onValueChanged.AddListener(UpdateToggleColor);
        }

        private void UpdateToggleColor(bool isOn)
        {
            Toggle.colors = isOn ? selectedColor : _originalColor;
        }
        public Toggle Toggle { get; private set; }
    }
}