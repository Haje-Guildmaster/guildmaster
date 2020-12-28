using System.Net.Sockets;
using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색 씬에서 이동하고 있는 캐릭터 한 명의 모습을 나타내는 오브젝트.
    /// 클릭당했을 경우 Clicked이벤트로 자기 자신을 반환한다.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharacterSprite : GenericButton<CharacterSprite>
    {
        protected override CharacterSprite EventArgument => this;

        public Character Character
        {
            get => _character;
            set
            {
                _character = value;
                UpdateAppearance();
            }
        }

        public void Goto(float xPosition)
        {
            var lp = transform.localPosition;
            lp.x = _initialLocalPosition.x + xPosition;
            transform.localPosition = lp;
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

        private void Awake()
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
            _ySpeed -= 0.01f;

            if (locPos.y < _initialLocalPosition.y)
            {
                locPos.y = _initialLocalPosition.y;
                _ySpeed = 0.02f;
            }

            transform.localPosition = locPos;
        }

        private void UpdateAppearance()
        {
            _renderer.sprite = _character?.StaticData.BasicData.Illustration;
        }

        private float _ySpeed;

        private Character _character;
        private bool _isMoving;
        private Vector3 _initialLocalPosition;
        private SpriteRenderer _renderer;
    }
}