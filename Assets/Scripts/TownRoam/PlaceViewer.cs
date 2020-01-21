using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class PlaceViewer: MonoBehaviour
    {
        private const int PlaceViewedLayer = 8;
        
        private void Start()
        {
            _camera = GetComponent<Camera>();
        }
        
        private void Update()
        {
            // _currentPlace 오브젝트가 삭제되면 오류납니다. 삭제될 일이 잇다면 !=로 바꿔주세요.
            if (ReferenceEquals(_currentPlace, null)) return;
            // 화면 비율이 바뀌는 것을 걱정해 Update에 넣습니다. 화면 비율이 바뀌지 않는 게 확실하면 Goto에 넣어주세요.
            _camera.orthographicSize = Math.Max(_currentPlace.Size.y/2, _currentPlace.Size.x/2/_camera.aspect);
            transform.position = _currentPlace.Center;
        }
        public void Goto(Place p)
        {
            // 현재는 카메라를 이동시키며 그 Place를 활성화시키는 방법을 사용하고 있으나,
            // 그 장소를 복제하여 그곳을 비추는 것도 괜찮아 보입니다.    
            if (p == null) throw new Exception("PlaceViewer cannot Goto null");
            
            // 전에 있던 장소 비활성화&현재 장소 활성화.
            if (_currentPlace != null)
                SetLayersOfChildren(_currentPlace.gameObject, _originalLayer);         
            _currentPlace = p;

            var gObj = p.gameObject;
            _originalLayer = gObj.layer;
            SetLayersOfChildren(gObj, PlaceViewedLayer);
            
            SubscribeMoveButtons(gObj);
        }

        
        /*
         * 현재 장소의 모든 PlaceMoveButtons의 onclick이벤트에 listener 부착.
         */
        private void SubscribeMoveButtons(GameObject parentObject)
        {
            _subscribedButtonClickEvents.ForEach(e=> e.RemoveListener(OnPlaceMoveButtonClicked));
            _subscribedButtonClickEvents.Clear();
            foreach (var button in parentObject.GetComponentsInChildren<PlaceMoveButton>())
            {
                button.clicked.AddListener(OnPlaceMoveButtonClicked);
                _subscribedButtonClickEvents.Add(button.clicked);
            }
        }
        private void OnPlaceMoveButtonClicked(PlaceMoveButton button)
        {
            Goto(button.connectedPlace);
        }

        private void SetLayersOfChildren(GameObject gObj, int layer)
        {
            foreach (var t in gObj.GetComponentsInChildren<Transform>())
                t.gameObject.layer = layer;
        }
        private int _originalLayer;
        private Place _currentPlace;
        private readonly List<PlaceMoveButton.ClickedEvent> _subscribedButtonClickEvents = new List<PlaceMoveButton.ClickedEvent>();
        private Camera _camera;
    }
}