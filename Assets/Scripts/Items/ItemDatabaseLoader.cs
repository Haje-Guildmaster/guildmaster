using UnityEngine;

namespace GuildMaster.Items
{
    public class ItemDatabaseLoader: MonoBehaviour
    {
        public static ItemDatabase Loaded;

        [SerializeField] private ItemDatabase itemDatabase;
        private void Start()
        {
            Loaded = itemDatabase;
        }
    }
}