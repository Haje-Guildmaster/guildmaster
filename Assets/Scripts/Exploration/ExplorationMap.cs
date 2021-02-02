using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;
using GuildMaster.Exploration.Events;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색에서 맵 하나를 나타내는 데이터 클래스입니다.
    /// </summary>
    [Serializable]
    public class ExplorationMap
    {
        [SerializeField] private Graph<NodeContent> _graph;
        
        
        [Serializable]
        public class NodeContent
        {
            public LocationCode LocationCode;
            public Vector2 Position;
            public Location Location => ExplorationLocationDatabase.Get(LocationCode);
            //해당 Node에서 일어날 수 있는 이벤트 풀을 정수 비중과 함께 담기 위해서.
            [SerializeField] public List<ProbabilityTool<EventSeedCode>.Weighteditem> _eventSeedCodeList;
        }
        
        public Sprite Background;
        public Graph<NodeContent> Graph => _graph;
    }
}