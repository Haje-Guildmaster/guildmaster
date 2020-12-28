using System;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /*
     * SlideBackgroundView의 부속품 역할을 수행합니다.
     */
    public class SlideBackgroundElement: MonoBehaviour
    {
        public float MoveRatio;
        public Vector3 InitialPosition { get; private set;}
        
        private void Start()
        {
            InitialPosition = transform.position;

            _parentView = GetComponentInParent<SlideBackgroundView>();
            if (_parentView == null)
                throw new Exception($"{nameof(SlideBackgroundElement)} needs to have a parent {nameof(SlideBackgroundView)}");
        }

        private SlideBackgroundView _parentView;
    }
}