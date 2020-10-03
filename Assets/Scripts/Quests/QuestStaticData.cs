using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GuildMaster.Data;
using GuildMaster.Dialogs;
using GuildMaster.Npcs;
using GuildMaster.Rewards;
using UnityEngine;

namespace GuildMaster.Quests
{
    [Serializable]
    public class QuestStaticData
    {
        [SerializeField] private string questName;
        [SerializeField] [TextArea] private string questDescription;
        [SerializeReference] [SerializeReferenceButton] private Player.Condition activationCondition;
        [SerializeField] private Script questSuggestScript;
        
        [SerializeField] private List<QuestStep> steps;

        [SerializeReference] [SerializeReferenceButton] private List<Reward> rewards;

        public bool IsAbleToGet = false;
        public string QuestName => questName;

        public string QuestDescription => questDescription;

        public Player.Condition ActivationCondition => activationCondition;
        public Script QuestSuggestScript => questSuggestScript;
        public ReadOnlyCollection<QuestStep> Steps => steps.AsReadOnly();
        public IEnumerable<Reward> Rewards => rewards;
    }
    
}