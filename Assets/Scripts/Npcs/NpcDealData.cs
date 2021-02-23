using GuildMaster.Data;
using GuildMaster.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GuildMaster.Npcs
{ 
    [Serializable]
    public class NpcDealData
    {
        public bool HasDeal => npcInventoryLimit.Count > 0 || npcInventoryUnlimit.Count > 0;
        [SerializeField] public string shopname;
        [SerializeField] private List<Item> npcInventoryUnlimit = new List<Item>();
        [SerializeField] private List<ItemCount> npcInventoryLimit = new List<ItemCount>();
        public ReadOnlyCollection<Item> NpcInventoryUnlimit => npcInventoryUnlimit.AsReadOnly();
        public ReadOnlyCollection<ItemCount> NpcInventoryLimit => npcInventoryLimit.AsReadOnly();
    }
}