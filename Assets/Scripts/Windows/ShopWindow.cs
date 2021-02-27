using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ShopWindow : DraggableWindow, IToggleableWindow
    {
        [SerializeField] private Text ShopName; 
        [SerializeField] private ShopItemListView shopItemListView;
        [SerializeField] private PlayerShopItemListView playerShopItemListView;
        [SerializeField] private Text BuyText;
        [SerializeField] private Text SellText;
        [SerializeField] private Text TotalText;
        public void CloseWindow()
        {
            UiWindowsManager.Instance.shopItemPanel.Close();
            UiWindowsManager.Instance.itemInfoPanel.Close();
            base.Close();
        }
        /// <summary>
        /// 외부에서 열 땐 이 메서드 말고 오버라이딩 된 다른 Open 메서드로 윈도우를 오픈해주세요.
        /// </summary>
        public void Open()
        {
            if (!initialized) throw new ArgumentException("ShopWindow는 npc 인벤토리를 넘겨받은 후 실행되어야 합니다.");
            base.OpenWindow();
        }
        /// <summary>
        /// 거래하는 npc의 인벤토리를 넘겨받으면서 열리는 open 코드
        /// </summary>
        /// <param name="npcInventoryInf"></param>
        /// <param name="npcInventoryNotInf"></param>
        /// <param name="shopname"></param>
        public void Open(ReadOnlyCollection<Item> npcInventoryInf, ReadOnlyCollection<ItemCount> npcInventoryNotInf, string shopname)
        {
            this.npcInventoryInf = npcInventoryInf;
            this.npcInventoryNotInf = new List<ItemCount>();
            foreach (ItemCount itemCount in npcInventoryNotInf)
            {
                this.npcInventoryNotInf.Add(new ItemCount(itemCount.Item, itemCount.Number));
            }
            this.shopname = shopname;
            BuildNPCInventory();
            Initialize();
            Open();
            Refresh();
        }
        void PointerEntered(Item item)
        {
            if (item != null)
                _panelRequestId = UiWindowsManager.Instance.itemInfoPanel.Open(item.Code);
        }

        void PointerExited()
        {
            if (_panelRequestId == 0) return;
            UiWindowsManager.Instance.itemInfoPanel.Close(_panelRequestId);
            _panelRequestId = 0;
        }
        void PlayerClicked(ItemStack itemStack, int index)
        {
            ResetIconOnOff();
            playerShopItemListView.OnOffItemIcon(true, index);
            UiWindowsManager.Instance.shopItemPanel.Open(itemStack, false);
        }

        void ShopClicked(ItemStack itemStack, int index)
        {
            ResetIconOnOff();
            shopItemListView.OnOffItemIcon(true, index);
            UiWindowsManager.Instance.shopItemPanel.Open(itemStack, true);
        }
        private bool _changeCategoryBlock = false;
        public void ChangeCategory(PlayerInventory.ItemCategory category)
        {
            if (_changeCategoryBlock) return;
            _changeCategoryBlock = true;
            _currentCategory = category;
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                ict.Toggle.isOn = ict.category == category;
            }
            playerShopItemListView.ChangeCategory((int)category);

            _changeCategoryBlock = false;
        }
        private void Start()
        {
            foreach (var ict in GetComponentsInChildren<ItemCategoryToggle>())
            {
                var cat = ict.category;
                ict.Toggle.onValueChanged.AddListener(b =>
                {
                    if (b) ChangeCategory(cat);
                });
            }
            ChangeCategory(PlayerInventory.ItemCategory.Equipable);
        }
        private void Initialize()
        {
            initialized = true;
            ShopName.text = shopname;
            playerShopItemListView.SetPlayerInventory(Player.Instance.PlayerInventory);
            shopItemListView.SetInventory(npcInventory);
            
        }
        private void BuildNPCInventory()
        {
            npcInventory = new Inventory(npcInventoryInf.Count + npcInventoryNotInf.Count, false);
            foreach (Item item in npcInventoryInf)
            {
                if (item == null) continue;
                npcInventory.TryAddInfiniteItem(item);
            }
            foreach (ItemCount itemcount in npcInventoryNotInf)
            {
                if (itemcount.Item == null) continue;
                npcInventory.TryAddItem(itemcount.Item, itemcount.Number);
            }
        }
        private void Refresh()
        {
            shopItemListView.Refresh();
            playerShopItemListView.Refresh();
            //이벤트 구독
            shopItemListView.PointerEntered -= PointerEntered;
            shopItemListView.PointerEntered += PointerEntered;
            shopItemListView.PointerExited -= PointerExited;
            shopItemListView.PointerExited += PointerExited;
            shopItemListView.SClick -= ShopClicked;
            shopItemListView.SClick += ShopClicked;
            playerShopItemListView.PointerEntered -= PointerEntered;
            playerShopItemListView.PointerEntered += PointerEntered;
            playerShopItemListView.PointerExited -= PointerExited;
            playerShopItemListView.PointerExited += PointerExited;
            playerShopItemListView.SClick -= PlayerClicked;
            playerShopItemListView.SClick += PlayerClicked;
        }
        private void ResetIconOnOff()
        {
            shopItemListView.ResetOnOffItemIcon();
            playerShopItemListView.ResetOnOffItemIcon();
        }
        private int _panelRequestId;
        private ReadOnlyCollection<Item> npcInventoryInf;
        private List<ItemCount> npcInventoryNotInf;
        private Dictionary<int, int> shopOnDict = new Dictionary<int, int>();
        private Dictionary<int, int> playerOnDict = new Dictionary<int, int>();
        private Inventory npcInventory;
        private string shopname;
        private bool initialized = false;
        private PlayerInventory.ItemCategory _currentCategory;
    }
}

