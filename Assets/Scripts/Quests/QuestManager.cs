using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private void _Changed() => GameEvents.QuestManagerDataChange.Invoke();
        
        private readonly PlayerData _playerData;
        
        public QuestManager(PlayerData playerData)
        {
            _playerData = playerData;
            GameEvents.QuestScriptPlayEnd.AddListener(OnQuestScriptPlayEnd);
        }
        

        public bool ReceiveQuest(QuestData questData, NpcData client)
        {
            if (!CanReceiveQuest(questData)) return false;
            var made = new Quest(questData, client);
            made.Changed += _Changed;
            _quests.Add(made);
            _Changed();
            return true;
        }
        public bool AbandonQuest(Quest quest)
        {
            var ret = _quests.Remove(quest);
            _Changed();
            return ret;
        }

        public bool CompletedQuest(QuestData questData) => _completedQuests.Contains(questData);
        public bool DoingQuest(QuestData questData) => _quests.Count(q => q.QuestData == questData) > 0;
        public ReadOnlyCollection<Quest> CurrentQuests() => _quests.AsReadOnly();

        public List<StepMission.TalkMission> GetCompletableTalkMissions(NpcData npcData)
        {
            return _quests.Select(q => q.CurrentStep)
                .Where(step => _playerData.CheckCondition(step.StepCondition))
                .Select(q => q.StepMission)
                .OfType<StepMission.TalkMission>()
                .Where(tm=>tm.talkTo==npcData)
                .ToList();
        }
        public List<QuestData> GetAvailableQuestsFrom(IEnumerable<QuestData> quests) => quests.Where(CanReceiveQuest).ToList();

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
            // 보상주기
            foreach (var reward in quest.QuestData.Rewards)
                _playerData.ApplyReward(reward);

            Debug.Log($"Completed a quest: {quest.QuestData.QuestName}");
            _completedQuests.Add(quest.QuestData);
            AbandonQuest(quest);
            _Changed();
        }
    }
}