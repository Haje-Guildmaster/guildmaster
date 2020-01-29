using System;
using System.Collections.Generic;
using GuildMaster.Conditions;
using GuildMaster.Npcs;
using UnityEngine;

namespace GuildMaster.Quests
{
    using Script = String;
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/QuestData", order = 0)]
    public class QuestData: ScriptableObject
    {
        [SerializeField] private Condition activationCondition;
        [SerializeField] private Script questReceiveScript;
        [SerializeField] private List<Step> steps;
        
        public Condition ActivationCondition => activationCondition;
        public Script QuestReceiveScript => questReceiveScript;
        public List<Step> Steps => steps;

        [Serializable]
        public class Step
        {
            public Condition completeCondition;
            public NpcData reportTo;
            public Script reportScript;
        }
        
    }
}