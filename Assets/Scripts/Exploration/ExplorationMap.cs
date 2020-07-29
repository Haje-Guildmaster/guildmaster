using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색에서 맵 하나를 나타내는 데이터 클래스입니다.
    /// </summary>
    [Serializable]
    public class ExplorationMap
    {
        [SerializeField] private MapGraph _graph = new MapGraph();

        [Serializable]
        public class NodeContent
        {
            public LocationCode LocationCode;
            public Vector2 Position;
            public Location Location => ExplorationLocationDatabase.Get(LocationCode);
        }
        
        public Sprite Background;
        public Graph<NodeContent> Graph => _graph; 

        [Serializable]
        private class MapGraph : Graph<NodeContent> {}
    }
}