using System;
using System.Collections.Generic;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.TownRoam.Towns;
using GuildMaster.UI.Inventory;
using UnityEngine;

namespace GuildMaster.UI
{
    public class UiWindowsManager: MonoBehaviour
    {
        [SerializeField] private NpcInteractWindow npcInteractWindow;
        [SerializeField] private QuestSuggestWindow questSuggestWindow;
        [SerializeField] private QuestListWindow questListWindow;
        [SerializeField] private QuestInspectWindow questInspectWindow;
        [SerializeField] private InGameEventWindow inGameEventWindow;
        [SerializeField] private InventoryWindow inventoryWindow;
        [SerializeField] private MessageBox messageBoxPrefab;
        [SerializeField] private Transform messageBoxesParent;
        [SerializeField] private CharacterInspectWindow characterInspectWindow;
        
            
        public ItemInfoPanel itemInfoPanel;    // 임시.
        
        public void OpenNpcInteractWindow(NpcStaticData npcData)
        {
            npcInteractWindow.SetNpc(npcData);
            npcInteractWindow.Open();
        }

        public void OpenQuestSuggestWindow(QuestStaticData questData, NpcStaticData npcData)
        {
            questSuggestWindow.Set(questData, npcData);
            questSuggestWindow.Open();
        }

        public void OpenQuestListWindow() => questListWindow.Open();
        public void ToggleQuestListWindow()
        {
            questListWindow.Toggle();
        }

        public void OpenQuestInspectWindow(ReadOnlyQuest quest)
        {
            questInspectWindow.Set(quest);
            questInspectWindow.Open();
        }
        
        public void OpenInGameEventWindow()
        {
            inGameEventWindow.Open();
        }

        public void RefreshInGameEventWindow()
        {
            inGameEventWindow.Refresh();
        }

        public void CloseInGameEventWindow()
        {
            inGameEventWindow.Close();
        }

        public void ToggleInventoryWindow()
        {
            inventoryWindow.Toggle();
        }

        public void ShowMessageBox(string title, string content, IEnumerable<(string buttonText, Action onClicked)> buttons)
        {
            var made = Instantiate(messageBoxPrefab, messageBoxesParent);
            made.Set(title, content, buttons);
            made.Open();
        }

        public void ToggleCharacterInspectWindow()
        {
            characterInspectWindow.Toggle();
        }
            
        private static UiWindowsManager _instance;
        public static UiWindowsManager Instance 
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = FindObjectOfType<UiWindowsManager>();  
                if (!_instance)
                    throw new Exception("There needs to be an active WindowManager component in the scene");
                return _instance;
            } 
        }
    }
}