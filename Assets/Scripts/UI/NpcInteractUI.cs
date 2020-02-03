using System.Linq;
using GuildMaster.Data;
using GuildMaster.Dialog;
using GuildMaster.Events;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class NpcInteractUI: MonoBehaviour
    {
        [SerializeField] private Image illustration;
        [SerializeField] private Text dialogTextBox;
        [SerializeField] private RectTransform interactionButtonsParent;
        [SerializeField] private NpcInteractionButton interactionButtonPrefab;
        [SerializeField] private QuestSuggestWindow questSuggestWindow;
        
        public Image Illustration => illustration;
        public Text DialogTextBox => dialogTextBox;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            questSuggestWindow.accepted.AddListener(()=>{PlayScript(new Script{str="(퀘스트를 수락하셨습니다)"});});
            questSuggestWindow.declined.AddListener(()=>{PlayScript(new Script{str="(퀘스트를 거부하셨습니다)"});});
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
                    questSuggestWindow.Open(quest, _npcData);
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