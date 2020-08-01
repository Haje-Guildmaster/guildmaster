using System.Collections.Generic;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    /// <summary>
    /// 탐색 과정을 총괄합니다.
    /// 탐색을 순수하게 사건 위주로 인지하며 시간/그래픽을 직접적으로 다루지 않습니다.
    /// </summary>
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private ExplorationView _explorationView;

        public static ExplorationManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<ExplorationManager>());
        
        
        public void StartExploration(int length, List<Character> characters, ExplorationMap map)
        {
            _characters = new List<Character>(characters);
            
            _map = map;
            
            _explorationView.Setup(characters, map);
            _explorationView.SelectStartingBase(node =>
            {
                // Todo: Assert대신 에러처리로 바꾸기.(유저에게 에러메시지를 출력하고 돌아간다던지.)
                Assert.IsTrue(node.Content.Location.LocationType == Location.Type.Base);
                _currentNode = node;
                HeadOutToNextLocation();
            });
        }

        /// <summary>
        /// 다음 목적지를 선택하고 그곳으로 이동.
        /// </summary>
        private void HeadOutToNextLocation()
        {
            _explorationView.SelectNextDestination(_currentNode,
                destination =>
                {
                    // Todo: Assert대신 에러처리로 바꾸기.(유저에게 에러메시지를 출력하고 돌아간다던지.)
                    Assert.IsTrue(_currentNode.Connected.Exists(ind => _map.Graph.GetNode(ind) == destination));
                    _explorationView.StartRoadView(_characters, _currentNode, destination, () =>
                    {
                        _currentNode = destination;
                        HeadOutToNextLocation();
                    });
                });
        }


        private static ExplorationManager _instance;

        private List<Character> _characters;
        private ExplorationMap _map;
        private MapNode _currentNode;
    }
}