
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
    public class ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        public event Action<Item> PointerEntered;
        public event Action PointerExited;
        public event Action<PointerEventData, int> BeginDrag;
        public event Action EndDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData, int> Drop;
        public event Action<Item, int> Click;

        [SerializeField] protected Image _itemImage;
        [SerializeField] protected Text _itemNumberLabel;
        protected ItemIcon() { }
        public ItemIcon(ItemStack itemStack, int index)
        {
            UpdateAppearance(itemStack, index);
        }

        public void UpdateAppearance(ItemStack itemStack, int index)
        {
            if (itemStack == null || itemStack.Item == null)
            {
                _item = null;
                _number = 0;
                _index = index;
                _itemImage.sprite = (Sprite)null;
                _itemNumberLabel.text = "";
            }
            else
            {
                _item = itemStack.Item;
                _number = itemStack.ItemNum;
                _index = index;
                _itemImage.sprite = ItemDatabase.Get(_item.Code).ItemImage;
                _itemNumberLabel.text = _number.ToString();
            }
            return;
        }
        /// <summary>
        /// true°¡ 
        /// </summary>
        /// <param name="onoff"></param>
        public void ItemIconOnOff(bool onoff)
        {
            if (onoff) gameObject.GetComponent<Image>().color = Color.red;
            else gameObject.GetComponent<Image>().color = Color.white;
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

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Click?.Invoke(_item, _number);
        }
        protected Item _item = null;
        protected int _number = 0, _index;
    }
}