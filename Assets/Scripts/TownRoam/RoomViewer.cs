using System;
using System.Collections.Generic;
using GuildMaster.Databases;
using GuildMaster.Tools;
using GuildMaster.Windows;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// Room 하나에 카메라를 맞추어 Room을 비춥니다. 현재 비추고 있는 Room만 레이어가 RoomBeingViewedLayer로 변경됩니다.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class RoomViewer : MonoBehaviour
    {
        private const int RoomBeingViewedLayer = 8;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnDisable()
        {
            Focus(null);
        }

        private void Update()
        {
            // if (_currentPlace == null) return;
            if (ReferenceEquals(_focusingRoom, null)) return;

            // Todo: 화면 비율이 바뀌는 것을 걱정해 Update에 넣습니다. 화면 비율이 바뀌지 않는 게 확실하면 Focus에 넣어주세요.
            _camera.orthographicSize = Math.Max(_focusingRoom.Size.y / 2, _focusingRoom.Size.x / 2 / _camera.aspect);
            transform.position = _focusingRoom.Center;
        }

        public void Focus(Room room)
        {
            // 전에 있던 Room 원래 레이어로 보냄.
            if (_focusingRoom != null)
            {
                SetLayersOfChildren(_focusingRoom.gameObject, _originalLayer);
            }

            _focusingRoom = room;

            if (room == null) return;


            var gObj = room.gameObject;
            _originalLayer = gObj.layer;
            SetLayersOfChildren(gObj, RoomBeingViewedLayer);
        }


        private void SetLayersOfChildren(GameObject gObj, int layer)
        {
            foreach (var t in gObj.GetComponentsInChildren<Transform>())
                t.gameObject.layer = layer;
        }

        private int _originalLayer;
        private Room _focusingRoom;
        private Camera _camera;
    }
}