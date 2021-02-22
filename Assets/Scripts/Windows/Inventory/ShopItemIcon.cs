
using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Tools;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ShopItemIcon : ItemIcon
    {
        [SerializeField] private Text _itemCostLabel;

        public ShopItemIcon(Item item, int number, int index, int cost) 
            : base(item, number, index)
        {
            UpdateAppearance(item, number, index, cost);
        }

        public void UpdateAppearance(Item item, int number, int index, int cost)
        {
            if (item == null || number == 0)
            {
                _item = null;
                _number = 0;
                _index = index;
                _cost = cost;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
                _itemCostLabel.text = "";
            }
            else
            {
                _item = item;
                _number = number;
                _index = index;
                _cost = cost;
                _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
                _itemNumberLabel.text = _number.ToString();
                _itemCostLabel.text = _cost.ToString();
            }
            return;
        }
        private int _cost;
    }
}