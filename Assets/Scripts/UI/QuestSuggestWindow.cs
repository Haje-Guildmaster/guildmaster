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
        public UnityEvent closed = new UnityEvent();
        public UnityEvent accepted = new UnityEvent();
        public UnityEvent declined = new UnityEvent();
        
        
        [SerializeField] private Text questNameText;
        [SerializeField] private Text questDescriptionText;
        [SerializeField] private Text clientNameText;

        private QuestData _questData;
        private NpcData _npcData;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Open(QuestData questData, NpcData npcData)
        {
            gameObject.SetActive(true);
            _questData = questData;
            _npcData = npcData;
            Refresh();
        }

        public void Close()
        {
            gameObject.SetActive(false);
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
            PlayerData.Instance.QuestManager.ReceiveQuest(_questData);
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