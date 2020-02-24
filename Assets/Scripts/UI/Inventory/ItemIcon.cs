using GuildMaster.Items;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class ItemIcon: MonoBehaviour
    {
        public Image itemImage;
        public Text itemNumberLabel;

        public void UpdateAppearance(Item item, int number)
        {
            itemImage.sprite = ItemDatabase.Instance.GetItemStaticData(item.Code).ItemImage;
            itemNumberLabel.text = number.ToString();
        }

        public void Clear()
        {
            itemImage.sprite = null;
            itemNumberLabel.text = "";
        }
    }
}