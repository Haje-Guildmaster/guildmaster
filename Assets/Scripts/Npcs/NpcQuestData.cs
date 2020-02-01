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
        [SerializeReference] private List<QuestData> questList;
        public ReadOnlyCollection<QuestData> QuestList => questList.AsReadOnly();
    }
}