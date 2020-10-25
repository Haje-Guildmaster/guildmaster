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
        public event Action<PointerEventData> PointerEntered;
        public event Action<PointerEventData> PointerExited;
        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> EndDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> Drop;

        [SerializeField] private Image _itemImage;
        [SerializeField] private Text _itemNumberLabel;

        public void UpdateAppearance(Item item, int number)
        {
            if (item == null || number == 0)
            {
                _itemImage.sprite = null;
                _itemNumberLabel.text = "";
                return;
            }
            _itemImage.sprite = ItemDatabase.Get(item.Code).ItemImage;
            _itemNumberLabel.text = number.ToString();
            return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            Drop?.Invoke(eventData);
        }

        private ItemListView _itemWindow;
        private Item _item;

        private int _panelRequestId; //static
    }
}