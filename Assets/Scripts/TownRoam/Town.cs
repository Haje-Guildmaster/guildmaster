using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    [CreateAssetMenu(fileName = "map", menuName = "ScriptableObjects/MapRoam/Map", order = 1)]
    public class Town: ScriptableObject
    {
        [SerializeField] private List<Place> places;
    }
}