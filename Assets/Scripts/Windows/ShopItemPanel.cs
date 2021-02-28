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
        itemStack.Quantity = quantity;
        UiWindowsManager.Instance.shopWindow.GetPanelInfo(itemStack, isbuy, index);
        Close();
    }
    public void Open(ItemStack itemStack, bool isbuy, int index)
    {
        this.itemStack = itemStack;
        this.isbuy = isbuy;
        this.index = index;
        this.quantity = itemStack.Quantity;
        shopItemIcon.UpdateAppearance(itemStack, 0, isbuy);
        Open();
        if (itemStack.isInfinite)
            slider.maxValue = 1000;
        else
            slider.maxValue = itemStack.ItemNum;
        slider.value = quantity;
    }
    private void Refresh()
    {
        int value = (int) Math.Round(slider.value);
        slider.value = value;
        quantity = value;
        Num.text = value.ToString();
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
            Refresh();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            slider.value -= 1;
            Refresh();
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
