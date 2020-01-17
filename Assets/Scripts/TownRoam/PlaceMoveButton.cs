using System;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.TownRoam
{
    [RequireComponent(typeof(Collider2D))]
    public class PlaceMoveButton: MonoBehaviour
    {
        public class ClickedEvent : UnityEvent<PlaceMoveButton> { }
        public Place connectedPlace;
        public ClickedEvent clicked = new ClickedEvent();
        private void OnMouseUpAsButton()
        {
            clicked.Invoke(this);
        }
    }
}