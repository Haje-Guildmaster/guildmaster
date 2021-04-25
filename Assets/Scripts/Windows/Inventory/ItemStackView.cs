using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ItemStackView: MonoBehaviour
    {
        [field: SerializeField] public Image ItemImage { get; private set; }
        [field: SerializeField] public Text ItemNumberLabel { get; private set; }

        public ItemStack ItemStack
        {
            get => _itemStack;
            set
            {
                _itemStack = value;
                UpdateAppearance();
            }
        }

        private void UpdateAppearance()
        {
            Item item = _itemStack.Item;
            int number = _itemStack.ItemNum;
            if (item == null || number == 0)
            {
                ItemImage.sprite = null;
                ItemNumberLabel.text = "";
            }
            else
            {
                ItemImage.sprite = item.StaticData.ItemImage;
                ItemNumberLabel.text = number.ToString();
            }
        }

        private ItemStack _itemStack;
    }
}