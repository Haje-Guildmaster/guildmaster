using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [Serializable]
    public class NpcQuestData
    {
        public bool HasQuests => false;
        [SerializeField] private List<QuestData> questList = new List<QuestData>();
        public ReadOnlyCollection<QuestData> QuestList => questList.AsReadOnly();
    }
}