using System;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Items;
using GuildMaster.Quests;
using GuildMaster.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class QuestInspectWindow: DraggableWindow
    {
        public Text questNameText;
        public Text questDescriptionText;
        public Text clientNameText;
        public Text rewardTextPrefab;
        public Transform rewardTextListParent;
        public MissionProgressView missionProgressViewPrefab;
        public Transform missionProgressListParent;

        private ReadOnlyQuest _quest;

        public void Set(ReadOnlyQuest quest)
        {
            _quest = quest;
        }

        public void AbandonCurrentQuest()
        {
            PlayerData.Instance.QuestManager.AbandonQuest(_quest);
            Close();
        }

        protected override void OnOpen()
        {
            if (_quest == null) return;
            questNameText.text = _quest.QuestData.QuestName;
            questDescriptionText.text = _quest.QuestData.QuestDescription;
            clientNameText.text = _quest.Client.basicData.npcName;
            
            foreach (Transform child in rewardTextListParent)
                Destroy(child.gameObject);
            foreach (var reward in _quest.QuestData.Rewards)
            {
                var made = Instantiate(rewardTextPrefab, rewardTextListParent);
                made.text = "* " + GetRewardText(reward);
            }
            
            foreach (Transform child in missionProgressListParent)
                Destroy(child.gameObject);
            for (var i=0; i < _quest.QuestData.Steps.Count; i++)
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
                    return $"{affinityReward.targetNpc.basicData.npcName}의 호감도 {affinityReward.amount}";
                case Reward.ItemReward itemReward:
                    var itemData = ItemDatabase.Instance.GetItemStaticData(itemReward.item.Code);
                    return $"{itemData.ItemName} x {itemReward.number}";
                default:
                    return "알 수 없는 보상";
            }
        }
        
    }
}