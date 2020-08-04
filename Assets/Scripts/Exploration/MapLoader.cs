using System.Collections.Generic;
using GuildMaster.Tools;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;

    /// <summary>
    /// ExplorationMap을 받아 지도를 볼 수 있는 오브젝트들로 옮깁니다. <br/>
    /// map의 노드마다 TNodeSprite를 생성하고, DrawEdge가 참일 경우연결된 노드 사이에 EdgeRenderer을 이용해 선을 긋습니다.
    /// </summary>
    /// <typeparam name="TNodeSprite"> map의 Locations 위치마다 생성될 버튼/스프라이트 타입 </typeparam>
    public class MapLoader<TNodeSprite> where TNodeSprite : MonoBehaviour, INodeRepresentative<MapNode>
    {
        public SpriteRenderer BackgroundRenderer;
        public TNodeSprite NodeSpritePrefab;
        public bool DrawEdgeUsingEdgeRenderer;
        public LineRenderer EdgeRendererPrefab;
        public bool DrawEdgeUsingSprite;
        public SpriteRenderer EdgeSpritePrefab;

        /// <summary>
        /// 축척. 그래프에서의 좌표 * MapScale = 실제 생성되는 오브젝트 좌표.
        /// </summary>
        public float MapScale = 1f;

        public List<TNodeSprite> LoadMap(ExplorationMap map)
        {
            var nodeSprites = new List<TNodeSprite>();

            BackgroundRenderer.sprite = map.Background;

            foreach (var node in map.Graph.Nodes)
            {
                var newButton = Object.Instantiate(NodeSpritePrefab, BackgroundRenderer.transform);
                newButton.transform.localPosition = node.Content.Position * MapScale;
                newButton.SetNode(node);

                foreach (var ns in nodeSprites)
                {
                    if (!node.Connected.Exists(ind => map.Graph.GetNode(ind) == ns.Node)) continue;

                    if (DrawEdgeUsingEdgeRenderer)
                    {
                        var edgeLine = Object.Instantiate(EdgeRendererPrefab, newButton.transform);
                        edgeLine.transform.localPosition = Vector3.zero;
                        edgeLine.SetPositions(
                            new[] {Vector3.zero, ns.transform.position - newButton.transform.position});
                    }

                    if (DrawEdgeUsingSprite)
                    {
                        var edgeSprite = Object.Instantiate(EdgeSpritePrefab, newButton.transform);
                        Vector2 v1 = newButton.transform.position;
                        Vector2 v2 = ns.transform.position;

                        var edgeTrans = edgeSprite.transform;
                        edgeTrans.position = (v1 + v2) / 2;

                        var angles = edgeTrans.eulerAngles;;
                        angles.z = Mathf.Atan2(v2.y-v1.y, v2.x-v1.x)*180 / Mathf.PI;
                        edgeTrans.eulerAngles = angles;


                        var size = edgeSprite.size;
                        size.x = Vector2.Distance(newButton.transform.localPosition, ns.transform.localPosition);
                        edgeSprite.size = size;
                        
                    }
                }

                nodeSprites.Add(newButton);
            }

            return nodeSprites;
        }
    }
}