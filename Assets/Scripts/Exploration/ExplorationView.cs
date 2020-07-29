using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// ExplorationManager로부터 명령 받아 탐색 과정을 실제로 유저에게 보여주는 역할입니다.
    /// </summary>
    public class ExplorationView: MonoBehaviour
    {
        [SerializeField] private ExplorationRoadView _roadView;
        [SerializeField] private MapView _mapView;
        
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
            _mapView.SetHighlightFunc(lb =>
            {
                if (lb.Node.Content.Location.LocationType == Location.Type.Base)
                    return lb.IsUnderPointer ? Color.blue : Color.cyan;
                return Color.white;
            });
            _mapView.Select(node => node.Content.Location.LocationType == Location.Type.Base, StartRoadView);
        }

        public void Pause()
        {
            CurrentState = State.Paused;
            _roadView.SetGoing(false);
            // Todo:
        }

        private void StartRoadView(Graph<ExplorationMap.NodeContent>.Node node)
        {
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