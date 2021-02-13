using System;
using UnityEngine;

namespace GuildMaster.Items
{
    [Serializable]
    public class ItemStaticData
    {
        [SerializeField] private string itemName;
        [SerializeField][TextArea] private string itemDescription;
        [SerializeField] private bool isConsumable;
        [SerializeReference][SerializeReferenceButton] private ItemEffect consumptionEffect;
        [SerializeField] private int maxStack;
        [SerializeField] private int buyPrice;
        [SerializeField] private int sellPrice;
        [SerializeField] private Sprite itemImage;            //Todo: Assetbundle(또는 Resource) path로 대체.
        [SerializeField] private bool isEquipable;
        [SerializeReference][SerializeReferenceButton] private EquipmentStats defaultEquipmentStats;
        [SerializeField] private bool isImportant;
        
        public ItemEffect ConsumptionEffect => consumptionEffect;
        public int MaxStack => maxStack;
        public int BuyPrice => buyPrice;
        public int SellPrice => sellPrice;
        public Sprite ItemImage => itemImage;
        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public bool IsConsumable => isConsumable;
        public bool IsImportant => isImportant;

        public bool IsEquipable => isEquipable;
        public EquipmentStats DefaultEquipmentStats => defaultEquipmentStats;
    }
}