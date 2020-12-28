using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색 씬에서 캐릭터들이 실제로 이동하는 길을 유저에게 보여주는 역할의 클래스입니다. 현재는 뒷쪽의 배경이 각자의 속도로 이동하는
    /// 기능과 탐색에 나온 캐릭터들이 길에서 움직이는 것처럼 보이게 하는 기능을 담당합니다.
    /// </summary>
    public class ExplorationRoadView : MonoBehaviour
    {
        [SerializeField] private SlideBackgroundView slideBackgroundView;
        /// <summary>
        /// 빈 캐릭터 스프라이트들. 이들에게 캐릭터를 설정해 주어 사용.
        /// </summary>
        [SerializeField] private List<CharacterSprite> characterSpriteFrames; 
        [SerializeField] private SlideBackgroundElement characterSlide;        // Todo: 임시.
        
        
        private const float NormalMoveSpeed = 8f;

        public IEnumerable<CharacterSprite> CharacterSprites => characterSpriteFrames.Take(_characterCount);

        public void TempResetPosition()
        {
            // Todo:
            _currentPosition = 0f;
        }
        
        
        public void Setup(List<Character> characters)
        {
            Assert.IsTrue(characters.Count <= characterSpriteFrames.Count);
            Cleanup();
            
            _characterCount = characters.Count;
            for (var i = 0; i < characterSpriteFrames.Count; i++)
            {
                characterSpriteFrames[i].Character = i<_characterCount ? characters[i] : null; 
            }

            _going = false;
            TempResetPosition();
            _moveSpeed = NormalMoveSpeed;
        }

        public void SetGoing(bool going)
        {
            _going = going;
            foreach (var chtr in characterSpriteFrames)
                chtr.SetMoveAnimation(going);
        }

        private void Update()
        {
            if (!_going) return;
            _currentPosition += _moveSpeed * Time.deltaTime;
            foreach (var chtr in characterSpriteFrames)
            {
                chtr.Goto(_currentPosition*characterSlide.MoveRatio);
            }

            slideBackgroundView.CurrentViewLocation = new Vector2(_currentPosition, 0);
        }

        private void Cleanup()
        {
            _characterCount = 0;
        }


        private int _characterCount;
        private float _moveSpeed;
        private float _currentPosition;
        private bool _going;
    }
}