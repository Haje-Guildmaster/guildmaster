using System;
using System.Linq;
using GuildMaster.Database;
using UnityEngine;

namespace GuildMaster.Items
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 0)]
    public class ItemDatabase : UnityEditableDatabase<ItemDatabase, ItemStaticData, ItemCode>
    {}

    // 유니티 serialization을 위해.
    [Serializable]
    public class ItemCode : ItemDatabase.Index {}
}