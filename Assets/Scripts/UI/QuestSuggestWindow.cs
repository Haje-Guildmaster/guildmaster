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
        public UnityEvent accepted = new UnityEvent();
        public UnityEvent declined = new UnityEvent();
        
        
        [SerializeField] private Text questNameText;
        [SerializeField] private Text questDescriptionText;
        [SerializeField] private Text clientNameText;

        private QuestData _questData;
        private NpcData _npcData;

        public void Set(QuestData questData, NpcData npcData)
        {
            _questData = questData;
            _npcData = npcData;
        }
        
        protected override void OnOpen()
        {
            Refresh();
        }

        protected override void OnClose()
        {
            closed.Invoke();
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
            accepted.Invoke();
            Close();
        }

        public void DeclineQuest()
        {
            declined.Invoke();
            Close();
        }
    }
}