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
        public NpcInteractWindow npcInteractWindow;
        public QuestSuggestWindow questSuggestWindow;
        public QuestListWindow questListWindow;
        public QuestInspectWindow questInspectWindow;
        public InGameEventWindow inGameEventWindow;
        public InventoryWindow inventoryWindow;
        public MessageBox messageBoxPrefab;
        public Transform messageBoxesParent;
        public CharacterInspectWindow characterInspectWindow;
        public GuildInspectWindow guildInspectWindow;
            
        public ItemInfoPanel itemInfoPanel;    // 임시.
        

        public void ShowMessageBox(string title, string content, IEnumerable<(string buttonText, Action onClicked)> buttons)
        {
            var made = Instantiate(messageBoxPrefab, messageBoxesParent);
            made.Open(title, content, buttons);
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
                    throw new Exception("There needs to be an active UiWindowManager component in the scene");
                return _instance;
            } 
        }
    }
}