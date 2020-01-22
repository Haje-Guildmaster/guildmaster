using System;
using GuildMaster.TownRoam;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.Tools
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class GenericButton<T>: MonoBehaviour
    {
        public class ClickedEvent : UnityEvent<T> { }
        public ClickedEvent clicked = new ClickedEvent();

        protected void OnMouseUpAsButton()
        {
            clicked.Invoke(EventParameter);
        }

        protected abstract T EventParameter { get; }

    }
}