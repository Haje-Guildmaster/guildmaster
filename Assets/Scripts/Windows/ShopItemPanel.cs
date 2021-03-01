using GuildMaster.Data;
using GuildMaster.Windows;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : DraggableWindow, IToggleableWindow
{
    [SerializeField] private ShopItemIcon shopItemIcon;
    [SerializeField] private Text gold;
    [SerializeField] private Text Num;
    [SerializeField] private Slider slider;

    public void Open()
    {
        base.OpenWindow();
    }
    public void SendToShopWindow()
    {
        UiWindowsManager.Instance.shopWindow.GetPanelInfo(itemStack, isbuy, index);
        Close();
    }
    public void Open(ItemStack itemStack, bool isbuy, int index)
    {
        if (itemStack == null) 
            Close();
        this.itemStack = itemStack;
        this.isbuy = isbuy;
        this.index = index;
        this.quantity = itemStack.Quantity;
        Open();
        if (itemStack.isInfinite)
        {
            slider.maxValue = 100;
            shopItemIcon.UpdateAppearance(new ItemStack(itemStack.Item, true), 0, isbuy);
        }
        else
        {
            slider.maxValue = itemStack.ItemNum;
            shopItemIcon.UpdateAppearance(new ItemStack(itemStack.Item, itemStack.ItemNum), 0, isbuy);
        }
        slider.value = quantity;
    }
    private void Refresh()
    {
        int value = (int) Math.Round(slider.value);
        slider.value = value;
        quantity = value;
        Num.text = value.ToString();
        itemStack.Quantity = value;
        if (isbuy)
            gold.text = (value * itemStack.BuyCost).ToString();
        else
            gold.text = (value * itemStack.SellCost).ToString();
    }
    private void Awake()
    {
        slider.onValueChanged.AddListener(delegate {
            Refresh();
        });
    }
    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            slider.value += 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            slider.value -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendToShopWindow();
            Close();
        }
    }
    private bool isbuy;
    private int quantity;
    private int index;
    private ItemStack itemStack;
}
