using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GuildMaster.Data;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using GuildMaster.Rewards;
using UnityEngine;

namespace GuildMaster.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/QuestData", order = 0)]
    public class QuestData: ScriptableObject
    {
        [SerializeField] private int questDataId;
        [SerializeField] private string questName;
        [SerializeField] [TextArea] private string questDescription;
        [SerializeReference] [SerializeReferenceButton] private Condition activationCondition;
        [SerializeField] private Script questSuggestScript;
        
        [SerializeField] private List<QuestStep> steps;

        [SerializeReference] [SerializeReferenceButton] private List<Reward> rewards;
        

        public string QuestName => questName;

        public string QuestDescription => questDescription;

        public Condition ActivationCondition => activationCondition;
        public Script QuestSuggestScript => questSuggestScript;
        public ReadOnlyCollection<QuestStep> Steps => steps.AsReadOnly();
        public IEnumerable<Reward> Rewards => rewards;
    }
    
}