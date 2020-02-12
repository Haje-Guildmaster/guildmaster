using System;
using GuildMaster.Data;
using GuildMaster.Quests;
using UnityEngine;


namespace GuildMaster.UI
{
    public class QuestListWindow: DraggableWindow
    {
        public QuestListItem questListItemPrefab;
        public Transform listItemsParent;
        private const float ListYDiff = 60f;

        private void OnEnable()
        {
            PlayerData.Instance.QuestManager.Changed += Refresh;
        }

        private void OnDisable()
        {
            PlayerData.Instance.QuestManager.Changed -= Refresh;
        }

        protected override void OnOpen()
        {
            Refresh();
        }

        private void Refresh()
        {
            foreach (Transform child in listItemsParent)
                Destroy(child.gameObject);

            _listBottom = 0;

            foreach (var quest in PlayerData.Instance.QuestManager.CurrentQuests())
                AddItem(quest);
        }

        private float _listBottom = 0f;
        private void AddItem(Quest quest)
        {
            var item = Instantiate(questListItemPrefab, listItemsParent);
            item.clickChecker.onClick.AddListener(()=>UiWindowsManager.Instance.OpenQuestInspectWindow(quest));
            item.questNameText.text = quest.QuestData.QuestName;
            item.questClientText.text = quest.Client.basicData.npcName;
            
            
            item.transform.localPosition += new Vector3(0, _listBottom, 0);
            _listBottom -= ListYDiff;
        }
    }
}