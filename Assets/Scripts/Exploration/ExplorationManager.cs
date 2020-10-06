using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Databases;
using GuildMaster.Exploration.Events;
using GuildMaster.Tools;
using GuildMaster.TownRoam.TownLoad;
using GuildMaster.TownRoam.Towns;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// 탐색 과정을 총괄합니다.
    /// </summary>
    /// <note>
    /// 탐색을 순수하게 사건 위주로 인지하며 시간/그래픽을 직접적으로 다루지 않습니다(Text adventure).
    /// ExplorationView가 올바른 값을 줄 것이라고 믿지 않습니다.
    /// </note>
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private ExplorationView _explorationView;
        [SerializeField] private EventSeedCode _testEventSeed;

        public static ExplorationManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<ExplorationManager>());

        /// <summary>
        /// 탐색 과정을 시작합니다.
        /// </summary>
        /// <param name="characters"> 탐색에 참여하는 캐릭터들 </param>
        /// <param name="map"> 탐색하는 맵 데이타 </param>
        public void StartExploration(List<Character> characters, ExplorationMap map)
        {
            _characters = new List<Character>(characters);
            _map = map;
            _explorationView.Setup(characters, map);
            _log = new ExplorationLog();

            RunExploration();
            
            async void RunExploration()
            {
                // 처음 시작 지점 선택
                var (endInBaseSelection, startingBase) = await SelectStartingBase();
                _currentNode = startingBase;

                if (!endInBaseSelection)
                {
                    var eventSeed = EventSeedDatabase.Get(_testEventSeed).EventSeed;
                    while (true)
                    {
                        // 다음 목적지 선택.
                        var (end, destination) = await SelectNextDestination();
                        if (end) break;

                        // 이동
                        await _explorationView.PlayRoadView(_currentNode, destination, 0f, 1f);
                        _currentNode = destination;

                        // 이벤트 발생
                        var testEvent = eventSeed.Generate(new System.Random());
                        await new EventProcessor(_explorationView, _characters.AsReadOnly(), _inventory, testEvent, _log)
                            .Run(); // EventProcessor에게 이벤트 처리 떠넘김. Todo: EventProcessor 재사용 고려.
                    }
                }

                // 탐색 종료.
                EndExploration();
                async void EndExploration()
                {
                    await _explorationView.ReportExplorationResults(_log);
                    TownLoadManager.LoadTownScene(TownRefs.TestTown);
                }
            }
        }


        /// <summary>
        /// 시작 지점을 선택합니다.
        /// </summary>
        /// <returns> (탐색을 끝내는 지의 여부, 선택한 시작 지점) 튜플 </returns>
        private async Task<(bool endExploration, MapNode startingBase)> SelectStartingBase()
        {
            var (endExploration, startingBase) = await _explorationView.SelectStartingBase();
            // Todo: Assert대신 에러처리로 바꾸기.(유저에게 에러메시지를 출력하고 돌아간다던지.)
            if (!endExploration)
            {
                Assert.IsTrue(_map.Graph.Nodes.Contains(startingBase));
                Assert.IsTrue(startingBase.Content.Location.LocationType == Location.Type.Base);
            }

            return (endExploration, startingBase);
        }

        /// <summary>
        /// 다음 목적지를 선택합니다.
        /// </summary>
        /// <returns> (탐색을 끝내는 지의 여부, 선택한 시작 지점) 튜플 </returns>
        private async Task<(bool endExploration, MapNode destination)> SelectNextDestination()
        {
            var allowEnd = _currentNode.Content.Location.LocationType == Location.Type.Base;
            var (endExploration, destination) = await _explorationView.SelectNextDestination(_currentNode, allowEnd);

            if (!endExploration)
            {
                // Todo: Assert대신 에러처리로 바꾸기.(유저에게 에러메시지를 출력하고 돌아간다던지.)
                Assert.IsTrue(_map.Graph.Nodes.Contains(destination));
                Assert.IsTrue(_currentNode.Connected.Exists(ind => _map.Graph.GetNode(ind) == destination));
            }

            return (endExploration, destination);
        }

        private static ExplorationManager _instance;

        private ExplorationLog _log;
        private List<Character> _characters;
        private ExplorationMap _map;
        private MapNode _currentNode;

        /// <summary>
        /// 이 탐색에 들고 나온 아이템들 인벤토리. // Todo: ExplorationLoader에서 탐색이 로드될 때 가져온 아이템 이곳으로 옮기기.
        /// </summary>
        private Inventory _inventory;
    }
}