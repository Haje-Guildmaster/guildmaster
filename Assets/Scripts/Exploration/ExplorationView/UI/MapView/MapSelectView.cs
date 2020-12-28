using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// 지도를 보여주고 위치를 선택할 수 있도록 하는 유니티 오브젝트.
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
        public async Task<MapNode> Select(LocationFilter filter, CancellationToken cancelToken = default)
        {
            _requestedSelect?.taskCompletionSource.SetResult(null);; // 이미 선택 중이었다면 원래 선택은 실패.

            var nodeSelectCompletionSource = new TaskCompletionSource<MapNode>();
            _requestedSelect = (filter, nodeSelectCompletionSource);
            
            using (cancelToken.Register(() => {
                nodeSelectCompletionSource.TrySetCanceled();
            })) {
                return await nodeSelectCompletionSource.Task;
            }
        }
        
        private void ProcessLocationButtonClick(LocationButton locationButton)
        {
            TrySelect(locationButton.Node);
        }
        
        private void TrySelect(MapNode node)
        {
            if (!_requestedSelect.HasValue) return;
            var (filter, tcs) = _requestedSelect.Value;

            if (filter(node))
            {
                _requestedSelect = null; // 순서주의
                tcs.TrySetResult(node);
            }
        }

        private (LocationFilter filter, TaskCompletionSource<MapNode> taskCompletionSource)? _requestedSelect;
        private BasicMapView<LocationButton, Image> _basicMapView;
        private ExplorationMap _map;
    }
}