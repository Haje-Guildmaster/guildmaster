using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class PlaceViewer: MonoBehaviour
    {
        [SerializeField] private Place startPlace;
        
        private void Start()
        {
            _camera = GetComponent<Camera>();
            Goto(startPlace);
        }

        
        private void Update()
        {
            if (ReferenceEquals(_currentPlace, null)) return;
            // 화면 비율이 바뀌는 것을 걱정해 Update에 넣습니다. 화면 비율이 바뀌지 않는 게 확실하면 Goto에 넣어주세요.
            _camera.orthographicSize = Math.Max(_currentPlace.Size.y/2, _currentPlace.Size.x/2/_camera.aspect);
            transform.position = _currentPlace.Center;
        }
        public void Goto(Place p)
        {
            _currentPlace = p;
            if (p == null) return;

            SubscribeMoveButtons(p.gameObject);
        }


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

        private Place _currentPlace;
        private readonly List<PlaceMoveButton.ClickedEvent> _subscribedButtonClickEvents = new List<PlaceMoveButton.ClickedEvent>();
        private Camera _camera;
    }
}