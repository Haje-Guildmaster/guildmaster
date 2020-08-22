using System;
using System.Threading.Tasks;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    public class MapBaseSelector : MonoBehaviour
    {
        [SerializeField] private Color _etcColor;
        [SerializeField] private Color _baseNormalColor;
        [SerializeField] private Color _baseMouseOnColor;
        [SerializeField] private Color _basePressedColor;

        public async Task<Graph<ExplorationMap.NodeContent>.Node> Select(MapSelectView mapSelectView)
        {
            mapSelectView.ColorLocationButtons(node =>
                node.Content.Location.LocationType == Location.Type.Base
                    ? (_baseNormalColor, _baseMouseOnColor, _basePressedColor)
                    : (_etcColor, _etcColor, _etcColor)
            );
            var ret = await mapSelectView.Select(node => node.Content.Location.LocationType == Location.Type.Base);
            ResetColors(mapSelectView);
            return ret;
        }

        private static void ResetColors(MapSelectView mapSelectView)
        {
            mapSelectView.ColorLocationButtons(_=>(Color.white, Color.white, Color.white));
        }
    }
}