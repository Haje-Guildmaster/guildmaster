using UnityEngine;

namespace GuildMaster.Databases
{
    public class DatabaseLoader: MonoBehaviour
    {
        public ItemDatabase itemDatabase;
        public NpcDatabase npcDatabase;
        public QuestDatabase questDatabase;
        public CharacterDatabase characterDatabase;
    
        private void Awake()
        {
            ItemDatabase.LoadSingleton(itemDatabase);
            NpcDatabase.LoadSingleton(npcDatabase);
            QuestDatabase.LoadSingleton(questDatabase);
            CharacterDatabase.LoadSingleton(characterDatabase);
        }
    }
}