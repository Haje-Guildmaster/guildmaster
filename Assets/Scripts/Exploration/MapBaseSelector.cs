using System;
using GuildMaster.Tools;
using UnityEngine;

namespace GuildMaster.Exploration
{
    public class MapBaseSelector : MonoBehaviour
    {
        [SerializeField] private Color etcColor;
        [SerializeField] private Color BaseNormalColor;
        [SerializeField] private Color BaseMouseOnColor;
        [SerializeField] private Color BasePressedColor;

        public void Select(MapView mapView, Action<Graph<ExplorationMap.NodeContent>.Node> callBack)
        {
            mapView.ColorLocationButtons(node =>
                node.Content.Location.LocationType == Location.Type.Base
                    ? (BaseNormalColor, BaseMouseOnColor, BasePressedColor)
                    : (Color.white, Color.white, Color.white)
            );
            mapView.Select(node => node.Content.Location.LocationType == Location.Type.Base,
                ret =>
                {
                    ResetColors(mapView);
                    callBack(ret);
                });
        }

        private static void ResetColors(MapView mapView)
        {
            mapView.ColorLocationButtons(_=>(Color.white, Color.white, Color.white));
        }
    }
}