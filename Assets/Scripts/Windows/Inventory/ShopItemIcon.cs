
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
        public ShopItemIcon(Item item, int number, int index, int cost, int quantity, bool infinite) 
            : base()
        {
            UpdateAppearance(item, number, index, cost, quantity, infinite);
        }
        public void UpdateAppearance(Item item, int number, int index, int cost, int quantity, bool infinite)
        {
            if (item == null || (number == 0 && infinite == false))
            {
                _item = null;
                _number = 0;
                _index = index;
                _cost = cost;
                _quantity = 0;
                _isinfinite = infinite;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
                _itemCostLabel.text = "";
                _itemQuantityLabel.text = "";
            }
            else if (infinite == true)
            {
                _item = item;
                _number = 0;
                _index = index;
                _cost = cost;
                _quantity = quantity;
                _isinfinite = true;
                _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
                _itemNumberLabel.text = "";
                _itemCostLabel.text = _cost.ToString();
                if (_quantity == 0)
                    _itemQuantityLabel.text = "";
                else
                    _itemQuantityLabel.text = _quantity.ToString();
            }
            else
            {
                _item = item;
                _number = number;
                _index = index;
                _cost = cost;
                _quantity = quantity;
                _isinfinite = false;
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
        private bool _isinfinite;
    }
}