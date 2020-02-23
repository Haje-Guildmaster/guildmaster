using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    public struct EquipmentStats
    {
        public int atk;
    }
    
    [Serializable]
    public class EquipmentStatsRef
    {
        [SerializeField] private EquipmentStats equipmentStats;

        public EquipmentStats EquipmentStats => equipmentStats;
    }
}