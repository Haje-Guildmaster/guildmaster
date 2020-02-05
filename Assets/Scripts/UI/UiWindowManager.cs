using System;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.UI
{
    public class UiWindowManager: MonoBehaviour
    {
        [SerializeField] private NpcInteractWindow npcInteractWindow;
        [SerializeField] private QuestSuggestWindow questSuggestWindow;

        public void OpenNpcInteractWindow(NpcData npcData)
        {
            npcInteractWindow.SetNpc(npcData);
            npcInteractWindow.Open();
        }

        public void OpenQuestSuggestWindow(QuestData questData, NpcData npcData)
        {
            questSuggestWindow.Set(questData, npcData);
            questSuggestWindow.Open();
        }
        
        private static UiWindowManager _instance;
        public static UiWindowManager Instance 
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = FindObjectOfType<UiWindowManager>();  
                if (!_instance)
                    throw new Exception("There needs to be an active WindowManager component in the scene");
                return _instance;
            } 
        }
    }
}