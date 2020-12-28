using System;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    /// <summary>
    /// MapSelectView를 받아 유저가 MapSelectView에서 베이스캠프 중 하나를 선택하도록 하는 클래스 
    /// </summary>
    [Serializable]
    public class MapBaseSelector
    {
        [Header("[MapBaseSelector]")]
        [SerializeField] private Color _etcColor;
        [SerializeField] private Color _baseNormalColor;
        [SerializeField] private Color _baseMouseOnColor;
        [SerializeField] private Color _basePressedColor;
        
        /// <summary>
        /// 베이스캠프 노드 하나를 선택하도록 합니다. 그 노드를 반환합니다.
        /// </summary>
        /// <param name="mapSelectView"> 지도 뷰 오브젝트 </param>
        /// <<param name="cancelToken"> CancellationToken </param>
        /// <returns> 선택된 베이스캠프 노드 </returns>
        public async Task<Graph<ExplorationMap.NodeContent>.Node> Select(MapSelectView mapSelectView, CancellationToken cancelToken=default)
        {
            mapSelectView.ColorLocationButtons(node =>
                node.Content.Location.LocationType == Location.Type.Base
                    ? (_baseNormalColor, _baseMouseOnColor, _basePressedColor)
                    : (_etcColor, _etcColor, _etcColor)
            );


            Graph<ExplorationMap.NodeContent>.Node ret;
            try
            {
                ret = await mapSelectView.Select(
                    node => node.Content.Location.LocationType == Location.Type.Base, cancelToken);
            }
            finally
            {
                ResetColors(mapSelectView);
            }

            return ret;
        }

        private static void ResetColors(MapSelectView mapSelectView)
        {
            mapSelectView.ColorLocationButtons(_=>(Color.white, Color.white, Color.white));
        }
    }
}