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
        
        // Draw a line to the connected place in the scene view. 
        private void OnDrawGizmos()
        {
            if (ReferenceEquals(connectedPlace, null)) return;
            Gizmos.color = Color.cyan;
            Vector3 lineEnd = connectedPlace.Center;
            var here = GetComponentInParent<Place>();
            foreach (var btn in connectedPlace.GetComponentsInChildren<PlaceMoveButton>())
            {
                if (btn.connectedPlace == here)
                {
                    lineEnd = btn.transform.position;
                    break;
                }
            }

            Gizmos.DrawLine(transform.position, lineEnd);
        }
    }
}