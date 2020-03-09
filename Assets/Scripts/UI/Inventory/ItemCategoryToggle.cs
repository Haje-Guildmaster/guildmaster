using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class ItemCategoryToggle: ToggleOnColor
    {
        public InventoryWindow.ItemCategory category;
    }
}