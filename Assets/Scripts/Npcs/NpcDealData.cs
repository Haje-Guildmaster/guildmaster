using GuildMaster.Data;
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
        [SerializeField] private List<ItemStack> npcInventoryUnlimit = new List<ItemStack>();
        [SerializeField] private List<ItemStack> npcInventoryLimit = new List<ItemStack>();
        public ReadOnlyCollection<ItemStack> NpcInventoryUnlimit => npcInventoryUnlimit.AsReadOnly();
        public ReadOnlyCollection<ItemStack> NpcInventoryLimit => npcInventoryLimit.AsReadOnly();
    }
}