using System;
using System.Collections.Generic;
using GuildMaster.Data;
using GuildMaster.InGameEvents;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace GuildMaster.UI
{
    public class InGameEventWindow : DraggableWindow
    {
        [SerializeField] private Text eventNameText;
        [SerializeField] private Text eventDescriptionText;

        public InGameEventChoiceItem InGameEventChoiceItemPrefab;
        public Transform listChoicesParent;
        private float buttonListBottom = 0f;
        private const float buttonHeight = 40f;
        
        private void Start()
        {
            
        }

        protected override void OnOpen()
        {
            eventNameText.text = PlayerData.Instance.InGameEventManager.currentInGameEvent.InGameEventData.EventName;
            Debug.Log("open event window");
            Refresh();
        }

        public void Refresh()
        {
            eventDescriptionText.text = PlayerData.Instance.InGameEventManager.currentInGameEvent.currentSceneData.InGameEventDescription;

            foreach (Transform child in listChoicesParent)
                Destroy(child.gameObject);

            buttonListBottom = 0f;

            int i = 0;
            foreach (var choice in PlayerData.Instance.InGameEventManager.currentInGameEvent.currentSceneData.Choices)
            {
                AddChoice(choice, i);
                i++;
            }
        }

        private void AddChoice(InGameEventChoiceData choiceData, int choiceNum)
        {
            var choice = Instantiate(InGameEventChoiceItemPrefab, listChoicesParent);
            choice.clickChecker.onClick.AddListener(() => PlayerData.Instance.InGameEventManager.Choose(choiceNum));
            choice.choiceDescText.text = choiceData.ChoiceScript;

            choice.transform.localPosition += new Vector3(0, buttonListBottom, 0);
            buttonListBottom -= buttonHeight;
        }

    }
}