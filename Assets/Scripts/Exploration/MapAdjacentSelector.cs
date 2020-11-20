using System;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// MapSelectView를 받아 유저가 MapSelectView에서 특정 노드의 옆 노드를 선택하도록 하는 클래스 
    /// </summary>
    [Serializable]
    public class MapAdjacentSelector
    {
        [Header("[MapAdjacentSelector]")]
        [SerializeField] private Color _etcColor;
        [SerializeField] private Color _startLocationColor;
        [SerializeField] private Color _targetNormalColor;
        [SerializeField] private Color _targetMouseOnColor;
        [SerializeField] private Color _targetPressedColor;


        /// <summary>
        /// startingNode와 1칸 거리에 있는 노드 하나를 선택하도록 합니다. 그 노드를 반환합니다.
        /// </summary>
        /// <param name="mapSelectView"> 지도 뷰 오브젝트 </param>
        /// <param name="startingNode"> 거리 기준 </param>
        /// <returns> 선택된 노드 </returns>
        public async Task<Graph<ExplorationMap.NodeContent>.Node> Select(MapSelectView mapSelectView, Graph<ExplorationMap.NodeContent>.Node startingNode)
        {
            var graph = mapSelectView.Graph;
            Assert.IsTrue(graph.Nodes.Contains(startingNode));

            mapSelectView.ColorLocationButtons(node =>
                node == startingNode
                    ? (_startLocationColor, _startLocationColor, _startLocationColor)
                    : startingNode.Connected.Exists(ind => graph.GetNode(ind) == node)
                        ? (_targetNormalColor, _targetMouseOnColor, _targetPressedColor)
                        : (_etcColor, _etcColor, _etcColor)
            );
            var ret = await mapSelectView.Select(node =>
                startingNode.Connected.Exists(ind => graph.GetNode(ind) == node));
            ResetColors(mapSelectView);
            return ret;
        }

        private static void ResetColors(MapSelectView mapSelectView)
        {
            mapSelectView.ColorLocationButtons(_ => (Color.white, Color.white, Color.white));
        }
    }
}