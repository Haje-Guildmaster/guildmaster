using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GuildMaster.Data;
using GuildMaster.Dialog;
using GuildMaster.Events;
using GuildMaster.Quests;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.Npcs
{
    public class NpcInteractUI: MonoBehaviour
    {
        [SerializeField] private Image illustration;
        [SerializeField] private Text dialogTextBox;
        [SerializeField] private RectTransform interactionButtonsParent;
        [SerializeField] private NpcInteractionButton interactionButtonPrefab;
        
        
        public Image Illustration => illustration;
        public Text DialogTextBox => dialogTextBox;

        public void Start()
        {
            gameObject.SetActive(false);
        }
        public void Open(NpcData npc)
        {
            _npcData = npc;
            gameObject.SetActive(true);
            
            InitialScreen();

            var questTalks = PlayerData.Instance.QuestManager.GetCompletableTalkMissions(npc);
            if (questTalks.Any())
                PlayTalkMissionScript(questTalks[0]);
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }

        
        private NpcData _npcData;
        private float _interactionListBottom;
        private const float InteractionButtonYDiff = 43f;
        private void InitialScreen()
        {
            var basicData = _npcData.basicData;
            illustration.sprite = basicData.illustration;
            dialogTextBox.text = $"[{basicData.npcName}]\n{basicData.greeting}";
            InitializeInteractionButtonList();
        }

        private void InitializeInteractionButtonList()
        {
            foreach (Transform child in interactionButtonsParent)
                Destroy(child.gameObject);

            _interactionListBottom = 0f;
            AddInteractionButtonToList("퀘스트 받기", ()=>
            {
                var questManager = PlayerData.Instance.QuestManager;
                var availableQuests = questManager.WhereAvailable(_npcData.questData.QuestList);
                if (availableQuests.Any())
                {
                    var quest = availableQuests[0];
                    PlayScript(quest.QuestSuggestScript);
                    questManager.ReceiveQuest(quest);
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
            GameEvents.QuestScriptPlayEnd.Invoke(talkMission);
        }
    }
}