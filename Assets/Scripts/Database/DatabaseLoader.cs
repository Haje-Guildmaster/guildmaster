using System;
using GuildMaster.Items;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Database
{
    public class DatabaseLoader: MonoBehaviour
    {
        public ItemDatabase itemDatabase;
        public NpcDatabase npcDatabase;
        public QuestDatabase questDatabase;
    
        private void Awake()
        {
            ItemDatabase.LoadSingleton(itemDatabase);
            NpcDatabase.LoadSingleton(npcDatabase);
            QuestDatabase.LoadSingleton(questDatabase);
        }
    }
}