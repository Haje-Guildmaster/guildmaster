using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Dialog;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class NpcInteractWindow : Window
    {
        public static event Action<StepMission.TalkMission> QuestScriptPlayEnd;


        [SerializeField] private Image illustration;
        [SerializeField] private TextEffect dialogTextBox;
        [SerializeField] private RectTransform interactionButtonsParent;
        [SerializeField] private NpcInteractionButton interactionButtonPrefab;
        [SerializeField] private QuestSuggestWindow questSuggestWindow;
        [SerializeField] private ProgressBar affinityBar;

        public Image Illustration => illustration;
        //public TextEffect DialogTextBox => dialogTextBox;


        private void Start()
        {
            questSuggestWindow.Accepted += () => PlayScript(new Script {str = "(퀘스트를 수락하셨습니다)"});
            questSuggestWindow.Declined += () => PlayScript(new Script {str = "(퀘스트를 거부하셨습니다)"});
        }


        public void Open(NpcCode npcCode)
        {
            Cleanup();
            base.OpenWindow();
            _npc = Player.Instance.GetNpc(npcCode);
            _npc.Status.Affinity.Changed += UpdateAffinityBar;
            
            InitialScreen();

            var questTalks = Player.Instance.QuestManager.GetCompletableTalkMissions(_npc);
            if (questTalks.Any())
                PlayTalkMissionScript(questTalks[0]);
        }


        private Npc _npc;
        private float _interactionListBottom;
        private const float InteractionButtonYDiff = 43f;    // 아직 Layout을 모르던 때라 버튼을 밑에 추가하는 걸 단순히 y값을 빼는 것으로 처리
                                                             // 이후 비슷한 걸 만든다면 대신 VerticalLayout을 사용해 주세요.

        private void InitialScreen()
        {
            var basicData = _npc.StaticData.basicData;
            illustration.sprite = basicData.illustration;
            dialogTextBox.SetMsg($"[{basicData.npcName}]\n{basicData.greeting}");
            InitializeInteractionButtonList();
            UpdateAffinityBar();
        }

        private void InitializeInteractionButtonList()
        {
            foreach (Transform child in interactionButtonsParent)
                Destroy(child.gameObject);

            _interactionListBottom = 0f;
            AddInteractionButtonToList("퀘스트 받기", () =>
            {
                var questManager = Player.Instance.QuestManager;
                var availableQuests = questManager.GetAvailableQuestsFrom(_npc.StaticData.questData.QuestList);
                if (availableQuests.Any())
                {
                    var questCode = availableQuests[0];
                    var questStaticData = QuestDatabase.Get(questCode);
                    PlayScript(questStaticData.QuestSuggestScript);
                    UiWindowsManager.Instance.questSuggestWindow.Open(questCode, _npc);
                }
                else
                    dialogTextBox.SetMsg("가능한 퀘스트가 없습니다.");
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

        private void PlayScript(Script script) // function for displacing text - Bang
        {
            dialogTextBox.SetMsg(script.str);
        }

        private void PlayTalkMissionScript(StepMission.TalkMission talkMission)
        {
            PlayScript(talkMission.script);
            QuestScriptPlayEnd?.Invoke(talkMission);
        }

        private void UpdateAffinityBar()
        {
            affinityBar.SetProgress((float) _npc.Status.Affinity / 100);
        }

        private void Cleanup()
        {
            if (_npc != null)
            {
                _npc.Status.Affinity.Changed -= UpdateAffinityBar;
            }

            _npc = null;
        }

        protected override void OnClose()
        {
            Cleanup();
            base.OnClose();
        }
    }
}