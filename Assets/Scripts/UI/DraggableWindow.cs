using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace GuildMaster.UI
{
    public abstract class DraggableWindow : Window, IDragHandler, IBeginDragHandler
    {
        private Vector3 _dragBeginCursorPos;
        private Vector3 _dragBeginWindowPos;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragBeginCursorPos = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
            _dragBeginWindowPos = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = _dragBeginWindowPos
                                 + eventData.pressEventCamera.ScreenToWorldPoint(eventData.position) - _dragBeginCursorPos;
        }
    }
}