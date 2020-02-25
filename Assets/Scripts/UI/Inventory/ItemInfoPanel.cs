using GuildMaster.Items;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class ItemInfoPanel: MonoBehaviour
    {
        public Image itemImage;
        public Text nameLabel;
        public Text descriptionLabel;

        public void UpdateAppearance(Item.ItemCode itemCode)
        {
            var itemData = ItemDatabase.Instance.GetItemStaticData(itemCode);
            itemImage.sprite = itemData.ItemImage;
            nameLabel.text = itemData.ItemName;
            
        }
    }
}