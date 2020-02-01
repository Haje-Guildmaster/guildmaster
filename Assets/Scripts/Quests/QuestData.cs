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
        [SerializeReference] [SerializeReferenceButton] private Condition activationCondition;
        [SerializeField] private Script questReceiveScript;
        [SerializeField] private List<QuestStep> steps;
        
        public Condition ActivationCondition => activationCondition;
        public Script QuestReceiveScript => questReceiveScript;
        public IEnumerable<QuestStep> Steps => steps;
    }
    
}