using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Exploration.Events;
using GuildMaster.Tools;
using GuildMaster.Windows;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// ExplorationManager로부터 명령받아 탐색 과정 전부를 유저에게 보여주는 역할입니다.
    /// </summary>
    public class ExplorationView : MonoBehaviour
    {
        // Todo: ExplorationUI로 분리?
        [SerializeField] private ExplorationRoadView _roadView;

        [FormerlySerializedAs("mapSelectView")] [SerializeField]
        private MapSelectView _mapSelectView;

        [SerializeField] private Toggle _mapSelectViewToggle;

        [SerializeField] private MapBaseSelector _baseSelector;
        [SerializeField] private MapAdjacentSelector _adjacentSelector;
        [SerializeField] private MinimapView _minimapView;
        [SerializeField] private Footer _footer;

        [SerializeField] private AsyncButton _explorationEndButton;

        [SerializeField] private EventProcessView _eventProcessView;

        public State CurrentState { get; private set; } = State.Waiting;

        public enum State
        {
            /// <summary>
            /// 명령 사이에 존재하는 명령을 기다리는 상태. 명령을 받기 전에 이 상태여야 하며
            /// 명령을 끝냈을 때 다시 이 상태로 돌아감. 이게 지켜지지 않을 경우 에러.
            /// </summary>
            Waiting,
            OnMove,
            EventProcessing,
            LocationSelecting,
        }

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="characters"> 탐색에 참여한 캐릭터들 </param>
        /// <param name="map"> 탐색하는 맵 </param>
        public void Setup(List<Character> characters, ExplorationMap map)
        {
            // Todo: Setup이 없어야 하는가?
            Cleanup();
            _roadView.Setup(characters);
            _mapSelectView.LoadMap(map);
            _minimapView.LoadMap(map);
            _footer.Setup(characters);
            _mapSelectViewToggle.onValueChanged.AddListener(b => _mapSelectView.gameObject.SetActive(b));

            // Todo: 맵 종류 받아서 slideBackgroundView 초기화
        }

        /// <summary>
        /// 유저가 시작하는 베이스캠프를 선택하거나 탐색 종료를 선택하는 것을 기다려 그 결정을 반환합니다. 
        /// </summary>
        /// <returns> (탐색을 그만두는지, 시작 베이스캠프) </returns>
        public async Task<(bool endExploration, MapNode startingNode)> SelectStartingBase()
        {
            SetStateLocationSelecting(true);

            async Task<(bool, MapNode)> Select() => (false, await _baseSelector.Select(_mapSelectView));

            async Task<(bool, MapNode)> End()
            {
                await _explorationEndButton.WaitForClick();
                return (true, null);
            }

            var ret = await new TaskFactory().ContinueWhenAny(new[]
            {
                Select(),
                End(),
            }, t => t.Result);
            SetStateWaiting();
            return ret;
        }


        /// <summary>
        /// 탐색 중, 처음 시작하고 시작 거점을 정한 후나 어떤 장소에 도착한 후에 불러짐. <br/>
        /// 다음 목적지를 고르거나, allowEndingExploration이 참이었을 경우 유저가 탐색 종료를 선택했을 때 그 결정을 반환함.
        /// </summary>
        /// <param name="startingNode"> 시작 노드 </param>
        /// <param name="allowEndingExploration"> 탐색 중단이 가능한지. </param>
        /// <return> (탐색을 종료하는지, 골라진 다음 목적지) </return>
        public async Task<(bool endExploration, MapNode destination)> SelectNextDestination(MapNode startingNode,
            bool allowEndingExploration)
        {
            SetStateLocationSelecting(allowEndingExploration);

            async Task<(bool, MapNode)> Select() =>
                (false, await _adjacentSelector.Select(_mapSelectView, startingNode));

            async Task<(bool, MapNode)> End()
            {
                await _explorationEndButton.WaitForClick();
                return (true, null);
            }

            (bool, MapNode) ret;
            if (allowEndingExploration)
            {
                var fac = new TaskFactory<(bool, MapNode)>();
                ret = await fac.ContinueWhenAny(new[] {Select(), End()}, t => t.Result);
            }
            else
            {
                ret = await Select();
            }

            SetStateWaiting();
            return ret;
        }

        /// <summary>
        /// 캐릭터들이 길을 가는 모습을 보여 줍니다.
        /// </summary>
        /// <param name="startingBaseNode"> 시작 노드 </param>
        /// <param name="headingNode"> 도착 지점 노드 </param>
        /// <param name="startingProgress"> 두 노드 사이 어느 지점에서 시작하는지. (0~1) </param>
        /// <param name="headingProgress"> 두 노드 사이 어느 지점에서 멈추고 반환하는지. (0~1) </param>
        public async Task PlayRoadView(MapNode startingBaseNode, MapNode headingNode,
            float startingProgress = 0f, float headingProgress = 1f)
        {
            Assert.IsTrue(startingProgress >= -0.00001f);
            Assert.IsTrue(headingProgress <= 1.00001f);

            SetStateOnMove();

            // Todo: 밑에 이거 수정.
            if (Math.Abs(startingProgress) < 0.001f)
                _roadView.TempResetPosition();

            _roadView.SetGoing(true);

            _minimapView.SetPlayerIndicatorPath(startingBaseNode, headingNode);

            await ProcessRoadView();

            async Task ProcessRoadView()
            {
                const float moveTime = 1f; // progress 1이 증가하는 데에 필요한 시간.
                // const float stepTime = 0.02f; // 스텝 시간
                var progress = startingProgress;

                var flag = false;
                while (true)
                {
                    _minimapView.SetProgress(progress);

                    if (flag) break;
                    await Task.Yield();


                    progress += Time.deltaTime / moveTime;
                    if (progress >= headingProgress - 0.00001)
                    {
                        progress = headingProgress;
                        flag = true;
                    }
                }
            }

            _roadView.SetGoing(false);
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            SetStateWaiting();
        }

        /// <summary>
        /// 게임 화면에 선택지에 대한 정보를 표시하기 위해 필요한 모든 데이터를 지닌 클래스.
        /// EventProcessView.ChoiceVisualData와 곂치는 건 이 함수의 호출자가 EventProcessView에 대한 의존성을 지니지 않도록 하기 위해서입니다.
        /// </summary>
        public class ChoiceVisualData : EventProcessView.ChoiceVisualData
        {
        }

        /// <summary>
        /// 이벤트의 선택지를 유저에게 보여주고 유저의 선택을 반환합니다.
        /// </summary>
        /// <param name="choiceVisualDataList"> 이벤트의 각 선택지가 어떤 모습으로 보여질 것인가를 정하는 데이터. </param>
        /// <param name="descriptionString"> 이벤트의 설명 </param>
        /// <param name="initialChoiceIndex"> 이벤트 화면에서 맨 처음 떠 있을 선택지 </param>
        /// <returns> (선택한 선택지의 인덱스, 선택한 캐릭터) </returns>
        public async Task<(int choiceIndex, Character selectedCharacter)> PlayEvent(
            IEnumerable<ChoiceVisualData> choiceVisualDataList, string descriptionString, int initialChoiceIndex = 0)
        {
            SetStateEventProcessing();
            var ret = await _eventProcessView.WaitUserDecision(_roadView.CharacterSprites,
                choiceVisualDataList.Select(cvd => (EventProcessView.ChoiceVisualData) cvd).ToList(),
                descriptionString, initialChoiceIndex);
            SetStateWaiting();
            return ret;
        }

        
        /// <summary>
        /// 유저에게 알림. 현재는 그냥 메시지 박스 띄우기.
        /// </summary>
        /// <param name="notifyString"> 띄울 알림 내용 </param>
        public async Task Notify(string notifyString)
        {
            await UiWindowsManager.Instance.AsyncShowMessageBox("알림", notifyString, new[] {"확인"});
        }

        /// <summary>
        /// ExplorationLog를 받아 최종적으로 탐색을 통해 무엇을 얻고 잃었는지 결산창을 띄웁니다. 유저가 결산창을 닫으면 반환합니다.
        /// </summary>
        /// <param name="log"> 탐색 로그 </param>
        /// <returns></returns>
        public async Task ReportExplorationResults(ExplorationLog log)
        {
            SetStateOnMove();

            var resultStr = "";

            resultStr += "획득한 아이템:\n";
            foreach (var pair in log.AcquiredItems)
                resultStr += $"{pair.Key.StaticData.ItemName} x {pair.Value}\n";
            resultStr += "사용한 아이템:\n";
            foreach (var pair in log.UsedItems)
                resultStr += $"{pair.Key.StaticData.ItemName} x {pair.Value}\n";

            await UiWindowsManager.Instance.AsyncShowMessageBox("탐색 결과", $"{resultStr}", new[] {"확인"});
            SetStateWaiting();
        }

        private void _SetState(bool minimap, bool eventProcessView, bool mapSelectView, bool explorationEndButton)
        {
            _minimapView.gameObject.SetActive(minimap);
            // _mapSelectView.gameObject.SetActive(false);
            _eventProcessView.SetActive(eventProcessView);
            _mapSelectViewToggle.gameObject.SetActive(mapSelectView);
            _mapSelectViewToggle.isOn = mapSelectView;
            _explorationEndButton.gameObject.SetActive(explorationEndButton);
        }

        private void SetStateOnMove()
        {
            Assert.IsTrue(CurrentState == State.Waiting);
            CurrentState = State.OnMove;
            _SetState(minimap: true, eventProcessView: false, mapSelectView: false, explorationEndButton: false);
        }

        private void SetStateLocationSelecting(bool canEndExploration)
        {
            Assert.IsTrue(CurrentState == State.Waiting);
            CurrentState = State.LocationSelecting;
            _SetState(minimap: false, eventProcessView: false, mapSelectView: true,
                explorationEndButton: canEndExploration);
        }

        private void SetStateEventProcessing()
        {
            Assert.IsTrue(CurrentState == State.Waiting);
            CurrentState = State.EventProcessing;
            _SetState(minimap: true, eventProcessView: true, mapSelectView: false, explorationEndButton: false);
        }

        private void SetStateWaiting()
        {
            CurrentState = State.Waiting;
        }


        private void Cleanup()
        {
            SetStateWaiting();
            // Todo:
        }
    }
}