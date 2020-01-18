using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuildMaster.TownRoam.Towns
{
    public abstract class Town: MonoBehaviour
    {
        public abstract Place Entrance { get; }

        public bool IsTypeOf<T>() => GetType() == typeof(T);
        
        [Obsolete]
        public Place GetPlaceByName(string placeName)
        {
            return GetComponentsInChildren<Place>().FirstOrDefault(p => p.placeName == placeName);
        }
    }
}