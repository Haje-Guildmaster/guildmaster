using System.Collections.Generic;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Exploration
{
    using MapNode = Graph<ExplorationMap.NodeContent>.Node;
    /// <summary>
    /// ExplorationMap을 받아 지도를 볼 수 있는 오브젝트들로 옮깁니다. <br/>
    /// map의 노드마다 TNodeSprite를 생성하고, DrawEdge가 참일 경우연결된 노드 사이에 EdgeRenderer을 이용해 선을 긋습니다.
    /// </summary>
    /// <typeparam name="TNodeSprite"> map의 Locations 위치마다 생성될 버튼/스프라이트 타입 </typeparam>
    public class MapLoader<TNodeSprite> where TNodeSprite: MonoBehaviour, INodeRepresentative<MapNode>
    {
        public SpriteRenderer BackgroundRenderer;
        public TNodeSprite NodeSpritePrefab;
        public bool DrawEdge;
        public LineRenderer EdgeRendererPrefab;
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
                    if (DrawEdge && node.Connected.Exists(ind => map.Graph.GetNode(ind) == ns.Node))
                    {
                        var edge = Object.Instantiate(EdgeRendererPrefab, newButton.transform);
                        edge.transform.localPosition = Vector3.zero;
                        edge.SetPositions(new[] {Vector3.zero, ns.transform.position - newButton.transform.position});
                    }
                }

                nodeSprites.Add(newButton);
            }

            return nodeSprites;
        }
        
    }
}