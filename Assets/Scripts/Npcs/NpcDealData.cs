using GuildMaster.Data;
using GuildMaster.Databases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [System.Serializable]
    public class ItemCodeStack
    {
        public ItemCode itemcode;
        public int number;
    }
    [Serializable]
    public class NpcDealData
    {
        public bool HasDeal => false;
        [SerializeField] private List<ItemCode> npcInventoryUnlimit = new List<ItemCode>();
        [SerializeField] private List<ItemCodeStack> npcInventoryLimit = new List<ItemCodeStack>();
        public ReadOnlyCollection<ItemCode> NpcInventoryUnlimit => npcInventoryUnlimit.AsReadOnly();
        public ReadOnlyCollection<ItemCodeStack> NpcInventoryLimit => npcInventoryLimit.AsReadOnly();
    }
}