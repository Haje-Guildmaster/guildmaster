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
    public void Close()
    {
        base.Close();

    }

    public void Open(ItemStack itemStack, bool isbuy)
    {
        this.ItemStack = itemStack;
        this.isbuy = isbuy;
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
        int value = (int) slider.value;
        Num.text = value.ToString();
        if (isbuy)
            gold.text = (value * ItemStack.BuyCost).ToString();
        else
            gold.text = (value * ItemStack.SellCost).ToString();
        slider.value = value;
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
    }
    private bool isbuy;
    private ItemStack ItemStack;
    private Action Changed;
}
