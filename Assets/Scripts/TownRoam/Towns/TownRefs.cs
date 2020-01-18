using System;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.TownRoam.Towns
{
    public class TownRefs: MonoBehaviour
    {
        [SerializeField] private TestTown testTown;
        public static TestTown TestTown => Instance.testTown;
        
        
        private static TownRefs _instance;
        private static TownRefs Instance 
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = FindObjectOfType<TownRefs>();  
                if (!_instance)
                    throw new Exception("There needs to be an active Towns component in the scene");
                return _instance;
            }  
        }  
    }
}