using System.Collections.Generic;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// 지도 뷰에서 제일 기본적이고 공통적인 부분, 즉 지도 그래프의 각 노드마다 노드 오브젝트를 생성하고 연결된 부분에는
    /// edge오브젝트를 생성해서 보여주는
    /// 컴포넌트.
    /// </summary>
    /// <typeparam name="TNodeSprite"> 생성되는 Node 타입 </typeparam>
    /// <typeparam name="TEdgeSprite"> 생성되는 Edge 타입 </typeparam>
    [RequireComponent(typeof(MapViewFrame))]
    public class BasicMapView<TNodeSprite, TEdgeSprite> : MonoBehaviour
        where TNodeSprite : MonoBehaviour, INodeRepresentative<MapNode> where TEdgeSprite : MonoBehaviour
    {
        public TNodeSprite NodeSpritePrefab;
        public bool DrawEdge;
        public TEdgeSprite EdgeSpritePrefab;


        public IEnumerable<TNodeSprite> Nodes => _nodes;
        public IEnumerable<TEdgeSprite> Edges => _edges;
        public Image BackgroundImage => _mapViewFrame.BackgroundImage;
        public Mask Mask => _mapViewFrame.Mask;
        
        /// <summary>
        /// 축척. 그래프에서의 좌표 * MapScale = 실제 생성되는 오브젝트 좌표.
        /// </summary>
        public float MapScale = 1f;


        private void Awake()
        {
            _mapViewFrame = GetComponent<MapViewFrame>();
        }

        public void LoadMap(ExplorationMap map)
        {
            Cleanup();

            var frame = _mapViewFrame;
            Assert.IsTrue(frame.EdgesParent.lossyScale ==
                          frame.NodesParent.lossyScale); // Vector3의 ==은 내부적으로 소수점 오류 처리가 됨.


            frame.BackgroundImage.sprite = map.Background;

            foreach (var node in map.Graph.Nodes)
            {
                var newButton = Instantiate(NodeSpritePrefab, frame.NodesParent);
                newButton.transform.localPosition = node.Content.Position * MapScale;
                newButton.SetNode(node);

                foreach (var ns in _nodes)
                {
                    if (!DrawEdge || !node.Connected.Exists(ind => map.Graph.GetNode(ind) == ns.Node)) continue;

                    var edgeSprite = Instantiate(EdgeSpritePrefab, frame.EdgesParent);
                    Vector2 v1 = newButton.transform.position;
                    Vector2 v2 = ns.transform.position;

                    var edgeTrans = edgeSprite.GetComponent<RectTransform>();
                    edgeTrans.position = (v1 + v2) / 2;

                    var angles = edgeTrans.eulerAngles;

                    angles.z = Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * 180 / Mathf.PI;
                    edgeTrans.eulerAngles = angles;


                    edgeTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                        Vector2.Distance(newButton.transform.localPosition, ns.transform.localPosition));

                    _edges.Add(edgeSprite);
                }

                _nodes.Add(newButton);
            }
        }


        public void Cleanup()
        {
            foreach (var ns in _nodes)
            {
                Destroy(ns.gameObject);
            }

            foreach (var es in _edges)
            {
                Destroy(es.gameObject);
            }

            _nodes.Clear();
            _edges.Clear();
        }

        private MapViewFrame _mapViewFrame;
        private readonly List<TNodeSprite> _nodes = new List<TNodeSprite>();
        private readonly List<TEdgeSprite> _edges = new List<TEdgeSprite>();
    }
}