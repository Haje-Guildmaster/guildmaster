using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Exploration.Events;
using GuildMaster.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// 탐색 과정을 총괄합니다.
    /// </summary>
    /// <note>
    /// 탐색을 순수하게 사건 위주로 인지하며 시간/그래픽을 직접적으로 다루지 않습니다.
    /// </note>
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private ExplorationView _explorationView;
        [SerializeField] private EventSeedCode _testEventSeed;

        public static ExplorationManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<ExplorationManager>());


        public void StartExploration(int length, List<Character> characters, ExplorationMap map)
        {
            _characters = new List<Character>(characters);
            _map = map;
            _explorationView.Setup(characters, map);

            RunExploration();

            async void RunExploration()
            {
                _currentNode = await SelectStartingBase();

                var eventSeed = EventSeedDatabase.Get(_testEventSeed).EventSeed;
                while (true)
                {
                    var testEvent = eventSeed.Generate(new System.Random());
                    var destination = await SelectNextDestination();
                    await _explorationView.PlayRoadView(_currentNode, destination, 0f, 0.5f);
                    await new EventProcessor(_explorationView, _characters.AsReadOnly(), _inventory).ProcessEvent(
                        testEvent); // EventProcessor에게 이벤트 처리 떠넘김. Todo: EventProcessor 재사용 고려.
                    await _explorationView.PlayRoadView(_currentNode, destination, 0.5f, 1f);

                    _currentNode = destination;
                }
            }
        }


        private async Task<MapNode> SelectStartingBase()
        {
            var ret = await _explorationView.SelectStartingBase();
            // Todo: Assert대신 에러처리로 바꾸기.(유저에게 에러메시지를 출력하고 돌아간다던지.)
            Assert.IsTrue(_map.Graph.Nodes.Contains(ret));
            Assert.IsTrue(ret.Content.Location.LocationType == Location.Type.Base);
            return ret;
        }

        private async Task<MapNode> SelectNextDestination()
        {
            var ret = await _explorationView.SelectNextDestination(_currentNode);
            // Todo: Assert대신 에러처리로 바꾸기.(유저에게 에러메시지를 출력하고 돌아간다던지.)
            Assert.IsTrue(_map.Graph.Nodes.Contains(ret));
            Assert.IsTrue(_currentNode.Connected.Exists(ind => _map.Graph.GetNode(ind) == ret));
            return ret;
        }

        private static ExplorationManager _instance;

        private List<Character> _characters;
        private ExplorationMap _map;
        private MapNode _currentNode;

        /// <summary>
        /// 이 탐색에 들고 나온 아이템들 인벤토리. // Todo: ExplorationLoader에서 탐색이 로드될 때 가져온 아이템 이곳으로 옮기기.
        /// </summary>
        private Inventory _inventory = new Inventory();
    }
}