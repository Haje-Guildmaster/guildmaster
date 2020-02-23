using UnityEngine;

namespace GuildMaster.Items
{
    public class ItemDatabaseLoader: MonoBehaviour
    {
        public ItemDatabase itemDatabase;
        private void Awake()
        {
            itemDatabase.SetAsSingleton();
            Destroy(this);
        }
    }
}