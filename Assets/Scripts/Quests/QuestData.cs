using System;
using System.Collections.Generic;
using GuildMaster.Conditions;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using UnityEngine;

namespace GuildMaster.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/QuestData", order = 0)]
    public class QuestData: ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string questDescription;
        [SerializeReference] [SerializeReferenceButton] private Condition activationCondition;
        [SerializeField] private Script questSuggestScript;
        
        [SerializeField] private List<QuestStep> steps;
        

        public string QuestName => questName;

        public string QuestDescription => questDescription;

        public Condition ActivationCondition => activationCondition;
        public Script QuestSuggestScript => questSuggestScript;
        public List<QuestStep> Steps => steps;
    }
    
}