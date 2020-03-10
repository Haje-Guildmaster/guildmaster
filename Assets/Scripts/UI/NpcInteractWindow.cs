﻿using System;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class NpcInteractWindow: Window
    {
        public static event Action<StepMission.TalkMission> QuestScriptPlayEnd;
        
        
        [SerializeField] private Image illustration;
        [SerializeField] private Text dialogTextBox;
        [SerializeField] private RectTransform interactionButtonsParent;
        [SerializeField] private NpcInteractionButton interactionButtonPrefab;
        [SerializeField] private QuestSuggestWindow questSuggestWindow;
        [SerializeField] private ProgressBar affinityBar;
        
        public Image Illustration => illustration;
        public Text DialogTextBox => dialogTextBox;

        
        private void Start()
        {
            questSuggestWindow.Accepted += ()=>PlayScript(new Script{str="(퀘스트를 수락하셨습니다)"});
            questSuggestWindow.Declined += ()=>PlayScript(new Script{str="(퀘스트를 거부하셨습니다)"});;
            PlayerData.Instance.QuestManager.Changed += UpdateAffinityBar;
        }
        
        
        public void Open(NpcStaticData npcData)
        {
            base.OpenWindow();
            _npcData = npcData;
            
            
            InitialScreen();

            var questTalks = PlayerData.Instance.QuestManager.GetCompletableTalkMissions(_npcData);
            if (questTalks.Any())
                PlayTalkMissionScript(questTalks[0]);
        }

        
        private NpcStaticData _npcData;
        private float _interactionListBottom;
        private const float InteractionButtonYDiff = 43f;
        private void InitialScreen()
        {
            var basicData = _npcData.basicData;
            illustration.sprite = basicData.illustration;
            dialogTextBox.text = $"[{basicData.npcName}]\n{basicData.greeting}";
            InitializeInteractionButtonList();
            UpdateAffinityBar();
        }

        private void InitializeInteractionButtonList()
        {
            foreach (Transform child in interactionButtonsParent)
                Destroy(child.gameObject);

            _interactionListBottom = 0f;
            AddInteractionButtonToList("퀘스트 받기", ()=>
            {
                var questManager = PlayerData.Instance.QuestManager;
                var availableQuests = questManager.GetAvailableQuestsFrom(_npcData.questData.QuestList);
                if (availableQuests.Any())
                {
                    var quest = availableQuests[0];
                    PlayScript(quest.QuestSuggestScript);
                    UiWindowsManager.Instance.questSuggestWindow.Open(quest, _npcData);
                }
                else
                    dialogTextBox.text = "가능한 퀘스트가 없습니다.";
            });
        }

        private void AddInteractionButtonToList(string buttonText, UnityAction handler)
        {
            var button = Instantiate(interactionButtonPrefab, interactionButtonsParent);
            button.button.onClick.AddListener(handler);
            button.buttonText.text = buttonText;
            button.transform.localPosition += new Vector3(0, _interactionListBottom, 0);
            _interactionListBottom -= InteractionButtonYDiff;
        }

        private void PlayScript(Script script)
        {
            dialogTextBox.text = script.str;
        }
        private void PlayTalkMissionScript(StepMission.TalkMission talkMission)
        {
            PlayScript(talkMission.script);
            QuestScriptPlayEnd?.Invoke(talkMission);
        }

        private void UpdateAffinityBar()
        {
            affinityBar.SetProgress((float) PlayerData.Instance.GetNpcStatus(_npcData).Affinity / 100);
        }
    }
}