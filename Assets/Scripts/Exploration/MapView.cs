using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.XR;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// 지도를 보여주고 위치를 선택할 수 있는 함수를 제공하는 오브젝트.
    /// </summary>
    public class MapView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private LocationButton _locationButtonPrefab;
        [SerializeField] private bool _drawEdge;
        [SerializeField] private LineRenderer _edgeRendererPrefab;

        public Graph<ExplorationMap.NodeContent> Graph => _map.Graph;

        private void Awake()
        {
            _mapLoader = new MapLoader<LocationButton>
            {
                BackgroundRenderer = _backgroundRenderer, DrawEdgeUsingEdgeRenderer = _drawEdge,
                EdgeRendererPrefab = _edgeRendererPrefab, NodeSpritePrefab = _locationButtonPrefab, MapScale = 1f
            };
        }

        public void LoadMap(ExplorationMap map)
        {
            Cleanup();

            _map = map;

            _locationButtons = _mapLoader.LoadMap(_map);
            foreach (var lb in _locationButtons)
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
            foreach (var lb in _locationButtons)
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

        private void Cleanup()
        {
            _map = null;
            if (_locationButtons != null)
            {
                foreach (var lb in _locationButtons)
                {
                    // lb.Clicked -= ProcessLocationButtonClick;
                    Destroy(lb.gameObject);
                }

                _locationButtons = null;
            }
        }

        private MapLoader<LocationButton> _mapLoader;
        private (LocationFilter filter, Action<MapNode> callback)? _requestedSelect;

        private ExplorationMap _map;
        private List<LocationButton> _locationButtons;
    }
}