using System;
using System.Linq;
using GuildMaster.Database;
using UnityEngine;

namespace GuildMaster.Items
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 0)]
    public class ItemDatabase : EditableDatabase<ItemDatabaseIndex, ItemStaticData>
    {
        private static ItemDatabase _instance = null;
        public static IDatabase<ItemDatabaseIndex, ItemStaticData> Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                return _instance = Resources.FindObjectsOfTypeAll<ItemDatabase>().FirstOrDefault();
            }
        }
    }

    [Serializable]
    public class ItemDatabaseIndex : DatabaseIndex {}
}