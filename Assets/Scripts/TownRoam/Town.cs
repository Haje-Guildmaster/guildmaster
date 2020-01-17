using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    public class Town: MonoBehaviour
    {
        public Place GetPlace(string placeName)
        {
            var p = _placeDictionary[placeName];
            if (p == null)
                throw new Exception(string.Format("Place with name \"{0}\" does not exist.", placeName));
            return p;
        }
        
        // private void Start()
        // {
            // UpdateDictionary();
        // }

        private void OnTransformChildrenChanged()
        {
            UpdateDictionary();
        }

        private Dictionary<string, Place> _placeDictionary;
        private void UpdateDictionary()
        {
            _placeDictionary = GetComponentsInChildren<Place>().ToDictionary(p => p.placeName, p => p);
        }
    }
}