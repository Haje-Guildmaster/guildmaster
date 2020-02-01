using System;
using System.Collections.Generic;
using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Quests
{
    [Serializable]
    public class QuestManager
    {
        private HashSet<QuestData> _completedQuests = new HashSet<QuestData>();
        private readonly PlayerData _playerData;

        public QuestManager(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private List<Quest> _quests;

        public void AddQuest(QuestData questData) => _quests.Add(new Quest(questData));
        public bool AbandonQuest(Quest quest) => _quests.Remove(quest);

        public void UpdateQuests()
        {
            foreach (var quest in _quests)
            {
                if (quest.CanCompleteStep)
                    quest.NextStep();

                if (quest.CanCompleteQuest)
                    // 혹시 모를 상황을 위해 일부로 위의 if (quest.CanCompleteStep)문 안에 넣지 않았습니다.
                    _CompleteQuest(quest);
            }
        }

        public bool CompletedQuest(QuestData questData) => _completedQuests.Contains(questData);

        private void AddProgress<T>(Func<T, bool> filter, int progress) where T : StepMission
        {
            foreach (var quest in _quests)
            {
                if (_playerData.CheckCondition(quest.CurrentStep.StepCondition) 
                    && quest.CurrentStep.StepMission is T tMission
                    && filter(tMission))
                {
                    quest.StepProgress += progress;
                }
            }
        }

        private void _CompleteQuest(Quest quest)
        {
            // Todo: 보상주기, 퀘스트 완료창?이나 메시지같은거.
            Debug.Log("Completed a quest");
            _completedQuests.Add(quest.QuestData);
            AbandonQuest(quest);
        }
    }
}