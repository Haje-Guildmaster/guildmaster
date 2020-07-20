using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using UnityEngine;

namespace GuildMaster.Exploration
{
    public class ExplorationView: MonoBehaviour
    {
        [SerializeField] private ExplorationRoadView roadView;
        public void Setup(List<Character> characters)
        {
            Cleanup();
            roadView.Setup(characters);
            // Todo: 맵 종류 받아서 slideBackgroundView 초기화
            // Todo: 캐릭터 생성.
        }

        public void StartExploration()
        {
            CurrentState = State.OnMove;
            roadView.SetGoing(true);
            // Todo:
        }

        public void Pause()
        {
            CurrentState = State.Paused;
            roadView.SetGoing(false);
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
            Stopped, OnMove, Paused, EventProcessing
        }
    }
}