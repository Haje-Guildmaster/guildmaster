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
        [SerializeField] private Text PlayerGold;
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
            EventRefresh();
            Refresh();
        }
        public void GetPanelInfo(ItemStack itemStack, int num, bool isbuying, int index)
        {
            if (isbuying)
            {
                if (num == 0)
                {
                    shopItemListView.ShopItemIconList[index].UpdateAppearance(itemStack, index, isbuying);
                }
                else if (shopOnDict.TryGetValue(itemStack, out int itemnum))
                {
                    shopOnDict[itemStack] = itemnum;
                    if (itemnum == 0)
                    {
                        shopOnDict.Remove(itemStack);
                        shopItemListView.OnOffItemIcon(false, index, true);
                    }
                    shopItemListView.ShopItemIconList[index].UpdateAppearance(itemStack, index, isbuying);
                }
                else
                {
                    shopOnDict.Add(itemStack, num);
                    shopItemListView.OnOffItemIcon(true, index, true);
                    shopItemListView.ShopItemIconList[index].UpdateAppearance(itemStack, index, isbuying);
                }
            }
            else
            {
                if (num != 0)
                    playerOnDict.Add(itemStack, num);
                else if (playerOnDict.ContainsKey(itemStack))
                    playerOnDict.Remove(itemStack);
            }
            Refresh();
        }

        private void PointerEntered(Item item)
        {
            if (item != null)
                _panelRequestId = UiWindowsManager.Instance.itemInfoPanel.Open(item.Code);
        }

        private void PointerExited()
        {
            if (_panelRequestId == 0) return;
            UiWindowsManager.Instance.itemInfoPanel.Close(_panelRequestId);
            _panelRequestId = 0;
        }
        private void ShopClicked(ItemStack itemStack, int index)
        {
            shopItemListView.OnOffItemIcon(true, index, true);
            int num = 0;
            if (shopOnDict.TryGetValue(itemStack, out int n))
                num = n;
            UiWindowsManager.Instance.shopItemPanel.Open(itemStack, num, true, index);
        }
        private void PlayerClicked(ItemStack itemStack, int index)
        {
            playerShopItemListView.OnOffItemIcon(true, index, false);
            int num = 0;
            if (playerOnDict.TryGetValue(itemStack, out int n))
                num = n;
            UiWindowsManager.Instance.shopItemPanel.Open(itemStack, num, false, index);
        }

        private bool _changeCategoryBlock = false;
        private void ChangeCategory(PlayerInventory.ItemCategory category)
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
                if (item == null) throw new ArgumentException("npcInventoryInf의 item이 null입니다.");
                npcInventory.TryAddInfiniteItem(item);
            }
            foreach (ItemCount itemcount in npcInventoryNotInf)
            {
                if (itemcount.Item == null) throw new ArgumentException("npcInventoryNotInf의 item이 null입니다.");
                npcInventory.TryAddItem(itemcount.Item, itemcount.Number);
            }
        }
        private void Refresh()
        {
            //Gold Refresh
            PlayerGold.text = Player.Instance.PlayerGuild.Balance.Value.ToString();
            int buycost = 0, sellcost = 0, totalcost;
            foreach (KeyValuePair<ItemStack, int> pair in shopOnDict)
            {
                buycost += pair.Value * pair.Key.BuyCost;
            }
            foreach (KeyValuePair<ItemStack, int> pair in playerOnDict)
            {
                sellcost += pair.Value * pair.Key.SellCost;
            }
            totalcost = sellcost - buycost;
            BuyText.text = buycost.ToString();
            SellText.text = sellcost.ToString();
            TotalText.text = totalcost.ToString();
            //ICons Refresh

        }
        private void EventRefresh()
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
        private int _panelRequestId;
        private ReadOnlyCollection<Item> npcInventoryInf;
        private List<ItemCount> npcInventoryNotInf;
        private Dictionary<ItemStack, int> shopOnDict = new Dictionary<ItemStack, int>();
        private Dictionary<ItemStack, int> playerOnDict = new Dictionary<ItemStack, int>();
        private Inventory npcInventory;
        private string shopname;
        private bool initialized = false;
        private PlayerInventory.ItemCategory _currentCategory;
    }
}

