using System;
using System.Collections.Generic;
using GuildMaster.Dialog;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    public class ItemDatabase: ScriptableObject
    {
        public ItemDatabase Instance { get; private set;}

        private void Awake()
        {
            if (Instance!=null) throw new Exception();
            Instance = this;
        }
        
        public ItemStaticData GetItemStaticData(Item.ItemCode itemCode)
        {
            return _itemStaticDataMap[(int) itemCode];
        }

        private List<ItemStaticData> _itemStaticDataMap = new List<ItemStaticData>();
    }
}