
using GuildMaster.Data;
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
        public event Action<int> SClick;
        public ItemStack ItemStack
        {
            get
            {
                if (_isinfinite)
                    return new ItemStack(itemStack.Item, true);
                else
                    return new ItemStack(itemStack.Item, itemStack.ItemNum);
            }
        }
            
        public ShopItemIcon(ItemStack itemStack, int index, bool isbuy) 
            : base()
        {
            UpdateAppearance(itemStack, index, isbuy);
        }
        public void UpdateAppearance(ItemStack itemStack, int index, bool isbuy)
        {
            this.itemStack = itemStack;
            if (itemStack == null || itemStack.Item == null)
            {
                _item = null;
                _number = 0;
                _index = index;
                _cost = 0;
                _quantity = 0;
                _isinfinite = false;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
                _itemCostLabel.text = "";
                _itemQuantityLabel.text = "";
            }
            else if (itemStack.isInfinite == true)
            {
                _item = itemStack.Item;
                _number = 0;
                _index = index;
                if (isbuy)
                    _cost = itemStack.BuyCost;
                else
                    _cost = itemStack.SellCost;
                _quantity = itemStack.Quantity;
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
                _item = itemStack.Item;
                _number = itemStack.ItemNum;
                _index = index;
                if (isbuy)
                    _cost = itemStack.BuyCost;
                else
                    _cost = itemStack.SellCost;
                _quantity = itemStack.Quantity;
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
        public override void OnPointerClick(PointerEventData eventData)
        {
            SClick?.Invoke(_index);
        }
        private ItemStack itemStack;
        private int _cost;
        private int _quantity;
        private bool _isinfinite;
    }
}