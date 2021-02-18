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
        public void Open(ReadOnlyCollection<ItemCode> npcInventoryUnlimited, ReadOnlyCollection<ItemCodeStack> npcInventoryLimited, string shopname)
        {
            this.npcInventoryUnlimited = npcInventoryUnlimited;
            foreach (ItemCodeStack itemCodeStack in npcInventoryLimited)
            {
                this.npcInventoryLimited.Add(new ItemCodeStack(itemCodeStack.itemcode, itemCodeStack.number));
            }
            this.shopname = shopname;
            Open();
            Initialize();
        }
        private void Initialize()
        {
            GameObject.Find("Title").GetComponent<Text>().text = shopname;
        }

        private ReadOnlyCollection<ItemCode> npcInventoryUnlimited;
        private List<ItemCodeStack> npcInventoryLimited = new List<ItemCodeStack>();
        private string shopname;
    }
}

