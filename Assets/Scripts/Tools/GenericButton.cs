using System;
using GuildMaster.TownRoam;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GuildMaster.Tools
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class GenericButton<T>: MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void ClickedHandler(T param);

        public event ClickedHandler Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            Clicked?.Invoke(EventArgument);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // parent 오브젝트에 IpointerDownHandler가 있으면 OnPointerClick이 불리지 않는 문제를 해결하기 위해.
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        protected abstract T EventArgument { get; }

    }
}