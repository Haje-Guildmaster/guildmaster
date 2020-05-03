using GuildMaster.Database;
using GuildMaster.Items;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.UI.Inventory
{
    public class ItemIcon: GenericButton<Item>, IPointerEnterHandler, IPointerExitHandler
    {
        public Image itemImage;
        public Text itemNumberLabel;

        public void UpdateAppearance(Item item, int number)
        {
            this._item = item;
            itemImage.sprite = ItemDatabase.Get(item.Code).ItemImage;
            itemNumberLabel.text = number.ToString();
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

        protected override Item EventArgument => _item;

        public void Clear()
        {
            itemImage.sprite = null;
            itemNumberLabel.text = "";
        }


        private Item _item;
        private int _panelRequestId;
    }
}