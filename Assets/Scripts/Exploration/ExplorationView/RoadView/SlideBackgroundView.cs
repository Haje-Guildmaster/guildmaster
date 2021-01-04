using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /*
     *  배경이 움직일 때, 뒤에 있는 배경 레이어일수록 느리게 움직이는 효과를 주기 위해 사용합니다.
     */
    public class SlideBackgroundView: MonoBehaviour
    {
        public Vector2 CurrentViewLocation
        {
            get => _currentViewLocation;
            set
            {
                _currentViewLocation = value;
                UpdateChildElements();
            }
        }
        

        public void Move(Vector2 distance)
        {
            CurrentViewLocation += distance;
        }
        

        private void UpdateChildElements()
        {
            foreach (var el in _elements)
            {
                el.transform.position = el.InitialPosition - (Vector3) CurrentViewLocation * el.MoveRatio;
            }
        }

        private void Start()
        {
            _elements = GetComponentsInChildren<SlideBackgroundElement>().ToList();
        }

        private Vector2 _currentViewLocation;
        private List<SlideBackgroundElement> _elements;
    }
}