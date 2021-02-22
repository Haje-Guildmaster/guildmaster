using GuildMaster.Data;
using GuildMaster.Databases;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class ShopWindow : DraggableWindow, IToggleableWindow
    {
        public void Open()
        {
            base.OpenWindow();
        }
        public void Open(ReadOnlyCollection<ItemStack> npcInventoryUnlimited, ReadOnlyCollection<ItemStack> npcInventoryLimited, string shopname)
        {
            this.npcInventoryUnlimited = npcInventoryUnlimited;
            foreach (ItemStack itemstack in npcInventoryLimited)
            {
                this.npcInventoryLimited.Add(new ItemStack(itemstack.Item, itemstack.ItemNum));
            }
            this.shopname = shopname;
            Open();
            Initialize();
        }
        private void Initialize()
        {
            GameObject.Find("Title").GetComponent<Text>().text = shopname;
        }

        private ReadOnlyCollection<ItemStack> npcInventoryUnlimited;
        private List<ItemStack> npcInventoryLimited = new List<ItemStack>();
        private string shopname;
    }
}

