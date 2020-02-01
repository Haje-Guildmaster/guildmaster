using System;
using GuildMaster.Conditions;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using UnityEngine;

namespace GuildMaster.Quests
{
    [Serializable]
    public class QuestStep
    {
        [SerializeReference][SerializeReferenceButton] private Condition stepCondition;
        public Condition StepCondition => stepCondition;
        [SerializeReference] [SerializeReferenceButton] private StepMission _stepMission;
        public StepMission StepMission => _stepMission;
    }
}