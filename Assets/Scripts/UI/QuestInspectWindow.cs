using GuildMaster.Data;
using GuildMaster.Quests;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class QuestInspectWindow: DraggableWindow
    {
        public Text questNameText;
        public Text questDescriptionText;
        public Text clientNameText;
        public Button abandonButton;
        
        private Quest _quest;

        public void Set(Quest quest)
        {
            _quest = quest;
        }

        protected override void OnOpen()
        {
            questNameText.text = _quest.QuestData.QuestName;
            questDescriptionText.text = _quest.QuestData.QuestDescription;
            clientNameText.text = _quest.Client.basicData.npcName;

            abandonButton.onClick.RemoveAllListeners();
            var q = _quest;
            abandonButton.onClick.AddListener(() =>
            {
                PlayerData.Instance.QuestManager.AbandonQuest(q);
                Close();
            });
        }
        
    }
}