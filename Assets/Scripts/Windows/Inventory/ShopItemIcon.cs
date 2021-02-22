
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
        [SerializeField] private Text _itemQuantityLabel;
        public ShopItemIcon(Item item, int number, int index, int cost, int quantity) 
            : base(item, number, index)
        {
            UpdateAppearance(item, number, index, cost, quantity);
        }

        public void UpdateAppearance(Item item, int number, int index, int cost, int quantity)
        {
            if (item == null || number == 0)
            {
                _item = null;
                _number = 0;
                _index = index;
                _cost = cost;
                _quantity = 0;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
                _itemCostLabel.text = "";
                _itemQuantityLabel.text = "";
            }
            else
            {
                _item = item;
                _number = number;
                _index = index;
                _cost = cost;
                _quantity = quantity;
                _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
                _itemNumberLabel.text = _number.ToString();
                _itemCostLabel.text = _cost.ToString();
                if (_quantity == 0)
                    _itemQuantityLabel.text = "";
                else
                    _itemQuantityLabel.text = _quantity.ToString();
            }
            return;
        }
        private int _cost;
        private int _quantity;
    }
}