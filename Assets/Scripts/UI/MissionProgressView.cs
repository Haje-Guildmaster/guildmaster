using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuildMaster.Data;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class MissionProgressView : MonoBehaviour
    {
        public Text descriptionText;
        public Text progressText;

        public void SetQuestStep(ReadOnlyQuest quest, int stepIndex)
        {
            _data = (quest, stepIndex);
            Refresh();
        }

        private void OnEnable()
        {
            PlayerData.Instance.QuestManager.Changed += Refresh;
        }

        private void OnDisable()
        {
            PlayerData.Instance.QuestManager.Changed -= Refresh;
        }

        private void Refresh()
        {
            if (_data.quest == null) return;
            var questCode = _data.quest.QuestCode;
            var questStaticData = QuestDatabase.Get(questCode);
            var step = questStaticData.Steps[_data.stepIndex];
            var completed = _data.quest.StepIndex > _data.stepIndex;
            var isCurrent = _data.quest.StepIndex == _data.stepIndex;

            descriptionText.text = string.Join("\n", step.StepMissions.Select((mission, i) =>
                GetMissionDescription(mission,
                    completed ? mission.MaxProgress : (isCurrent ? _data.quest.DoingMissions[i].progress : 0))));
            progressText.text = completed ? "[  ✓  ]" : "[       ]";
        }

        private (ReadOnlyQuest quest, int stepIndex) _data;

        private static string GetMissionDescription(StepMission mission, int progress)
        {
            string ret;
            switch (mission)
            {
                case StepMission.TalkMission talkMission:
                    ret = $"{NpcDatabase.Get(talkMission.talkTo).basicData.npcName}와 대화.";
                    break;
                default:
                    ret = "알 수 없는 미션.";
                    break;
            }

            ret += $" ({progress}/{mission.MaxProgress})";
            return ret;
        }
    }
}