using System.Collections.Generic;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    public class MinimapView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private MinimapLocationSprite _locationSpritePrefab;
        [SerializeField] private SpriteRenderer _edgeSpritePrefab;
        [SerializeField] private Transform _playerLocationIndicatorObject;
        [SerializeField] private Transform _maskTransform;
        
        private void Awake()
        {
            _mapLoader = new MapLoader<MinimapLocationSprite>
            {
                BackgroundRenderer = _backgroundRenderer, DrawEdgeUsingSprite = true,
                EdgeSpritePrefab = _edgeSpritePrefab, NodeSpritePrefab = _locationSpritePrefab, MapScale = 1f
            };
        }

        public void LoadMap(ExplorationMap map)
        {
            Cleanup();

            _map = map;

            _locationSprites = _mapLoader.LoadMap(_map);
        }

        /// <summary>
        /// 미니맵 상에서 플레이어 위치를 표시하기 위해, 현재 플레이어가 어떤 경로를 이동하고 있는지 설정합니다.
        /// </summary>
        /// <param name="start"> 시작점 </param>
        /// <param name="end"> 종료점 </param>
        public void SetPlayerIndicatorPath(MapNode start, MapNode end)
        {
            Transform GetPos(MapNode node)
                => _locationSprites.Find(sprite => sprite.Node == node).transform;

            _path = (GetPos(start), GetPos(end));
            UpdatePlayerIndicator();
        }

        /// <summary>
        /// 미니맵 상에서 플레이어 위치를 표시하기 위해, 플레이어가 경로를 얼마나 진행했는지를 설정합니다. 
        /// </summary>
        /// <param name="progress"> 0~1 까지 </param>
        public void SetProgress(float progress)
        {
            _progress = progress;
            UpdatePlayerIndicator();
        }


        private void UpdatePlayerIndicator()
        {
            Assert.IsTrue(-0.00001 <= _progress && _progress <= 1.00001f);

            _playerLocationIndicatorObject.position =
                _path.start.position * (1 - _progress) + _path.end.position * _progress;

            _backgroundRenderer.transform.position +=
                (_maskTransform.transform.position - _playerLocationIndicatorObject.position);
        }

        private void Cleanup()
        {
            _map = null;
            if (_locationSprites != null)
            {
                foreach (var ls in _locationSprites)
                {
                    Destroy(ls.gameObject);
                }

                _locationSprites = null;
            }
        }

        private float _progress = 0f;
        private (Transform start, Transform end) _path;
        private ExplorationMap _map;
        private List<MinimapLocationSprite> _locationSprites;
        private MapLoader<MinimapLocationSprite> _mapLoader;
    }
}