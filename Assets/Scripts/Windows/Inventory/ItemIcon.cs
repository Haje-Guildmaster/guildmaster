using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Items;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.Windows.Inventory
{
    public class ItemIcon : GenericButton<int>, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Image itemImage;
        public Text itemNumberLabel;
        public Item item => _item;
        public int number => _number;
        public int index => _index;
        public Data.Inventory currentInventory => _currentInventory;
        public static int panelRequestId => _panelRequestId;

        public void UpdateAppearance(Item item, int number, int index, Data.Inventory inventory)
        {
            this._item = item;
            if (item == null || number == 0)
            {
                itemImage.sprite = null;
                itemNumberLabel.text = "";
                _number = 0;
                _index = index;
                _currentInventory = inventory;
                return;
            }
            itemImage.sprite = ItemDatabase.Get(item.Code).ItemImage;
            itemNumberLabel.text = number.ToString();
            _number = number;
            _index = index;
            _currentInventory = inventory;
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

        public void OnDrag(PointerEventData eventData)
        {
            if (_item == null) return;
            if (!makeItem)
            {
                _thisitem = Instantiate(transform.GetChild(0), GameObject.FindGameObjectWithTag("Canvas").transform);
                makeItem = true;
            }
            _thisitem.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_item == null) return;
            int _currentcategory = Data.Inventory.getItemToCategoryNum(_item);
            ItemWindow.DraggingItem = this;
            GetComponent<Image>().color = UnityEngine.Color.red;
            foreach(var im in GetComponentsInParent<Image>())
            {
                im.raycastTarget = false;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_item == null) return;
            Destroy(_thisitem.gameObject);
            _thisitem = null;
            makeItem = false;
            GetComponent<Image>().color = UnityEngine.Color.white;
            foreach (var im in GetComponentsInParent<Image>())
            {
                im.raycastTarget = true;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("이예에에");
            ItemStack t1, t2;
            int categoryNum = Data.Inventory.getItemToCategoryNum(_item);
            if (_currentInventory.IsStacked) categoryNum = 0;
            var (_item1, _itemnum1) = _currentInventory.InventoryAList[categoryNum][_index].getItemStack();
            categoryNum = Data.Inventory.getItemToCategoryNum(_item);
            if (ItemWindow.DraggingItem.currentInventory.IsStacked) categoryNum = 0;
            var (_item2, _itemnum2) = ItemWindow.DraggingItem.currentInventory.InventoryAList[categoryNum][ItemWindow.DraggingItem.index].getItemStack();
            _currentInventory.TryDeleteItem(_item1, _itemnum1);
            ItemWindow.DraggingItem.currentInventory.TryDeleteItem(_item2, _itemnum2);
            _currentInventory.TryAddItem(_item2, _itemnum2);
            ItemWindow.DraggingItem.currentInventory.TryAddItem(_item1, _itemnum1);
        }

        private int _currentCategory => Data.Inventory.getItemToCategoryNum(_item);
        private bool makeItem = false;
        private Transform _thisitem;
        protected override int EventArgument => _index;
        private Data.Inventory _currentInventory;
        private Item _item;
        private int _index, _number;
        private static int _panelRequestId;
    }
}