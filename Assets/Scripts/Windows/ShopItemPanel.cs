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
        UiWindowsManager.Instance.shopWindow.GetPanelInfo(itemStack, num, isbuy, index);
        Close();
    }
    public void Open(ItemStack itemStack, bool isbuy, int index)
    {
        this.itemStack = itemStack;
        this.isbuy = isbuy;
        this.index = index;
        if (isbuy)
            shopItemIcon.UpdateAppearance(itemStack.Item, itemStack.ItemNum, 0, itemStack.BuyCost, 0, itemStack.isInfinite);
        else
            shopItemIcon.UpdateAppearance(itemStack.Item, itemStack.ItemNum, 0, itemStack.SellCost, 0, itemStack.isInfinite);
        Open();
        if (itemStack.isInfinite)
            slider.maxValue = 1000;
        else
            slider.maxValue = itemStack.ItemNum;
        slider.value = 0;
    }
    private void Refresh()
    {
        int value = (int) Math.Round(slider.value);
        slider.value = value;
        num = value;
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
    private int num;
    private int index;
    private ItemStack itemStack;
}
