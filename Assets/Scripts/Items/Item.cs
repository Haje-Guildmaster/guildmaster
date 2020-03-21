using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    // Immutable...이길 바랍니다. (readonly를 못쓰다보니..)
    public sealed class Item
    {
        [SerializeReference][SerializeReferenceButton] private EquipmentStats equipmentStats;
        [SerializeField] private ItemDatabaseIndex code;
        
        
        public bool EquipAble => equipmentStats != null;
        public EquipmentStats EquipmentStats => equipmentStats;
        public ItemDatabaseIndex Code => code;

        private bool Equals(Item other)
        {
            if (code != other.code) return false;
            if (equipmentStats == null)
                return other.equipmentStats == null;
            return (other.equipmentStats != null) && EquipmentStats.Equals(other.EquipmentStats);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Item other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((equipmentStats != null ? equipmentStats.GetHashCode() : 0) * 397) ^ code.Value;
            }
        }
    }
}