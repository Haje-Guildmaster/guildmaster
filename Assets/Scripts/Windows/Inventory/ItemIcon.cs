
using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Tools;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        public event Action<Item> PointerEntered;
        public event Action PointerExited;
        public event Action<PointerEventData, int> BeginDrag;
        public event Action EndDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData, int> Drop;
        public event Action<Item, int> Click;

        [SerializeField] private Image _itemImage;
        [SerializeField] private Text _itemNumberLabel;

        ItemIcon(Item _item, int _number, int _index)
        {
            UpdateAppearance(_item, _number, _index);
        }

        public void UpdateAppearance(Item _item, int _number, int _index)
        {
            if (_item == null || _number == 0)
            {
                this._item = null;
                this._number = 0;
                this._index = _index;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
            }
            else
            {
                this._item = _item;
                this._number = _number;
                this._index = _index;
                _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
                _itemNumberLabel.text = _number.ToString();
            }
            return;
        }

        public void ItemIconOnOff(bool onoff)
        {
            if (onoff) gameObject.GetComponent<Image>().color = Color.white;
            else gameObject.GetComponent<Image>().color = Color.red;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(_item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(eventData, _index);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            Drop?.Invoke(eventData, _index);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Click?.Invoke(_item, _number);
        }

        private Item _item = null;
        private int _number = 0, _index;
    }
}