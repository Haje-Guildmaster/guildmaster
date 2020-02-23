using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    public sealed class Item
    {
        [SerializeReference][SerializeReferenceButton] private EquipmentStatsRef equipmentStatsRef;
        [SerializeField] private ItemCode code;
        
        
        public bool EquipAble => equipmentStatsRef != null;
        public EquipmentStats EquipmentStats => equipmentStatsRef.EquipmentStats;
        public ItemCode Code => code;
        
        public enum ItemCode
        {
            TestItem1, TestItem2, TestItem3
        }

        private bool Equals(Item other)
        {
            if (code != other.code) return false;
            if (equipmentStatsRef == null)
                return other.equipmentStatsRef == null;
            return (other.equipmentStatsRef != null) && EquipmentStats.Equals(other.EquipmentStats);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Item other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((equipmentStatsRef != null ? equipmentStatsRef.EquipmentStats.GetHashCode() : 0) * 397) ^ (int) code;
            }
        }
    }
}