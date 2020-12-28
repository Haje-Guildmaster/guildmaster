using System;
using System.Collections.Generic;
using GuildMaster.Data;
using GuildMaster.Dialogs;
using GuildMaster.Npcs;
using UnityEngine;

namespace GuildMaster.Quests
{
    [Serializable]
    public class QuestStep
    {
        [SerializeReference][SerializeReferenceButton] private Player.Condition stepCondition;
        public Player.Condition StepCondition => stepCondition;
        [SerializeReference] [SerializeReferenceButton] private List<StepMission> _stepMissions;
        public List<StepMission> StepMissions => _stepMissions;
    }
}