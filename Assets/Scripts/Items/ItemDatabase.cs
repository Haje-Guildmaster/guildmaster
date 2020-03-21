using System.Linq;
using GuildMaster.Database;
using UnityEngine;

namespace GuildMaster.Items
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase", order = 0)]
    public class ItemDatabase : Database<Item.ItemCode, ItemStaticData>
    {
        private static ItemDatabase _instance = null;
        public static IDatabase<Item.ItemCode, ItemStaticData> Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                return _instance = Resources.FindObjectsOfTypeAll<ItemDatabase>().FirstOrDefault();
            }
        }
    }
}