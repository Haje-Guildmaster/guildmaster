using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GuildMaster.Database;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [Serializable]
    public class NpcQuestData
    {
        public bool HasQuests => false;
        [SerializeField] private List<QuestCode> questList = new List<QuestCode>();
        public ReadOnlyCollection<QuestCode> QuestList => questList.AsReadOnly();
    }
}