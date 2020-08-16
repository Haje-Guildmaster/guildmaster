using System.Collections.Generic;
using System.Linq;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    [RequireComponent(typeof(BasicMapView<BasicMapLocationSprite, Image>))]
    public class MinimapView : MonoBehaviour
    {
        [SerializeField] private Transform _playerLocationIndicatorObject;

        private void Awake()
        {
            _basicMapView = GetComponent<BasicMapView<BasicMapLocationSprite, Image>>();
        }

        public void LoadMap(ExplorationMap map)
        {
            _basicMapView.LoadMap(map);
        }

        /// <summary>
        /// 미니맵 상에서 플레이어 위치를 표시하기 위해, 현재 플레이어가 어떤 경로를 이동하고 있는지 설정합니다.
        /// </summary>
        /// <param name="start"> 시작점 </param>
        /// <param name="end"> 종료점 </param>
        public void SetPlayerIndicatorPath(MapNode start, MapNode end)
        {
            Transform GetPos(MapNode node)
                => _basicMapView.Nodes.First(sprite => sprite.Node == node).transform;

            _path = (GetPos(start), GetPos(end));        // Todo: 조건을 만족하는 nodeSprite가 없을 땐 어떻게 행동할지.
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

            _basicMapView.BackgroundImage.transform.position +=
                (_basicMapView.Mask.transform.position - _playerLocationIndicatorObject.position);
        }


        private float _progress = 0f;
        private (Transform start, Transform end) _path;
        private BasicMapView<BasicMapLocationSprite, Image> _basicMapView;
    }
}