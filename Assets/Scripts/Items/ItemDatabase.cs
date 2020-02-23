using System;
using System.Collections.Generic;
using GuildMaster.Dialog;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Items
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 0)]
    public class ItemDatabase: ScriptableObject
    {
        public static ItemDatabase Instance { get; private set;}


        
        public ItemStaticData GetItemStaticData(Item.ItemCode itemCode)
        {
            return itemStaticDataMap[(int) itemCode];
        }

        public void SetAsSingleton()
        {
            if (Instance != null) throw new Exception();
            Instance = this;
        }
        
        [SerializeField] private List<ItemStaticData> itemStaticDataMap = new List<ItemStaticData>();
    }
}