using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Tools
{
    public class DragForwarder: MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> Drag; 
        public event Action<PointerEventData> BeginDrag; 
        public event Action<PointerEventData> EndDrag; 
        
        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke(eventData);
        }
    }
}