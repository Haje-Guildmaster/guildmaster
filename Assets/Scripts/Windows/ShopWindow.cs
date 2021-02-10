using GuildMaster.Data;
using GuildMaster.Items;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Windows
{
    public class ShopWindow : DraggableWindow, IToggleableWindow
    {
        public void Open()
        {
            base.OpenWindow();
            //Refresh(); 넣으면 안됨. 도대체 왜?
        }
    }
}

