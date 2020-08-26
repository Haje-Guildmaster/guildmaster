using System;
using System.Linq;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    public class MapAdjacentSelector : MonoBehaviour
    {
        [SerializeField] private Color _etcColor;
        [SerializeField] private Color _startLocationColor;
        [SerializeField] private Color _targetNormalColor;
        [SerializeField] private Color _targetMouseOnColor;
        [SerializeField] private Color _targetPressedColor;


        public void Select(MapSelectView mapSelectView, Graph<ExplorationMap.NodeContent>.Node startingNode,
            Action<Graph<ExplorationMap.NodeContent>.Node> callBack)
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
            mapSelectView.Select(node => startingNode.Connected.Exists(ind => graph.GetNode(ind) == node),
                ret =>
                {
                    ResetColors(mapSelectView);
                    callBack(ret);
                });
        }

        private static void ResetColors(MapSelectView mapSelectView)
        {
            mapSelectView.ColorLocationButtons(_ => (Color.white, Color.white, Color.white));
        }
    }
}