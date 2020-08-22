using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Tools;
using GuildMaster.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        [SerializeField] private ScrollPicker _decisionSelector;
        [SerializeField] private GameObject _tempCharacterSelectHelperParent;
        [SerializeField] private GameObject _tempEventDescriptionLabel;

        private void Awake()
        {
            // Todo: 수정.
            foreach (var cshb in _tempCharacterSelectHelperParent.GetComponentsInChildren<Button>())
            {
                cshb.onClick.AddListener(()=>CharacterSelected?.Invoke());
            }
        }

        public State CurrentState { get; private set; } = State.Stopped;

        public enum State
        {
            Stopped,
            OnMove,
            Paused,
            EventProcessing,
            LocationSelecting,
        }

        public void Setup(List<Character> characters, ExplorationMap map)
        {
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

            return await _baseSelector.Select(_mapSelectView);
        }

        [Obsolete]
        public void Pause()
        {
            SetState(State.Paused);
            _roadView.SetGoing(false);
            // Todo:
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

            return await _adjacentSelector.Select(_mapSelectView, startingNode);
        }

        public async Task PlayRoadView(List<Character> characters, MapNode startingBaseNode, MapNode headingNode)
        {
            SetState(State.OnMove);

            _roadView.Setup(characters);
            _roadView.SetGoing(true);

            _minimapView.SetPlayerIndicatorPath(startingBaseNode, headingNode);

            await ProcessRoadView();

            async Task ProcessRoadView()
            {
                const float moveTime = 4f;
                const float stepTime = 0.05f;
                var progress = 0f;

                var beforeEvent = true;

                var flag = false;
                while (true)
                {
                    _minimapView.SetProgress(progress);
                    if (beforeEvent && progress > 0.5f)
                    {
                        beforeEvent = false;
                        await TempProcessEvent();
                        
                        SetState(State.OnMove);
                    }

                    if (flag) break;
                    await Task.Delay(TimeSpan.FromSeconds(stepTime));

                    progress += stepTime / moveTime;
                    if (progress >= 1 - 0.00001)
                    {
                        progress = 1;
                        flag = true;
                    }
                }

                _roadView.SetGoing(false);
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
            }
        }
        
        private async Task TempProcessEvent()
        {
            SetState(State.EventProcessing);
            _decisionSelector.SetSelectedIndex(0, false);

            var tcs = new TaskCompletionSource<object>();

            CharacterSelected += EventEnd;
            void EventEnd()
            {
                CharacterSelected -= EventEnd;
                tcs.SetResult(null);
            }

            await tcs.Task;

            await UiWindowsManager.Instance.AsyncShowMessageBox("결과", "뭐가 어떻게 됐고 뭘 얻었고 어쩌고 저쩌고", new[] {"확인"});
        }
        
        
        private event Action CharacterSelected;

        private void SetState(State state)
        {
            switch (state)
            {
                case State.OnMove:
                    _roadView.SetGoing(true);
                    _minimapView.gameObject.SetActive(true);
                    _mapSelectView.gameObject.SetActive(false);
                    _tempEventDescriptionLabel.SetActive(false);
                    _tempCharacterSelectHelperParent.SetActive(false);
                    _decisionSelector.gameObject.SetActive(false);
                    break;
                case State.LocationSelecting:
                    _roadView.SetGoing(false);
                    _minimapView.gameObject.SetActive(false);
                    _mapSelectView.gameObject.SetActive(true);
                    _tempEventDescriptionLabel.SetActive(false);
                    _tempCharacterSelectHelperParent.SetActive(false);
                    _decisionSelector.gameObject.SetActive(false);
                    break;
                case State.EventProcessing:
                    _roadView.SetGoing(false);
                    _minimapView.gameObject.SetActive(true);
                    _mapSelectView.gameObject.SetActive(false);
                    _tempEventDescriptionLabel.SetActive(true);
                    _tempCharacterSelectHelperParent.SetActive(true);
                    _decisionSelector.gameObject.SetActive(true);
                    break;
            }

            CurrentState = state;
        }


        private void Cleanup()
        {
            CurrentState = State.Stopped;
            // Todo:
        }
    }
}