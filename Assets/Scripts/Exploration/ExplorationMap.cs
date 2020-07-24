using System;
using GuildMaster.Databases;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// 탐색에서 맵 하나를 나타내는 데이터 클래스입니다.
    /// </summary>
    public class ExplorationMap
    {
        public class NodeContent
        {
            public LocationCode LocationCode;
            public float X, Y;
        }
        public Sprite MapBackground;
        public Graph<NodeContent> Graph = new Graph<NodeContent>();
    }
}