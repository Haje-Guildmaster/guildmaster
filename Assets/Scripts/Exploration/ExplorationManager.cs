using System;
using System.Collections.Generic;
using GuildMaster.Characters;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration
{
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private ExplorationView _explorationView;

        public void StartExploration(int length, List<Character> characters)
        {
            // Todo:
            // _explorationView.Setup(characters, );
        }


        private static ExplorationManager _instance;

        public static ExplorationManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<ExplorationManager>());
    }
}