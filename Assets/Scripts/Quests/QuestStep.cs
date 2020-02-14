using System;
using System.Collections.Generic;
using GuildMaster.Data;
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
        [SerializeReference] [SerializeReferenceButton] private List<StepMission> _stepMissions;
        public List<StepMission> StepMissions => _stepMissions;
    }
}