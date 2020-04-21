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
        private NpcCode _npc;
        private NpcStaticData _npcStaticDataCache;

        public void Open(QuestStaticData questData, NpcCode npc)
        {
            base.OpenWindow();
            _questData = questData;
            _npc = npc;
            _npcStaticDataCache = NpcDatabase.Instance.GetElement(npc);
            Refresh();
        }

        private void Refresh()
        {
            questNameText.text = _questData.QuestName;
            questDescriptionText.text = _questData.QuestDescription;
            clientNameText.text = _npcStaticDataCache.basicData.npcName;
        }

        public void AcceptQuest()
        {
            PlayerData.Instance.QuestManager.ReceiveQuest(_questData, _npc);
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