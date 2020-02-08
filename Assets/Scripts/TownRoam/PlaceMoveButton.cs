using System;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.TownRoam
{
    public class PlaceMoveButton: GenericButton<PlaceMoveButton>
    {
        protected override PlaceMoveButton EventArgument => this;
        public Place connectedPlace;
        
        // Draw a line to the connected place in the scene view. 
        private void OnDrawGizmos()
        {
            if (connectedPlace == null) return;
            // if (ReferenceEquals(connectedPlace, null)) return;
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