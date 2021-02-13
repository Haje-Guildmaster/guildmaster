using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ItemStackView: MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private Text _itemNumberLabel;

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
                _itemImage.sprite = null;
                _itemNumberLabel.text = "";
            }
            else
            {
                _itemImage.sprite = item.StaticData.ItemImage;
                _itemNumberLabel.text = number.ToString();
            }
        }

        private ItemStack _itemStack;
    }
}