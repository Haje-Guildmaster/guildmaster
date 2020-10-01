using UnityEngine;

namespace GuildMaster.Databases
{
    /// <summary>
    /// ScriptableObject인 데이터베이스들을 선택해서 싱글톤으로 올리는 역할을 하는 유니티 오브젝트입니다.
    /// </summary>
    public class DatabaseLoader: MonoBehaviour
    {
        public ItemDatabase itemDatabase;
        public NpcDatabase npcDatabase;
        public QuestDatabase questDatabase;
        public CharacterDatabase characterDatabase;
        public TraitDatabase traitDatabase;
        public InGameEventDatabase inGameEventDatabase;
        public ExplorationLocationDatabase explorationLocationDatabase;
        public EventSeedDatabase eventSeedDatabase;
        
        private void Awake()
        {
            ItemDatabase.LoadSingleton(itemDatabase);
            NpcDatabase.LoadSingleton(npcDatabase);
            QuestDatabase.LoadSingleton(questDatabase);
            CharacterDatabase.LoadSingleton(characterDatabase);
            TraitDatabase.LoadSingleton(traitDatabase);
            InGameEventDatabase.LoadSingleton(inGameEventDatabase);
            ExplorationLocationDatabase.LoadSingleton(explorationLocationDatabase);
            EventSeedDatabase.LoadSingleton(eventSeedDatabase);
        }
    }
}