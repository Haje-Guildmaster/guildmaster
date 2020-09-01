using System;
using System.Collections.Generic;
using GuildMaster.Windows.Inventory;
using UnityEngine;

namespace GuildMaster.Windows
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
        
        public SettingWindow settingWindow;
        public TextureWindow TextureWindow;
        public MVWindow MvWindow;
        public ResolutionWindow ResolutionWindow;
        public SEWindow SeWindow;
        public TextspeedWindow TextspeedWindow;
        public BGMWindow BGMWindow;
        public ExplorationCharacterSelectingWindow ExplorationCharacterSelectingWindow;
            
        public ItemInfoPanel itemInfoPanel;    // 임시.
        

        public void ShowMessageBox(string title, string content, IEnumerable<(string buttonText, Action onClicked)> buttons)
        {
            var made = Instantiate(messageBoxPrefab, messageBoxesParent);
            made.Open(title, content, buttons);
        }

        private static UiWindowsManager _instance;
        public static UiWindowsManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<UiWindowsManager>());
    }
}