using System;
using System.Collections.Generic;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    public class MapView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private LocationButton _locationButtonPrefab;

        public void LoadMap(ExplorationMap map)
        {
            Cleanup();

            _map = map;
            _backgroundRenderer.sprite = _map.Background;

            foreach (var node in map.Graph.Nodes)
            {
                var newButton = Instantiate(_locationButtonPrefab, _backgroundRenderer.transform);
                newButton.transform.localPosition = node.Content.Position * 1f;
                newButton.SetNode(node);
                newButton.Clicked += ProcessLocationButtonClick;
                _locationButtons.Add(newButton);
            }
            
            SetHighlightFunc(_ => Color.white);
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
                _requestedSelect = null;  // 순서주의
                callback(node);           // 순서주의
            }
        }


        public delegate Color LocationButtonColorFunc(LocationButton node);
        public void SetHighlightFunc(LocationButtonColorFunc colorFunc)
        {
            foreach (var lb in _locationButtons)
            {
                lb.Highlight(colorFunc(lb));
            }
        }

        public delegate bool LocationFilter(MapNode node);

        public void Select(LocationFilter filter, Action<MapNode> callback)
        {
            _requestedSelect?.callback.Invoke(null);     // 이미 선택 중이었다면 원래 선택은 실패.

            _requestedSelect = (filter, callback);
        }

        private void Cleanup()
        {
            _map = null;
            foreach (var lb in _locationButtons)
            {
                // lb.Clicked -= ProcessLocationButtonClick;
                Destroy(lb.gameObject);
            }

            _locationButtons.Clear();
        }
        
        private (LocationFilter filter, Action<MapNode> callback)? _requestedSelect;

        private ExplorationMap _map;
        private readonly List<LocationButton> _locationButtons = new List<LocationButton>();
    }
}