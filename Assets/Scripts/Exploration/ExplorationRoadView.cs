using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using UnityEngine;

namespace GuildMaster.Exploration
{
    public class ExplorationRoadView : MonoBehaviour
    {
        [SerializeField] private SlideBackgroundView slideBackgroundView;
        [SerializeField] private List<ExplorationCharacterSprite> characterSprites;
        [SerializeField] private SlideBackgroundElement characterSlide;        // Todo: 임시.

        private const float NormalMoveSpeed = 0.012f;
        private float MoveSpeed = NormalMoveSpeed;

        public void Setup(List<Character> characters)
        {
            // Todo: 
        }
        
        public void SetGoing(bool going)
        {
            _going = going;
            foreach (var chtr in characterSprites)
                chtr.SetMoveAnimation(going);
        }

        private void Update()
        {
            if (!_going) return;
            _currentPosition += MoveSpeed;
            foreach (var chtr in characterSprites)
            {
                chtr.Goto(_currentPosition*characterSlide.MoveRatio);
            }

            slideBackgroundView.CurrentViewLocation = new Vector2(_currentPosition, 0);
        }

        private float _currentPosition = 0f;
        private bool _going;
    }
}