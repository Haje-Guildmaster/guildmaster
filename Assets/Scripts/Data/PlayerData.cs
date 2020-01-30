using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Conditions;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Data
{
    // 게임을 플레이하는 사람의 모든 정보, 즉 세이브를 했을 때 저장되는 모든 데이터를 담습니다.
    // 퀘스트 클리어 정보, 길드원들, 레벨, 장비, etc...
    [Serializable]
    public class PlayerData
    {
        private HashSet<QuestData> _completedQuests = new HashSet<QuestData>();
        private HashSet<Quest> _currentQuests = new HashSet<Quest>();
        private int _level = 1;

        public bool CheckCondition(Condition condition)
        {
            switch (condition)
            {
                case Condition.Always always:
                    return always.isTrue;
                case Condition.And and:
                    return and.conditions.Aggregate(true, (calc, cond) => calc && CheckCondition(cond));
                case Condition.Or or:
                    return or.conditions.Aggregate(false, (calc, cond) => calc || CheckCondition(cond));
                case Condition.CompletedQuest hasCompletedQuest:
                    return _completedQuests.Contains(hasCompletedQuest.quest);
                case Condition.LevelOver levelOver:
                    return levelOver.level > _level;
                default:
                    throw new Exception($"Unexpected Condition: {condition}");
            }
        }

        private static PlayerData _instance;

        public static PlayerData Instance
        {
            get { return _instance = _instance ?? new PlayerData(); }
        }
    }
}