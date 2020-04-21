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
        

        public bool ReceiveQuest(QuestCode questCode, NpcCode client)
        {
            if (!CanReceiveQuest(questCode)) return false;
            _quests.Add(new Quest(questCode, client));
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

        public bool CompletedQuest(QuestCode questData) => _completedQuests.Contains(questData);
        public bool DoingQuest(QuestCode questData) => _quests.Count(q => q.QuestCode == questData) > 0;
        public List<ReadOnlyQuest> CurrentQuests() 
            => _quests.Select(q => new ReadOnlyQuest(q)).ToList();

        public List<StepMission.TalkMission> GetCompletableTalkMissions(NpcCode npc)
        {
            return _quests
                .Where(q => _playerData.CheckCondition(q.CurrentStep.StepCondition))
                .SelectMany(q => q.DoingMissions)
                .Where(mp => (mp.progress<mp.mission.MaxProgress))
                .Select(mp => mp.mission)
                .OfType<StepMission.TalkMission>()
                .Where(tm=> tm.talkTo == npc)
                .ToList();
        }
        public List<QuestCode> GetAvailableQuestsFrom(IEnumerable<QuestCode> quests) => quests.Where(CanReceiveQuest).ToList();

        // Event Listeners
        private void OnQuestScriptPlayEnd(StepMission.TalkMission mission)
        {
            AddProgress<StepMission.TalkMission>(s=>s==mission, 1);
        }


        private bool CanReceiveQuest(QuestCode q) 
            =>_playerData.CheckCondition(QuestDatabase.Get(q).ActivationCondition) && !CompletedQuest(q) && !DoingQuest(q);


        private readonly List<Quest> _quests = new List<Quest>();
        private readonly HashSet<QuestCode> _completedQuests = new HashSet<QuestCode>();

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

        private int AddProgress<T>(Func<T, bool> filter, int addingProgress) where T : StepMission
        {
            var cnt = 0;

            foreach (var quest in _quests.Where(quest=>_playerData.CheckCondition(quest.CurrentStep.StepCondition)))
            {
                var mps = quest.DoingMissions;
                for (var i = 0; i < mps.Length; i++)
                {
                    if (!(mps[i].mission is T tMission && filter(tMission))) continue;
                    cnt++;
                    quest.SetProgress(i, mps[i].progress + addingProgress);
                }
            }
            Debug.Log($"Added {addingProgress} progress to {cnt} missions");
            UpdateQuests();
            Changed?.Invoke();
            return cnt;
        }

        private void _CompleteQuest(Quest quest)
        {
            var questStaticData = QuestDatabase.Get(quest.QuestCode);
            // 보상주기
            foreach (var reward in questStaticData.Rewards)
                _playerData.ApplyReward(reward);

            Debug.Log($"Completed a quest: {questStaticData.QuestName}");
            _completedQuests.Add(quest.QuestCode);
            AbandonQuest(quest);
            Changed?.Invoke();
        }
    }
}