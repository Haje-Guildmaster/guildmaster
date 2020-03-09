using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class ItemCategoryColorIfToggleIsOn: ColorIfToggleIsOn
    {
        public InventoryWindow.ItemCategory category;
    }
}