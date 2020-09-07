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

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// ExplorationManager로부터 명령 받아 탐색 과정을 실제로 유저에게 보여주는 역할입니다.
    /// </summary>
    public class ExplorationView : MonoBehaviour
    {
        [SerializeField] private ExplorationRoadView _roadView;

        [FormerlySerializedAs("mapSelectView")] [SerializeField]
        private MapSelectView _mapSelectView;

        [SerializeField] private MapBaseSelector _baseSelector;
        [SerializeField] private MapAdjacentSelector _adjacentSelector;
        [SerializeField] private MinimapView _minimapView;
        [SerializeField] private Footer _footer;

        [SerializeField] private EventProcessView _eventProcessView;
        // Todo: Ui 부분 묶어서 한 클래스로 만들기?

        public State CurrentState { get; private set; } = State.Waiting;

        public enum State
        {
            /// <summary>
            /// 명령 사이에 존재하는 명령을 기다리는 상태. 이 상태로 설정한다고 해서 직접적으로
            /// 바뀌는 건 없으나 명령을 받기 전에 이 상태여야 하며
            /// 명령을 끝냈을 때 다시 이 상태로 돌아감. 이게 지켜지지 않을 경우 에러.
            /// </summary>
            Waiting,
            OnMove,
            EventProcessing,
            LocationSelecting,
        }

        public void Setup(List<Character> characters, ExplorationMap map)
        {
            // Todo: Setup이 없어야 하는가?
            Cleanup();
            _roadView.Setup(characters);
            _mapSelectView.LoadMap(map);
            _minimapView.LoadMap(map);
            _footer.Setup(characters);

            // Todo: 맵 종류 받아서 slideBackgroundView 초기화
            // Todo: 캐릭터 생성.
        }

        public async Task<MapNode> SelectStartingBase()
        {
            SetState(State.LocationSelecting);

            var ret = await _baseSelector.Select(_mapSelectView);
            SetState(State.Waiting);
            return ret;
        }


        /// <summary>
        /// 탐색 중, 처음 시작하고 시작 거점을 정한 후나 어떤 장소에 도착한 후에 불러짐. <br/>
        /// 다음 목적지를 고르고 반환.
        /// </summary>
        /// <param name="startingNode"> 시작 노드 </param>
        /// <return> 골라진 다음 목적지 </return>
        public async Task<MapNode> SelectNextDestination(MapNode startingNode)
        {
            SetState(State.LocationSelecting);

            var ret = await _adjacentSelector.Select(_mapSelectView, startingNode);
            SetState(State.Waiting);
            return ret;
        }

        /// <summary>
        /// 캐릭터들이 길을 가는 모습을 보여 준다.
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

            SetState(State.OnMove);

            _roadView.SetGoing(true);
            _roadView.TempResetPosition();
            _minimapView.SetPlayerIndicatorPath(startingBaseNode, headingNode);

            await ProcessRoadView();

            async Task ProcessRoadView()
            {
                const float moveTime = 4f; // progress 1이 증가하는 데에 필요한 시간.
                const float stepTime = 0.05f; // 스텝 시간
                var progress = startingProgress;

                var flag = false;
                while (true)
                {
                    _minimapView.SetProgress(progress);

                    if (flag) break;
                    await Task.Delay(TimeSpan.FromSeconds(stepTime));


                    progress += stepTime / moveTime;
                    if (progress >= headingProgress - 0.00001)
                    {
                        progress = headingProgress;
                        flag = true;
                    }
                }
            }

            _roadView.SetGoing(false);
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            SetState(State.Waiting);
        }

        /// <summary>
        /// 이벤트에서 선택지를 어떻게 보여 줄까에 대한 데이터.
        /// EventProcessView.ChoiceVisualData와 곂치는 건 의존성을 줄이기 위해서임다.
        /// </summary>
        public class ChoiceVisualData : EventProcessView.ChoiceVisualData{}

        public async Task<(int choiceIndex, Character selectedCharacter)> PlayEvent(
            List<ChoiceVisualData> choiceVisualDataList)
        {
            SetState(State.EventProcessing);
            var ret = await _eventProcessView.WaitUserDecision(_roadView.CharacterSprites,
                choiceVisualDataList.Select(cvd => (EventProcessView.ChoiceVisualData) cvd).ToList());
            SetState(State.Waiting);
            return ret;
        }

        /// <summary>
        /// 유저에게 알림. 현재는 그냥 메시지 박스 띄우기.
        /// </summary>
        /// <param name="notifyString"></param>
        public async Task Notify(string notifyString)
        {
            await UiWindowsManager.Instance.AsyncShowMessageBox("알림", notifyString, new[] {"확인"});
        }

        /// <summary>
        /// 자신의 상태 설정. 같은 상태가 여러번 불리더라도 똑같이 작동함
        /// (ex: SetState(State.OnMove)를 50번 반복해도 1번 호출한 것과 같은 효과)
        /// </summary>
        /// <param name="state"></param>
        private void SetState(State state)
        {
            Assert.IsTrue(CurrentState == State.Waiting || state == State.Waiting);
            switch (state)
            {
                case State.OnMove:
                    _minimapView.gameObject.SetActive(true);
                    _mapSelectView.gameObject.SetActive(false);
                    _eventProcessView.SetActive(false);
                    break;
                case State.LocationSelecting:
                    _minimapView.gameObject.SetActive(false);
                    _mapSelectView.gameObject.SetActive(true);
                    _eventProcessView.SetActive(false);
                    break;
                case State.EventProcessing:
                    _minimapView.gameObject.SetActive(true);
                    _mapSelectView.gameObject.SetActive(false);
                    _eventProcessView.SetActive(true);
                    break;
            }

            CurrentState = state;
        }


        private void Cleanup()
        {
            CurrentState = State.Waiting;
            // Todo:
        }
    }
}