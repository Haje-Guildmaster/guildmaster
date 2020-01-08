using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.MapRoam
{
    [CreateAssetMenu(fileName = "map", menuName = "ScriptableObjects/MapRoam/Map", order = 1)]
    public class Map: ScriptableObject
    {
        [SerializeField] private List<Place> places;
    }
}