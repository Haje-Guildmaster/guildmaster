using System;

namespace GuildMaster.Items
{
    [Serializable]
    public readonly struct EquipmentStats
    {
        public readonly int atk;
    }

    [Serializable]
    public class EquipmentStatsRef
    {
        public EquipmentStatsRef(EquipmentStats equipmentStats)
        {
            this.EquipmentStats = equipmentStats;
        }
        public readonly EquipmentStats EquipmentStats;
    }
}