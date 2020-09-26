using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.Windows.Inventory
{
    public class ItemCategoryToggle: ColorIfToggleIsOn
    {
        public ItemWindow.ItemCategory category;
    }
}