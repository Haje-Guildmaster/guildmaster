using System;
using System.Collections.Generic;
using GuildMaster.TownRoam.Towns;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Assertions;


namespace GuildMaster.TownRoam
{
    public class Place : MonoBehaviour
    {
        public Vector2 Size => backgroundRenderer.bounds.size;

        public Vector2 Center => backgroundRenderer.transform.position;
        // 일단 임시로 backgroundRenderer에서 모든 값을 가져오도록 했습니다.
        // 추후에 직접 값 입력도 할 수 있도록 해야 하지 않을까 합니다.

        public string placeName;
        public SpriteRenderer backgroundRenderer;

        public void NotifyView(bool viewIn)
        {
            if (viewIn)
            {
                _viewCount++;
                if (_viewCount == 1) _StartBeingViewed();
            }
            else
            {
                _viewCount--;
                if (_viewCount == 0) _StopBeingViewed();
            }

            Assert.IsTrue(_viewCount >= 0);
        }

        private void _StartBeingViewed()
        {
            foreach (var aopv in GetComponentsInChildren<ActivateOnPlaceViewed>())
            {
                aopv.gameObject.SetActive(true);
            }
        }

        private void _StopBeingViewed()
        {
            foreach (var aopv in GetComponentsInChildren<ActivateOnPlaceViewed>())
            {
                aopv.gameObject.SetActive(false);
            }
        }

        private int _viewCount = 0;
    }
}