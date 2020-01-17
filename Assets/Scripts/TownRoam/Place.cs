using System;
using System.Collections.Generic;
using UnityEngine;


namespace GuildMaster.TownRoam
{
    public class Place: MonoBehaviour
    {
        public Vector2 Size => backgroundRenderer.bounds.size;
        public Vector2 Center => backgroundRenderer.transform.position;
        // 일단 임시로 backgroundRenderer에서 모든 값을 가져오도록 했습니다.
        // 추후에 직접 값 입력도 할 수 있도록 해야 하지 않을까 합니다.
        
        public string placeName;
        public SpriteRenderer backgroundRenderer;
        
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}