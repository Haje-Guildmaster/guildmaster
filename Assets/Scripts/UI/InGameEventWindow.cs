using System;
using System.Collections.Generic;
using GuildMaster.Data;
using GuildMaster.InGameEvents;
using UnityEngine;
using UnityEngine.UI;

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


        public void Open()
        {
            eventNameText.text = Player.Instance.InGameEventManager.currentInGameEvent.InGameEventData.EventName;
            Debug.Log("open event window");
            Refresh();
        }

        public void Refresh()
        {
            eventDescriptionText.text = Player.Instance.InGameEventManager.currentInGameEvent.currentSceneData.SceneDescription;

            foreach (Transform child in listChoicesParent)
                Destroy(child.gameObject);

            buttonListBottom = 0f;

            int i = 0;
            foreach (var choice in Player.Instance.InGameEventManager.currentInGameEvent.currentSceneData.Choices)
            {
                AddChoice(choice, i);
                i++;
            }
        }

        private void AddChoice(InGameEventStaticData.InGameEventSceneData.InGameEventChoiceData choiceData, int choiceNum)
        {
            var choice = Instantiate(InGameEventChoiceItemPrefab, listChoicesParent);
            choice.clickChecker.onClick.AddListener(() => Player.Instance.InGameEventManager.Choose(choiceNum));
            choice.choiceDescText.text = choiceData.ChoiceScript;

            choice.transform.localPosition += new Vector3(0, buttonListBottom, 0);
            buttonListBottom -= buttonHeight;
        }

    }
}