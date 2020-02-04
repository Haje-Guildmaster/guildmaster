using System;
using GuildMaster.TownRoam;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GuildMaster.Tools
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class GenericButton<T>: MonoBehaviour, IPointerClickHandler
    {
        public class ClickedEvent : UnityEvent<T> { }
        public ClickedEvent clicked = new ClickedEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked.Invoke(EventParameter);
        }


        protected abstract T EventParameter { get; }

    }
}