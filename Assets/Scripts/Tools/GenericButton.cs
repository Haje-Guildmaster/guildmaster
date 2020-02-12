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
        public delegate void ClickedHandler(T param);

        public event ClickedHandler Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            Clicked?.Invoke(EventArgument);
        }


        protected abstract T EventArgument { get; }

    }
}