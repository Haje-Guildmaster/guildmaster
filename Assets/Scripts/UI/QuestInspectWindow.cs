using System;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Database;
using GuildMaster.Items;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.Rewards;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class QuestInspectWindow: DraggableWindow
    {
        // Todo: 완료시 포기버튼 숨기기.
        public Text questNameText;
        public Text questDescriptionText;
        public Text clientNameText;
        public Text rewardTextPrefab;
        public Transform rewardTextListParent;
        public MissionProgressView missionProgressViewPrefab;
        public Transform missionProgressListParent;

        private ReadOnlyQuest _quest;
        

        public void AbandonCurrentQuest()
        {
            PlayerData.Instance.QuestManager.AbandonQuest(_quest);
            Close();
        }

        public void Open(ReadOnlyQuest quest)
        {
            base.OpenWindow();
            _quest = quest;
            var questStaticData = QuestDatabase.Get(_quest.QuestCode);
            questNameText.text = questStaticData.QuestName;
            questDescriptionText.text = questStaticData.QuestDescription;
            clientNameText.text = NpcDatabase.Get(_quest.Client).basicData.npcName;
            
            foreach (Transform child in rewardTextListParent)
                Destroy(child.gameObject);
            foreach (var reward in questStaticData.Rewards)
            {
                var made = Instantiate(rewardTextPrefab, rewardTextListParent);
                made.text = "* " + GetRewardText(reward);
            }
            
            foreach (Transform child in missionProgressListParent)
                Destroy(child.gameObject);
            for (var i=0; i < questStaticData.Steps.Count; i++)
            {
                var made = Instantiate(missionProgressViewPrefab, missionProgressListParent);
                made.SetQuestStep(_quest, i);
            }
        }

        private static string GetRewardText(Reward reward)
        {
            switch (reward)
            {
                case Reward.AffinityReward affinityReward:
                    return $"{NpcDatabase.Get(affinityReward.targetNpc).basicData.npcName}의 호감도 {affinityReward.amount}";
                case Reward.ItemReward itemReward:
                    var itemData = ItemDatabase.Get(itemReward.item.Code);
                    return $"{itemData.ItemName} x {itemReward.number}";
                default:
                    return "알 수 없는 보상";
            }
        }
        
    }
}