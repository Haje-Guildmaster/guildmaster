using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Tools
{
    /// <summary>
    /// 클릭당했을 시 OnClick함수를 호출합니다.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class ClickableComponent: MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            OnClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // parent 오브젝트에 IpointerDownHandler가 있으면 OnPointerClick이 불리지 않는 문제를 해결하기 위해.
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        protected abstract void OnClick();
    }
}