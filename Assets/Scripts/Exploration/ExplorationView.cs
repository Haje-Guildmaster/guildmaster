using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Tools;
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
        
        public void Setup(List<Character> characters, ExplorationMap map)
        {
            Cleanup();
            _roadView.Setup(characters);
            _mapView.LoadMap(map);
            
            // Todo: 맵 종류 받아서 slideBackgroundView 초기화
            // Todo: 캐릭터 생성.
        }

        public void StartExploration()
        {
            CurrentState = State.LocationSelecting;
            _mapView.gameObject.SetActive(true);
            _baseSelector.Select(_mapView, SelectNextDestination);
        }

        public void Pause()
        {
            CurrentState = State.Paused;
            _roadView.SetGoing(false);
            // Todo:
        }

        /// <summary>
        /// 탐색 중, 처음 시작하고 시작 거점을 정한 후나 어떤 장소에 도착한 후에 불러짐. <br/>
        /// 다음 목적지를 고르고 탐색시작.
        /// </summary>
        /// <param name="startingNode"> 시작 노드 </param>
        private void SelectNextDestination(MapNode startingNode)
        {
            _adjacentSelector.Select(_mapView, startingNode, node=>StartRoadView(startingNode, node));
        }

        private void StartRoadView(MapNode startingBaseNode, MapNode headingNode)
        {
            Assert.IsTrue(startingBaseNode.Content.Location.LocationType == Location.Type.Base);
            // Assert.IsTrue(startingBaseNode.Connected.Contains(headingNode.NodeIndex));
            
            CurrentState = State.OnMove;
            
            _roadView.SetGoing(true);
            _mapView.gameObject.SetActive(false);
            // Todo:
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