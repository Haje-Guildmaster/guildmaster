using System;
using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.TownRoam
{
    public class TownLoader: MonoBehaviour
    {
        [SerializeField] private PlaceViewer placeViewer;
        
        public static T Load<T>(T town)where T: Town => Instance._Load(town);
        
        
        private T _Load<T>(T town, params TownModifier[] modifiers) where T: Town
        {
            var townObj = Instantiate(town);
            foreach (var modifier in modifiers)
            {
                modifier.Modify(townObj);
            }
            placeViewer.Goto(townObj.Entrance);
            return townObj;
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