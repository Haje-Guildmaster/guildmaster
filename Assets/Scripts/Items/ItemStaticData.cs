using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    // struct로 하지 않는 이유는 차피 effect의 내부 변수를 수정함으로서 원본을 수정할 수 있으므로.
    public class ItemStaticData
    {
        [SerializeReference][SerializeReferenceButton] private ItemEffect usageEffect;
        [SerializeField] private int maxStack;
        [SerializeField] private Sprite itemImage;
        [SerializeReference][SerializeReferenceButton] private EquipmentStatsRef defaultEquipmentStats;
        
        public ItemEffect UsageEffect => usageEffect;
        public int MaxStack => maxStack;
        public Sprite ItemImage => itemImage;
    }
}