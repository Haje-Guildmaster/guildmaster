using System;
using System.Collections;
using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    /// <summary>
    /// ExplorationManager로부터 명령 받아 탐색 과정을 실제로 유저에게 보여주는 역할입니다.
    /// </summary>
    public class ExplorationView: MonoBehaviour
    {
        [SerializeField] private ExplorationRoadView _roadView;
        [SerializeField] private MapView _mapView;
        [SerializeField] private MapBaseSelector _baseSelector;
        [SerializeField] private MapAdjacentSelector _adjacentSelector;
        [SerializeField] private MinimapView _minimapView;
        
        public void Setup(List<Character> characters, ExplorationMap map)
        {
            Cleanup();
            _roadView.Setup(characters);
            _mapView.LoadMap(map);
            _minimapView.LoadMap(map);
            
            // Todo: 맵 종류 받아서 slideBackgroundView 초기화
            // Todo: 캐릭터 생성.
        }

        public void SelectStartingBase(Action<MapNode> callback)
        {
            SetState(State.LocationSelecting);
            
            _minimapView.gameObject.SetActive(false);
            _mapView.gameObject.SetActive(true);
            _baseSelector.Select(_mapView, callback);
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
        /// 다음 목적지를 고르고 callback으로 반환.
        /// </summary>
        /// <param name="startingNode"> 시작 노드 </param>
        /// <param name="callback"> callback </param>
        public void SelectNextDestination(MapNode startingNode, Action<MapNode> callback)
        {
            SetState(State.LocationSelecting);
            
            _adjacentSelector.Select(_mapView, startingNode, callback);
        }

        public void StartRoadView(List<Character> characters, MapNode startingBaseNode, MapNode headingNode, Action callback)
        {
            SetState(State.OnMove);
            
            _roadView.Setup(characters);
            _roadView.SetGoing(true);

            _minimapView.SetPlayerIndicatorPath(startingBaseNode, headingNode);
            StartCoroutine(ProcessRoadView());

            IEnumerator ProcessRoadView()
            {
                const float moveTime = 4f;
                const float stepTime = 0.05f;
                var progress = 0f;

                
                var flag = false;
                while (true)
                {
                    _minimapView.SetProgress(progress);
                    if (flag) break;
                    yield return new WaitForSeconds(stepTime);

                    progress += stepTime / moveTime;
                    if (progress >= 1 - 0.00001)
                    {
                        progress = 1;
                        flag = true;
                    }
                }
                
                _roadView.SetGoing(false);
                yield return new WaitForSeconds(0.5f);
                
                callback();
            }   
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.OnMove:
                    _minimapView.gameObject.SetActive(true);
                    _mapView.gameObject.SetActive(false);
                    break;
                case State.LocationSelecting:
                    _minimapView.gameObject.SetActive(false);
                    _mapView.gameObject.SetActive(true);
                    break;
            }
            CurrentState = State.OnMove;
        }
        
        
        private void Cleanup()
        {
            CurrentState = State.Stopped;
            // Todo:
        }

        public State CurrentState { get; private set; } = State.Stopped;
        public enum State
        {
            Stopped, OnMove, Paused, EventProcessing, LocationSelecting,
        }
    }
}