using UnityEngine;

namespace GuildMaster.Databases
{
    public class DatabaseLoader: MonoBehaviour
    {
        public ItemDatabase itemDatabase;
        public NpcDatabase npcDatabase;
        public QuestDatabase questDatabase;
        public CharacterDatabase characterDatabase;
        public InGameEventDatabase inGameEventDatabase;
        public ExplorationLocationDatabase explorationLocationDatabase;
        public EventSeedDatabase eventSeedDatabase;
        
        private void Awake()
        {
            ItemDatabase.LoadSingleton(itemDatabase);
            NpcDatabase.LoadSingleton(npcDatabase);
            QuestDatabase.LoadSingleton(questDatabase);
            CharacterDatabase.LoadSingleton(characterDatabase);
            InGameEventDatabase.LoadSingleton(inGameEventDatabase);
            ExplorationLocationDatabase.LoadSingleton(explorationLocationDatabase);
            EventSeedDatabase.LoadSingleton(eventSeedDatabase);
        }
    }
}