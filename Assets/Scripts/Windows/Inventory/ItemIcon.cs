using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.Windows.Inventory
{
    public class ItemIcon : GenericButton<int>, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        public Image itemImage;
        public Text itemNumberLabel;
        public Item item => _item;
        public int number => _number;
        public int index => _index;
        public Data.Inventory currentInventory => _currentInventory;
        public static int panelRequestId => _panelRequestId;

        public void UpdateAppearance(Item item, int number, int index, Data.Inventory inventory, ItemWindow itemWindow)
        {
            this._item = item;
            if (item == null || number == 0)
            {
                itemImage.sprite = null;
                itemNumberLabel.text = "";
                _number = 0;
                _index = index;
                _currentInventory = inventory;
                _itemWindow = itemWindow;
                return;
            }
            itemImage.sprite = ItemDatabase.Get(item.Code).ItemImage;
            itemNumberLabel.text = number.ToString();
            _number = number;
            _index = index;
            _currentInventory = inventory;
            _itemWindow = itemWindow;
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_item == null) return;
            int _currentcategory = Data.Inventory.getItemToCategoryNum(_item);
            ItemWindow.DraggingItem = this;
            GetComponent<Image>().color = UnityEngine.Color.red;
            //_thisitem = Instantiate(transform.GetChild(0), GameObject.FindGameObjectWithTag("Canvas").transform);
            //this.itemImage.GetComponent<Image>().sprite = null;
            //this.itemNumberLabel.GetComponent<Text>().text = "";
            return;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_item == null) return;
            //_thisitem.position = eventData.position;
            return;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_item == null) return;
            //Destroy(_thisitem.gameObject);
            //_thisitem = null;
            GetComponent<Image>().color = UnityEngine.Color.white;
            //UpdateAppearance(_item, _number, _index, _currentInventory);
            return;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!_currentInventory.Equals(ItemWindow.DraggingItem.currentInventory)) return;
            GetComponent<Image>().color = UnityEngine.Color.white;

            int categoryNum = Data.Inventory.getItemToCategoryNum(ItemWindow.DraggingItem.item);
            if (_currentInventory.IsStacked) categoryNum = 0;
            //var (_item1, _itemnum1) = _currentInventory.InventoryAList[categoryNum][_index].getItemStack();

            //categoryNum = Data.Inventory.getItemToCategoryNum(ItemWindow.DraggingItem.item);
            //if (ItemWindow.DraggingItem.currentInventory.IsStacked) categoryNum = 0;
            //var (_item2, _itemnum2) = ItemWindow.DraggingItem.currentInventory.InventoryAList[categoryNum][ItemWindow.DraggingItem.index].getItemStack();
            //가방 - 인벤토리 간 상호작용을 위해 짜려고 했던 흔적. 일단 인벤토리 - 인벤토리 or 가방 - 가방 상호작용만 구현함

            _currentInventory.ChangeItemIndex(categoryNum, _index, ItemWindow.DraggingItem.index);
            _itemWindow.Refresh();
            return;
        }

        private int _currentCategory => Data.Inventory.getItemToCategoryNum(_item);
        private Transform _thisitem;
        private Data.Inventory _currentInventory;
        private ItemWindow _itemWindow;
        private Item _item;
        private int _index, _number;

        protected override int EventArgument => index;

        private static int _panelRequestId;
    }
}