using System;
using GuildMaster.Data;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;


namespace GuildMaster.UI
{
    public class QuestListWindow: DraggableWindow, IToggleableWindow
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

        public void Open()
        {
            base.OpenWindow();
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
        private void AddItem(ReadOnlyQuest quest)
        {
            var item = Instantiate(questListItemPrefab, listItemsParent);
            item.clickChecker.onClick.AddListener(()=>UiWindowsManager.Instance.questInspectWindow.Open(quest));
            item.questNameText.text = quest.QuestData.QuestName;
            item.questClientText.text = NpcDatabase.Instance.GetElement(quest.Client).basicData.npcName;
            
            
            item.transform.localPosition += new Vector3(0, _listBottom, 0);
            _listBottom -= ListYDiff;
        }
    }
}