using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuildMaster.Exploration
{
    public class ExplorationLoader : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (!_reservation.HasValue) return;

                var (len, characters) = _reservation.Value;
                _reservation = null;
                ExplorationManager.Instance.StartExploration(len, characters, FindObjectOfType<ExplorationDebugger>()._map);    // Todo:
            };
        }

        [Obsolete]
        public void Load(List<Character> characters)
        {
            _reservation = (5, characters);
            SceneManager.LoadScene("ExplorationScene");
        }
        
        public void Load(List<Character> characters, Inventory inventory)
        {
            throw new NotImplementedException();
        }

        private (int length, List<Character> characters)? _reservation; //length는 임시.

        private static ExplorationLoader _instance;
        public static ExplorationLoader Instance => _instance!=null ? _instance : (_instance = FindObjectOfType<ExplorationLoader>());
    }
}