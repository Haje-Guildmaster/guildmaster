using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Tools;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.Windows.Inventory
{
    public class ItemIcon : GenericButton, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        public event Action<PointerEventData, Item> PointerEntered;
        public event Action<PointerEventData> PointerExited;
        public event Action<PointerEventData, int> BeginDrag;
        public event Action<PointerEventData> EndDrag;
        public event Action<PointerEventData, Item, int> Drag;
        public event Action<PointerEventData, int> Drop;

        [SerializeField] private Image _itemImage;
        [SerializeField] private Text _itemNumberLabel;

        public void UpdateAppearance(Item _item, int _number, int _index)
        {
            if (_item == null || _number == 0)
            {
                this._index = _index;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
                return;
            }
            this._item = _item;
            this._number = _number;
            this._index = _index;
            _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
            _itemNumberLabel.text = _number.ToString();
            return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(eventData, _item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(eventData, _index);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData, _item, _number);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            Drop?.Invoke(eventData, _index);
        }

        private Item _item = null;
        private int _number = 0, _index;

        private int _panelRequestId; //static
    }
}