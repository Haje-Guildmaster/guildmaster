using System;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.TownRoam
{
    [RequireComponent(typeof(Button))]
    public class RoomMoveButton: MonoBehaviour
    {
        [SerializeField] private Room connectedRoom;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        // Draw a line to the connected place in the scene view. 
        private void OnDrawGizmos()
        {
            if (connectedRoom == null) return;
            // if (ReferenceEquals(connectedPlace, null)) return;
            Gizmos.color = Color.cyan;
            Vector3 lineEnd = connectedRoom.Center;
            var here = GetComponentInParent<Room>();
            foreach (var btn in connectedRoom.GetComponentsInChildren<RoomMoveButton>())
            {
                if (btn.connectedRoom == here)
                {
                    lineEnd = btn.transform.position;
                    break;
                }
            }

            Gizmos.DrawLine(transform.position, lineEnd);
        }

        private void OnClick()
        {
            TownRoamManager.Instance.GotoRoom(connectedRoom);
        }
        
        private Button _button;
    }
}