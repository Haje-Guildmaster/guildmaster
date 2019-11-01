using System;
using UnityEngine;
using UnityEngine.Events;

// 현재 사용되지 않습니다.

namespace GuildMaster.MapRoam { 
   [Obsolete("CheckClick is deprecated")]
    // 클릭되었는지 확인. 현재는 그냥 누르면 바로 이벤트가 발생하나 아마 누로고 뗄 때 이벤트가 나도록 수정해야 하지 않을까 함.
    public class CheckClick : MonoBehaviour {
        public UnityEvent clicked; 
        private void OnMouseDown() {
            clicked.Invoke();
        }
    }
}