using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.Tools
{
    /// <summary>
    /// unity ui의 버튼처럼 마우스를 올려놓았을 때, 클릭했을 때 색이 바뀌는 유의 버튼입니다.
    /// (이쪽 기능만 따로 분리해서 컴포넌트로 만들까 했지만 일단은 당장 필요가 없는 바 GenericButton을 상속하도록 두었습니다.
    /// 이 기능이 버튼과 분리되어야 하는 상황이라고 생각한다면 분리해 주세요)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ColorTintButton<T> : GenericButton<T>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SpriteRenderer _targetSprite;

        private void OnEnable()
        {
            _isUnderPointer = false;
            _pressed = false;

            UpdateColor();
        }

        public void SetColor((Color normalColor, Color mouseOnColor, Color pressedColor) colorSet)
        {
            (_normalColor, _mouseOnColor, _pressedColor) = colorSet;
            UpdateColor();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isUnderPointer = true;
            UpdateColor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isUnderPointer = false;
            UpdateColor();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _pressed = true;
            UpdateColor();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _pressed = false;
            UpdateColor();
        }

        private void UpdateColor()
        {
            Color newColor;
            if (_isUnderPointer)
                newColor = _pressed ? _pressedColor : _mouseOnColor;
            else newColor = _normalColor;

            _targetSprite.color = newColor;
        }


        [SerializeField] private Color _normalColor, _mouseOnColor, _pressedColor;
        private bool _pressed;
        private bool _isUnderPointer;
    }
}