using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Npcs;
using GuildMaster.UI;
using UnityEngine;

namespace GuildMaster.Quests
{

    [Serializable]
    public class QuestManager
    {
        public event Action Changed;
        
        private readonly PlayerData _playerData;
        
        public QuestManager(PlayerData playerData)
        {
            _playerData = playerData;
            NpcInteractWindow.QuestScriptPlayEnd += OnQuestScriptPlayEnd;
        }
        

        public bool ReceiveQuest(QuestData questData, NpcData client)
        {
            if (!CanReceiveQuest(questData)) return false;
            _quests.Add(new Quest(questData, client));
            Changed?.Invoke();
            return true;
        }
        public bool AbandonQuest(ReadOnlyQuest roq)
        {
            var found = _quests.FirstOrDefault(q => q.QuestId == roq.QuestId);
            return found != null && AbandonQuest(found);
        }
        private bool AbandonQuest(Quest quest)
        {
            var ret = _quests.Remove(quest);
            if (ret)
                Changed?.Invoke();
            return ret;
        }

        public bool CompletedQuest(QuestData questData) => _completedQuests.Contains(questData);
        public bool DoingQuest(QuestData questData) => _quests.Count(q => q.QuestData == questData) > 0;
        public List<ReadOnlyQuest> CurrentQuests() 
            => _quests.Select(q => new ReadOnlyQuest(q)).ToList();

        public List<StepMission.TalkMission> GetCompletableTalkMissions(NpcData npcData)
        {
            return _quests
                .Where(q => _playerData.CheckCondition(q.CurrentStep.StepCondition))
                .SelectMany(q => q.DoingMissions)
                .Where(mp => (!mp.Completed) && mp.Mission is StepMission.TalkMission tm && tm.talkTo == npcData)
                .Select(mp => mp.Mission)
                .OfType<StepMission.TalkMission>().ToList();
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
            var flag = false;
            foreach (var quest in _quests)
            {
                if (quest.CanCompleteStep)
                {
                    quest.NextStep();
                    flag = true;
                }

                if (quest.CanCompleteQuest)
                    // 혹시 모를 상황을 위해 일부로 위의 if (quest.CanCompleteStep)문 안에 넣지 않았습니다.
                    completeQuestQueue.Add(quest);
            }
            
            if (flag) Changed?.Invoke();
            completeQuestQueue.ForEach(_CompleteQuest);
        }

        private int AddProgress<T>(Func<T, bool> filter, int progress) where T : StepMission
        {
            var cnt = 0;
            
            foreach (var missionProgress in _quests
                .Where(quest=>_playerData.CheckCondition(quest.CurrentStep.StepCondition))
                .SelectMany(quest=>quest.DoingMissions))
            {
                if (!(missionProgress.Mission is T tMission && filter(tMission))) continue;
                cnt++;
                missionProgress.Progress += progress;
            }
            Debug.Log($"Added {progress} progress to {cnt} missions");
            UpdateQuests();
            Changed?.Invoke();
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
            Changed?.Invoke();
        }
    }
}