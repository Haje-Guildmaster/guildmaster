using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    using Weighteditem = ProbabilityTool<EventSeedCode>.Weighteditem;

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
        //발생 가능한 EventSeedCode를 리스트로 받습니다.
        [SerializeField] private List<Weighteditem> _testEventSeed;
        [SerializeField] private EventSeedCode _defaultEventSeed;


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
                    
                    while (true)
                    {
                        // 다음 목적지 선택.
                        var (end, destination) = await SelectNextDestination();
                        if (end) break;

                        // 이동
                        await _explorationView.PlayRoadView(_currentNode, destination);
                        _currentNode = destination;

                        // 현재 Node에서 이벤트 풀 정보를 가져오고 처리
                        List<Weighteditem> weightedSeedList = _currentNode.Content._eventSeedCodeList.ToList();
                        // 100을 전체 비중의 한계치로 두고, 남는 비중을 주어진 _defaultEventSeed를 이용해 채웁니다.(_defaultEventSeed는 '이벤트 없음'의 EventSeedCode로 가정)
                        int weightSum = 0;
                        foreach (Weighteditem item in weightedSeedList)
                        {
                            weightSum += item.Weight;
                        }
                        int weightLeft = 100 - weightSum;
                        if (weightLeft > 0)
                        {
                            Weighteditem defaultItem = new Weighteditem(_defaultEventSeed, weightLeft);
                            weightedSeedList.Add(defaultItem);
                        }

                        //위에서 생성된 weightedSeedList를 인자로 ProbabilityTool 오브젝트를 만듭니다.
                        ProbabilityTool<EventSeedCode> seedChoiceTool = new ProbabilityTool<EventSeedCode>(weightedSeedList);

                        // 이벤트 발생
                        // 확률적으로 택해진 EventSeedCode를 담을 변수 생성
                        EventSeedCode chosenEventSeedCode = new EventSeedCode();

                        chosenEventSeedCode = seedChoiceTool.Getitem().item;
                        var eventSeed = EventSeedDatabase.Get(chosenEventSeedCode).EventSeed;
                        var testEvent = eventSeed.Generate(new System.Random());
                        await new EventProcessor(_explorationView, _characters.AsReadOnly(), _inventory, testEvent,
                                _log)
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
        /// 탐색을 종료할 수 있을 시 탐색을 종료합니다. 현재 탐색을 종료할 수 있는 순간은 처음 시작 Base를 고르는 동안과 Base에서
        /// 다음 목적지를 정하는 동안뿐입니다.
        /// 탐색을 종료할 수 없을 때 이 함수가 불리면 오류를 던집니다.
        /// </summary>
        /// <note>
        /// 수동적인 함수입니다. signal을 보내고 누군가 처리해주길 바라기만 합니다. 능동적으로 바꿔볼까 생각도 해보았지만
        /// 노력에 비해 얻는 게 거의 없을 거라 판단하였습니다. 
        /// </note>
        public void RequestExit()
        {
            if (!_exitRequestWaiter.WaiterExist)
                throw new InvalidOperationException($"현재 상황에서 탐색을 종료시킬 수 없습니다.");
            Assert.IsTrue(_exitRequestWaiter.WaitersCount == 1);
            _exitRequestWaiter.Signal();
        }


        /// <summary>
        /// 시작 지점을 선택합니다. 선택되기 전 RequestExit이 불리면 즉시 함수를 종료하며 반환값에 이를 표시합니다.
        /// </summary>
        /// <returns> (탐색을 끝내는 지의 여부, 선택한 시작 지점) 튜플 </returns>
        private async Task<(bool endExploration, MapNode startingBase)> SelectStartingBase()
        {
            var cancelTokenSource = new CancellationTokenSource();

            async Task<(bool, MapNode)> Select() =>
                (false, await _explorationView.SelectStartingBase(cancelTokenSource.Token));

            async Task<(bool, MapNode)> End()
            {
                await _exitRequestWaiter.Wait(cancelTokenSource.Token);
                return (true, null);
            }

            var (endExploration, startingBase) = await new TaskFactory().ContinueWhenAny(new[]
            {
                Select(),
                End(),
            }, t => t.Result);
            cancelTokenSource.Cancel();


            if (!endExploration)
            {
                Assert.IsTrue(_map.Graph.Nodes.Contains(startingBase));
                Assert.IsTrue(startingBase.Content.Location.LocationType == Location.Type.Base);
            }

            return (endExploration, startingBase);
        }

        /// <summary>
        /// 다음 목적지를 선택합니다. 선택되기 전 RequestExit이 불리면 즉시 함수를 종료하며 반환값에 이를 표시합니다.
        /// </summary>
        /// <returns> (탐색을 끝내는 지의 여부, 선택한 시작 지점) 튜플 </returns>
        private async Task<(bool endExploration, MapNode destination)> SelectNextDestination()
        {
            var allowEnd = _currentNode.Content.Location.LocationType == Location.Type.Base;

            var cancelTokenSource = new CancellationTokenSource();

            async Task<(bool, MapNode)> Select() =>
                (false, await _explorationView.SelectNextDestination(_currentNode, allowEnd, cancelTokenSource.Token));

            async Task<(bool, MapNode)> End()
            {
                await _exitRequestWaiter.Wait(cancelTokenSource.Token);
                return (true, null);
            }

            var taskList = new List<Task<(bool, MapNode)>>{Select()};
            if (allowEnd) taskList.Add(End());

            var (endExploration, destination) =
                await new TaskFactory().ContinueWhenAny(taskList.ToArray(), t => t.Result);
            cancelTokenSource.Cancel();

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

        private readonly SignalWaiter _exitRequestWaiter = new SignalWaiter();

        /// <summary>
        /// 이 탐색에 들고 나온 아이템들 인벤토리. // Todo: ExplorationLoader에서 탐색이 로드될 때 가져온 아이템 이곳으로 옮기기.
        /// </summary>
        private Inventory _inventory;
    }
}