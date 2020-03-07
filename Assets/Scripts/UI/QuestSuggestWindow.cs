using System;
using GuildMaster.Data;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class QuestSuggestWindow: DraggableWindow
    {
        public event Action Accepted;
        public event Action Declined;
        
        
        [SerializeField] private Text questNameText;
        [SerializeField] private Text questDescriptionText;
        [SerializeField] private Text clientNameText;

        private QuestStaticData _questData;
        private NpcStaticData _npcData;

        public void Set(QuestStaticData questData, NpcStaticData npcData)
        {
            _questData = questData;
            _npcData = npcData;
        }
        
        protected override void OnOpen()
        {
            Refresh();
        }

        private void Refresh()
        {
            questNameText.text = _questData.QuestName;
            questDescriptionText.text = _questData.QuestDescription;
            clientNameText.text = _npcData.basicData.npcName;
        }

        public void AcceptQuest()
        {
            PlayerData.Instance.QuestManager.ReceiveQuest(_questData, _npcData);
            Accepted?.Invoke();
            Close();
        }

        public void DeclineQuest()
        {
            Declined?.Invoke();
            Close();
        }
    }
}