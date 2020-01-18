using System;
using GuildMaster.TownRoam.Towns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.TownRoam
{
    public class TownLoader: MonoBehaviour
    {
        [SerializeField] private PlaceViewer placeViewer;
        
        public static void Load(Town town) => Instance._Load(town);
        
        
        private void _Load(Town town)
        {
            var townObj = Instantiate(town);
            placeViewer.Goto(townObj.Entrance);
        }
        
        private static TownLoader _instance;

        private static TownLoader Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = FindObjectOfType<TownLoader>();
                if (!_instance)
                    throw new Exception("There needs to be an active TownLoader in the scene");
                return _instance;
            }
        }
        
        
    }
}