using System;
using GuildMaster.Items;
using GuildMaster.Npcs;
using UnityEngine;

namespace GuildMaster.Database
{
    public class DatabaseLoader: MonoBehaviour
    {
        public ItemDatabase itemDatabase;
        public NpcDatabase npcDatabase;

        private void Awake()
        {
            ItemDatabase.LoadSingleton(itemDatabase);
            NpcDatabase.LoadSingleton(npcDatabase);
        }
    }
}