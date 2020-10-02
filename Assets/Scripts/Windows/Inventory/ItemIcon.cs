using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace GuildMaster.Windows.Inventory
{
    public class ItemIcon: GenericButton<int>, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {
        public Image itemImage;
        public Text itemNumberLabel;
        public Item item => _item;
        public int number => _number;
        public int index => _index;

        public void UpdateAppearance(Item item, int number, int index)
        {
            this._item = item;
            if(item == null || number == 0)
            {
                itemImage.sprite = null;
                itemNumberLabel.text ="0";
                _index = index;
                _number = 0;
                return;
            }
            itemImage.sprite = ItemDatabase.Get(item.Code).ItemImage;
            itemNumberLabel.text = number.ToString();
            _number = number;
            _index = index;
            return;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_item != null)
                _panelRequestId = UiWindowsManager.Instance.itemInfoPanel.Open(_item.Code);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_panelRequestId == 0) return;
            UiWindowsManager.Instance.itemInfoPanel.Close(_panelRequestId);
            _panelRequestId = 0;
        }

        public void OnDrag(PointerEventData data)
        {
            if (_item == null) return;
            if (transform.childCount > 0)
                transform.GetChild(0).parent = transform.GetChild(0);
            transform.GetChild(0).position = data.position;
        }

        protected override int EventArgument => _index;
        private Item _item;
        private int _panelRequestId, _index, _number;
    }
}