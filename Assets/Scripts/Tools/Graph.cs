using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace GuildMaster.Tools
{
    /// <summary>
    ///  여러 그래프를 나타내기 위한 제너릭 클래스입니다.
    /// </summary>
    /// <typeparam name="TNodeContent"> 노드가 소유할 내용의 타입</typeparam>
    public class Graph<TNodeContent>
    {
        public class Node
        {
            public Node(int nodeIndex)
            {
                NodeIndex = nodeIndex;
            }

            public readonly int NodeIndex;
            public TNodeContent Content { get; private set; }
            public List<Node> Connected = new List<Node>();
        }

        public Node AddNode(TNodeContent content)
        {
            var index = _nodes.Count;
            var newNode = new Node(index);
            _nodes.Add(newNode);
            return newNode;
        }

        public Node GetNode(int nodeIndex)
        {
            var got = _nodes.ElementAtOrDefault(nodeIndex);
            if (got == null) return null;
            Assert.IsTrue(got.NodeIndex == nodeIndex);
            return got;
        }

        public int NodeCount => _nodes.Count;

        private readonly List<Node> _nodes = new List<Node>();
    }
}