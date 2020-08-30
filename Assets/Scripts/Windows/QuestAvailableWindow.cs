using System;
using System.Collections.Generic;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class QuestAvailableWindow : DraggableWindow
    {

        [SerializeField] private RectTransform questOpenButtonsParent;
        [SerializeField] private NpcInteractionButton questOpenButtonPrefab;


        public void Open(List<QuestCode> questCodes, NpcCode npc, UnityAction<Script> playScript)
        {
            base.OpenWindow();
            foreach (Transform child in questOpenButtonsParent)
                Destroy(child.gameObject);
            foreach (var questCode in questCodes)
            {
                var questStaticData= QuestDatabase.Get(questCode);
                AddQuestOpenButtonToList(questStaticData.QuestName, () =>
                    {
                        UiWindowsManager.Instance.questSuggestWindow.Open(questCode, npc);
                        playScript(questStaticData.QuestSuggestScript);
                    }
                    );
            }
            
        }
        private void AddQuestOpenButtonToList(string buttonText, UnityAction handler)
        {
            var button = Instantiate(questOpenButtonPrefab, questOpenButtonsParent);
            button.button.onClick.AddListener(handler);
            button.buttonText.text = buttonText;
        }
    }
}