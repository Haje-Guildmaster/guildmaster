using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace GuildMaster.Tools
{
    /// <summary>
    ///  여러 그래프를 나타내기 위한 제너릭 클래스입니다.
    /// </summary>
    /// <typeparam name="TNodeContent"> 노드가 소유할 내용의 타입</typeparam>
    [Serializable]
    public class Graph<TNodeContent>
    {
        [Serializable]
        public class Node: InnerNode
        {
        }

        [Serializable]
        public class InnerNode
        {
            public TNodeContent Content => _content;

            [SerializeField] private TNodeContent _content;
            public List<int> Connected = new List<int>();
        }

        public Node GetNode(int nodeIndex)
        {
            var got = _nodes.ElementAtOrDefault(nodeIndex);
            return got;
        }

        public int NodeCount => _nodes.Count;
        public IEnumerable<Node> Nodes => _nodes;

        [SerializeField] private List<Node> _nodes = new List<Node>();
    }
}