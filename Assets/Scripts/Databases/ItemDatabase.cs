using System;
using GuildMaster.Items;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 0)]
    public class ItemDatabase : UnityEditableDatabase<ItemDatabase, ItemStaticData, ItemCode>
    {}

    // 유니티 serialization을 위해.
    [Serializable]
    public class ItemCode : ItemDatabase.Index {}
    
    [CustomPropertyDrawer(typeof(ItemCode))]
    public class ItemCodeDrawer : DatabaseIndexDrawer<ItemDatabase, ItemStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, ItemStaticData element) =>
            $"Item {i}: {element.ItemName}";
    }
    
}