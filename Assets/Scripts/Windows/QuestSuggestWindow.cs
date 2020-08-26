using System;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class QuestSuggestWindow: DraggableWindow
    {
        public event Action Accepted;
        public event Action Declined;
        
        
        [SerializeField] private Text questNameText;
        [SerializeField] private Text questDescriptionText;
        [SerializeField] private Text clientNameText;

        private QuestCode _questCode;
        private QuestStaticData _questStaticDataCache;
        private NpcCode _npc;
        private NpcStaticData _npcStaticDataCache;

        public void Open(QuestCode questCode, NpcCode npc)
        {
            base.OpenWindow();
            _questCode = questCode;
            _questStaticDataCache = QuestDatabase.Get(questCode);
            _npc = npc;
            _npcStaticDataCache = NpcDatabase.Get(npc);
            Refresh();
        }

        private void Refresh()
        {
            questNameText.text = _questStaticDataCache.QuestName;
            questDescriptionText.text = _questStaticDataCache.QuestDescription;
            clientNameText.text = _npcStaticDataCache.basicData.npcName;
        }

        public void AcceptQuest()
        {
            Player.Instance.QuestManager.ReceiveQuest(_questCode, _npc);
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