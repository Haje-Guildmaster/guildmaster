using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Events;
using GuildMaster.Npcs;
using UnityEngine;

namespace GuildMaster.Quests
{
    [Serializable]
    public class QuestManager
    {
        public QuestManager(PlayerData playerData)
        {
            _playerData = playerData;
            GameEvents.QuestScriptPlayEnd.AddListener(OnQuestScriptPlayEnd);
        }
        
        private readonly PlayerData _playerData;

        public bool ReceiveQuest(QuestData questData)
        {
            if (!CanReceiveQuest(questData)) return false;
            _quests.Add(new Quest(questData));
            return true;
        }
        public bool AbandonQuest(Quest quest) => _quests.Remove(quest);
        public bool CompletedQuest(QuestData questData) => _completedQuests.Contains(questData);
        public bool DoingQuest(QuestData questData) => _quests.Count(q => q.QuestData == questData) > 0;
        

        public List<StepMission.TalkMission> GetCompletableTalkMissions(NpcData npcData)
        {
            return _quests.Select(q => q.CurrentStep)
                .Where(step => _playerData.CheckCondition(step.StepCondition))
                .Select(q => q.StepMission)
                .OfType<StepMission.TalkMission>()
                .Where(tm=>tm.talkTo==npcData)
                .ToList();
        }
        public List<QuestData> WhereAvailable(IEnumerable<QuestData> quests) => quests.Where(CanReceiveQuest).ToList();

            // Event Listeners
        private void OnQuestScriptPlayEnd(StepMission.TalkMission mission)
        {
            AddProgress<StepMission.TalkMission>(s=>s==mission, 1);
        }


        private bool CanReceiveQuest(QuestData q) 
            =>_playerData.CheckCondition(q.ActivationCondition) && !CompletedQuest(q) && !DoingQuest(q);


        private readonly List<Quest> _quests = new List<Quest>();
        private readonly HashSet<QuestData> _completedQuests = new HashSet<QuestData>();

        private void UpdateQuests()
        {
            var completeQuestQueue = new List<Quest>();
            foreach (var quest in _quests)
            {
                if (quest.CanCompleteStep)
                    quest.NextStep();

                if (quest.CanCompleteQuest)
                    // 혹시 모를 상황을 위해 일부로 위의 if (quest.CanCompleteStep)문 안에 넣지 않았습니다.
                    completeQuestQueue.Add(quest);
            }
            
            completeQuestQueue.ForEach(_CompleteQuest);
        }

        private int AddProgress<T>(Func<T, bool> filter, int progress) where T : StepMission
        {
            var cnt = 0;
            foreach (var quest in _quests)
            {
                if (_playerData.CheckCondition(quest.CurrentStep.StepCondition) 
                    && quest.CurrentStep.StepMission is T tMission
                    && filter(tMission))
                {
                    quest.StepProgress += progress;
                    cnt++;
                }
            }
            Debug.Log($"Added {progress} progress to {cnt} quests");
            UpdateQuests();
            return cnt;
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