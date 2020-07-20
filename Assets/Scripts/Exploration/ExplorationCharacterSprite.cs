using System.Net.Sockets;
using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /*
     * 탐색 씬에서 이동하고 있는 캐릭터 한 명의 모습을 나타냅니다.
     */
    [RequireComponent(typeof(SpriteRenderer))]
    public class ExplorationCharacterSprite : GenericButton<ExplorationCharacterSprite>
    {
        protected override ExplorationCharacterSprite EventArgument => this;
        public Character Character { get; private set; }

        public void SetCharacter(Character character)
        {
            Character = character;
            _renderer.sprite = Character.StaticData.basicData.illustration;
        }

        public void Goto(float xPosition)
        {
            transform.localPosition = _initialLocalPosition + new Vector3(xPosition, 0, 0);
        }

        public void SetMoveAnimation(bool value)
        {
            _isMoving = value;

            if (!_isMoving)
            {
                var trs = transform;
                var pos = trs.localPosition;
                pos.y = _initialLocalPosition.y;
                trs.localPosition = pos;
            }
        }

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _initialLocalPosition = transform.localPosition;
        }

        private void Update()
        {
            // 임시로 점프 시켜봄
            // Todo:
            if (!_isMoving) return;
            var locPos = transform.localPosition;

            locPos.y += _ySpeed;
            _ySpeed -= 0.0006f;

            if (locPos.y < _initialLocalPosition.y)
            {
                locPos.y = _initialLocalPosition.y;
                _ySpeed = 0.02f;
            }

            transform.localPosition = locPos;
        }

        private float _ySpeed;

        private bool _isMoving;
        private Vector3 _initialLocalPosition;
        private SpriteRenderer _renderer;
    }
}