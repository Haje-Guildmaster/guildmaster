using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    public sealed class EquipmentStats
    {
        [SerializeField] private int atk;
        public int ATK => atk;
        
        
        private bool Equals(EquipmentStats other)
        {
            return atk == other.atk;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is EquipmentStats other && Equals(other);
        }

        public override int GetHashCode()
        {
            return atk;
        }
    }
}