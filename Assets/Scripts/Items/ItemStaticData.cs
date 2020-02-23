using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    // struct로 하지 않는 이유는 차피 effect의 내부 변수를 수정함으로서 원본을 수정할 수 있으므로.
    public class ItemStaticData
    {
        [SerializeField] private string itemName;
        [SerializeField] private bool isConsumable;
        [SerializeReference][SerializeReferenceButton] private ItemEffect consumptionEffect;
        [SerializeField] private int maxStack;
        [SerializeField] private Sprite itemImage;
        [SerializeField] private bool isEquipable;
        [SerializeReference][SerializeReferenceButton] private EquipmentStatsRef defaultEquipmentStatsRef;
        
        public ItemEffect ConsumptionEffect => consumptionEffect;
        public int MaxStack => maxStack;
        public Sprite ItemImage => itemImage;
        public string ItemName => itemName;
        public bool IsConsumable => isConsumable;

        public bool IsEquipable => isEquipable;
        public EquipmentStats DefaultEquipmentStats => defaultEquipmentStatsRef.EquipmentStats;
    }
}