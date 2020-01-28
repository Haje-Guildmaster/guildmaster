using System;
using System.Collections.Generic;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Data
{
    // 게임을 플레이하는 사람의 모든 정보, 즉 세이브를 했을 때 저장되는 모든 데이터를 담습니다.
    // 퀘스트 클리어 정보, 길드원들, 레벨, 장비, etc...
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private HashSet<QuestData> completedQuests;
        [SerializeField] private HashSet<QuestData> currentQuests;
    }
}