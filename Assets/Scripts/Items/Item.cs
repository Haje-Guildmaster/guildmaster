using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    public sealed class Item
    {
        [SerializeReference][SerializeReferenceButton] private EquipmentStatsRef equipmentStatsRef;
        public ItemCode code;
        
        public bool EquipAble => equipmentStatsRef != null;
        public EquipmentStats EquipmentStats => equipmentStatsRef.EquipmentStats;
        
        
        public enum ItemCode
        {
            TestItem1, TestItem2, TestItem3
        }
    }
}