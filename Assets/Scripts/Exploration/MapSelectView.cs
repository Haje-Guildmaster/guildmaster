using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// 지도를 보여주고 위치를 선택할 수 있는 함수를 제공하는 오브젝트.
    /// </summary>

    [RequireComponent(typeof(BasicMapView<LocationButton, Image>))]
    public class MapSelectView : MonoBehaviour
    {
        public Graph<ExplorationMap.NodeContent> Graph => _map.Graph;

        private void Awake()
        {
            _basicMapView = GetComponent<BasicMapView<LocationButton, Image>>();
        }

        public void LoadMap(ExplorationMap map)
        {
            _map = map;
            _basicMapView.LoadMap(map);

            foreach (var lb in _basicMapView.Nodes)
            {
                lb.Clicked += ProcessLocationButtonClick;
            }

            ColorLocationButtons(_ => (Color.white, Color.white, Color.white));
        }

        private void ProcessLocationButtonClick(LocationButton locationButton)
        {
            TrySelect(locationButton.Node);
        }


        private void TrySelect(MapNode node)
        {
            if (!_requestedSelect.HasValue) return;
            var (filter, callback) = _requestedSelect.Value;

            if (filter(node))
            {
                _requestedSelect = null; // 순서주의
                callback(node); // 순서주의
            }
        }


        [Pure]
        public delegate (Color normalColor, Color mouseOnColor, Color pressedColor) LocationButtonColorFunc(
            MapNode node);

        public void ColorLocationButtons(LocationButtonColorFunc colorFunc)
        {
            foreach (var lb in _basicMapView.Nodes)
            {
                lb.SetColor(colorFunc(lb.Node));
            }
        }

        public delegate bool LocationFilter(MapNode node);

        public void Select(LocationFilter filter, Action<MapNode> callback)
        {
            _requestedSelect?.callback.Invoke(null); // 이미 선택 중이었다면 원래 선택은 실패.

            _requestedSelect = (filter, callback);
        }

        
        private (LocationFilter filter, Action<MapNode> callback)? _requestedSelect;
        private BasicMapView<LocationButton, Image> _basicMapView;
        private ExplorationMap _map;
    }
}