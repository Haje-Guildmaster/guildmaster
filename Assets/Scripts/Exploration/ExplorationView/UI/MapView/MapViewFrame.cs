using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 모든 맵 뷰의 공통된 부분을 묶어둔 컴포넌트. 데이터 클래스.
    /// </summary>
    public class MapViewFrame : MonoBehaviour
    {
        public Image BackgroundImage;
        public Mask Mask;
        public Transform NodesParent;
        public Transform EdgesParent;
    }
}